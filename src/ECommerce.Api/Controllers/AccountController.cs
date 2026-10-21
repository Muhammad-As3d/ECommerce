using ECommerce.Api.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("me")]
[ApiController]
[Authorize]
public class AccountController : ApiBaseController
{
}
