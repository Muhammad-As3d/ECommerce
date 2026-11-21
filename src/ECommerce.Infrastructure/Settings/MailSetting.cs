namespace ECommerce.Infrastructure.Settings;

public class MailSetting
{
    [Required, MaxLength(100)]
    public string Mail { get; set; } = string.Empty;
    [Required, MaxLength(100)]
    public string DisplayName { get; set; } = string.Empty;
    [Required, MaxLength(100)]
    public string Password { get; set; } = string.Empty;
    [Required, MaxLength(100)]
    public string Host { get; set; } = string.Empty;
    [Required]
    public int Port { get; set; }
}
