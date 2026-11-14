namespace ECommerce.Infrastructure.Identity.Settings;

public class AppSettings
{
    public static readonly string SectionName = "AppSettings";

    [Required, MaxLength(100)]
    public string Name { get; init; } = string.Empty;

    [Required, Url]
    public string FrontendUrl { get; init; } = string.Empty;
}
