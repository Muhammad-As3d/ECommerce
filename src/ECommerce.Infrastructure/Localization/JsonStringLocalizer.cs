using Microsoft.Extensions.Localization;
using System.Globalization;

namespace ECommerce.Infrastructure.Localization;

public class JsonStringLocalizer : IStringLocalizer
{
    private readonly Dictionary<string, Dictionary<string, string>> _resources = new(StringComparer.OrdinalIgnoreCase);
    private readonly CultureInfo _defaultCulture;

    public JsonStringLocalizer(string resourcesPath, string defaultCulture = "en")
    {
        _defaultCulture = CultureInfo.GetCultureInfo(defaultCulture);

        foreach (var file in Directory.EnumerateFiles(resourcesPath, "*.json"))
        {
            var culture = CultureInfo.GetCultureInfo(Path.GetFileNameWithoutExtension(file));
            var translations = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(file))
                ?? throw new InvalidOperationException($"Translation file '{file}' must contain a JSON object.");

            if (translations.Values.Any(value => value is null))
                throw new InvalidOperationException($"Translation file '{file}' contains null values.");

            _resources.Add(culture.Name, translations);
        }

        if (!_resources.ContainsKey(_defaultCulture.Name))
            throw new InvalidOperationException($"Default translation file '{defaultCulture}.json' is missing.");
    }

    public LocalizedString this[string name] => Find(name);

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var localized = Find(name);
            return new LocalizedString(name, string.Format(CultureInfo.CurrentCulture, localized.Value, arguments), localized.ResourceNotFound);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var cultures = includeParentCultures ? GetLookupCultures() : [CultureInfo.CurrentUICulture];
        var seen = new HashSet<string>(StringComparer.Ordinal);

        foreach (var culture in cultures)
        {
            if (!_resources.TryGetValue(culture.Name, out var translations))
                continue;

            foreach (var (key, value) in translations)
            {
                if (seen.Add(key))
                    yield return new LocalizedString(key, value);
            }
        }
    }

    private LocalizedString Find(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        foreach (var culture in GetLookupCultures())
        {
            if (_resources.TryGetValue(culture.Name, out var translations)
                && translations.TryGetValue(name, out var value))
                return new LocalizedString(name, value);
        }

        return new LocalizedString(name, name, resourceNotFound: true);
    }

    private IEnumerable<CultureInfo> GetLookupCultures()
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var start in new[] { CultureInfo.CurrentUICulture, _defaultCulture })
        {
            for (var culture = start; !string.IsNullOrEmpty(culture.Name); culture = culture.Parent)
            {
                if (seen.Add(culture.Name))
                    yield return culture;
            }
        }
    }
}
