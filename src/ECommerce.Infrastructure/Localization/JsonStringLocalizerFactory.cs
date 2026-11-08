using Microsoft.Extensions.Localization;

namespace ECommerce.Infrastructure.Localization;

public class JsonStringLocalizerFactory(JsonStringLocalizer localizer) : IStringLocalizerFactory
{
    public IStringLocalizer Create(Type resourceSource) => localizer;

    public IStringLocalizer Create(string baseName, string location) => localizer;
}
