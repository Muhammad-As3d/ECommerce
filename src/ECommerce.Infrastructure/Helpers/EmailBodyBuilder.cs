using System.Net;
using System.Reflection;

namespace ECommerce.Infrastructure.Helpers;

public static class EmailBodyBuilder
{
    private static readonly Assembly Assembly = typeof(EmailBodyBuilder).Assembly;

    public static string Generate(string templateName, IDictionary<string, string> values)
    {
        var resourceName = $"ECommerce.Infrastructure.Templates.{templateName}.html";

        using var stream = Assembly.GetManifestResourceStream(resourceName)
            ?? throw new FileNotFoundException($"Email template '{templateName}' was not found.", resourceName);

        using var reader = new StreamReader(stream);
        var body = reader.ReadToEnd();

        foreach (var (key, value) in values)
            body = body.Replace($"{{{{{key}}}}}", WebUtility.HtmlEncode(value));

        return body;
    }
}
