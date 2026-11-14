using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Application.Contracts.Users;
using Mapster;

namespace ECommerce.Infrastructure.Implementations.Authentication;

internal class UserService(UserManager<User> userManager,
    IRoleService roleService,
    ApplicationDbContext context) : IUserService
{
    public async Task<PaginatedList<UserResponse>> GetAllAsync(UserFiltersRequest filter, CancellationToken cancellationToken = default)
    {
        var query = context.Users
            .AsNoTracking()
            .Where(u => !context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == DefaultRoles.Admin.Id));

        query = ApplySearch(query, filter.SearchValue);
        query = ApplyFilters(query, filter);
        query = ApplySorting(query, filter);

        var source = query.Select(u => new UserResponse
        (
              u.Id,
              u.FirstName,
              u.LastName,
              u.Email!,
              u.IsDisabled,
              (from ur in context.UserRoles
               join r in context.Roles on ur.RoleId equals r.Id
               where ur.UserId == u.Id
               select r.Name).ToList()
        ));

        return await source.ToPaginatedListAsync(filter.PageNumber, filter.PageSize, cancellationToken);

        //var source = (from u in context.Users
        //              join ur in context.UserRoles 
        //              on u.Id equals ur.UserId
        //              join r in context.Roles
        //              on ur.RoleId equals r.Id into roles
        //              where !roles.Any(x => x.IsDefault)
        //              select new
        //              {
        //                  u.Id,
        //                  u.FirstName,
        //                  u.LastName,
        //                  u.Email,
        //                  u.IsDisabled,
        //                  Roles = roles.Select(x => x.Name).ToList()!
        //              }
        //        )
        //        .GroupBy(u => new { u.Id, u.FirstName, u.LastName, u.Email, u.IsDisabled })
        //        .Select(u => new UserResponse
        //        (
        //            u.Key.Id,
        //            u.Key.FirstName,
        //            u.Key.LastName,
        //            u.Key.Email,
        //            u.Key.IsDisabled,
        //            u.SelectMany(x => x.Roles)
        //        ));

        //return await source.ToPaginatedListAsync(filter.PageNumber, filter.PageSize, cancellationToken);
    }

    public async Task<Result<UserResponse>> GetAsync(string id)
    {
        if (await userManager.FindByIdAsync(id) is not { } user)
            return UserErrors.NotFound(id);

        var userRoles = await userManager.GetRolesAsync(user);

        var response = new UserResponse(user.Id, user.FirstName, user.LastName, user.Email!, user.IsDisabled, userRoles);

        return Result.Success(response);
    }

    public async Task<Result<UserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var emailIsExists = await userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

        if (emailIsExists)
            return UserErrors.DuplicatedEmail;

        var allowedRoles = await roleService.GetAllAsync(cancellationToken: cancellationToken);

        if (request.Roles.Except(allowedRoles.Select(x => x.Name)).Any())
            return UserErrors.InvalidRoles;

        var user = request.Adapt<User>();

        user.UserName = request.Email;
        user.EmailConfirmed = true;

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return result.ToError();

        await userManager.AddToRolesAsync(user, request.Roles);

        var response = new UserResponse(user.Id, user.FirstName, user.LastName, user.Email!, user.IsDisabled, request.Roles);

        return Result.Success(response);
    }

    public async Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user is null)
            return UserErrors.NotFound(id);

        if (user.IsDisabled)
            return UserErrors.IsDisabled;

        var emailIsExists = await userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id, cancellationToken);

        if (emailIsExists)
            return UserErrors.DuplicatedEmail;

        var allowedRoles = await roleService.GetAllAsync(cancellationToken: cancellationToken);

        if (request.Roles.Except(allowedRoles.Select(x => x.Name)).Any())
            return UserErrors.InvalidRoles;

        user = request.Adapt(user);
        user.UserName = request.Email;
        user.NormalizedUserName = request.Email.ToUpper();

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return result.ToError();

        await context.UserRoles
            .Where(x => x.UserId == id)
            .ExecuteDeleteAsync(cancellationToken);

        await userManager.AddToRolesAsync(user, request.Roles);

        return Result.Success();
    }

    public async Task<Result> ToggleStatusAsync(string id, CancellationToken cancellationToken = default)
    {
        if (await userManager.FindByIdAsync(id) is not { } user)
            return UserErrors.NotFound(id);

        user.IsDisabled = !user.IsDisabled;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return result.ToError();

        return Result.Success();
    }

    public async Task<Result> UnlockAsync(string id)
    {
        if (await userManager.FindByIdAsync(id) is not { } user)
            return UserErrors.NotFound(id);

        var result = await userManager.SetLockoutEndDateAsync(user, null);

        if (!result.Succeeded)
            return result.ToError();

        return Result.Success();
    }

    private static IQueryable<User> ApplySearch(IQueryable<User> query, string? searchValue)
    {
        if (string.IsNullOrWhiteSpace(searchValue))
            return query;

        var terms = searchValue.Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Take(5);

        foreach (var term in terms)
        {
            query = query.Where(u =>
                u.FirstName.Contains(term) ||
                u.LastName.Contains(term) ||
                u.Email!.Contains(term));
        }

        return query;
    }

    private IQueryable<User> ApplyFilters(IQueryable<User> query, UserFiltersRequest filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Role))
        {
            var normalizedRole = filter.Role.Trim().ToUpperInvariant();

            query = query.Where(u =>
                (from ur in context.UserRoles
                 join r in context.Roles on ur.RoleId equals r.Id
                 where ur.UserId == u.Id && r.NormalizedName == normalizedRole
                 select ur).Any());
        }

        if (filter.DefaultRole.HasValue)
        {
            var wantDefault = filter.DefaultRole.Value;

            query = query.Where(u =>
                (from ur in context.UserRoles
                 join r in context.Roles on ur.RoleId equals r.Id
                 where ur.UserId == u.Id && r.IsDefault
                 select ur).Any() == wantDefault);
        }

        if (filter.IsDisabled.HasValue)
            query = query.Where(u => u.IsDisabled == filter.IsDisabled.Value);

        return query;
    }

    private static IQueryable<User> ApplySorting(IQueryable<User> query, UserFiltersRequest filter)
    {
        Expression<Func<User, object>> key = filter.SortColumn?.Trim().ToLowerInvariant() switch
        {
            "firstname" => u => u.FirstName,
            "lastname" => u => u.LastName,
            "email" => u => u.Email!,
            _ => u => u.Id
        };

        return filter.IsDescending ? query.OrderByDescending(key) : query.OrderBy(key);
    }
}