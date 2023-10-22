using System.ComponentModel;

namespace ApiTestTask.Services.SearchDataProviderServices;

[TypeConverter(typeof(ProviderRequest))]
public class ProviderRequest
{
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public DateTime DateFrom { get; set; }
    public ProviderFilters Filter { get; set; } = new();
}

public class ProviderResponse
{
    public ProviderResponse(SearchServices.Route[] routes)
    {
        Routes = routes;
    }

    public SearchServices.Route[] Routes { get; set; }
}

public class ProviderFilters
{
    public DateTime? DestinationDateTime { get; set; }

    // Optional
    // Maximum price of route
    public decimal? MaxPrice { get; set; }

    // Optional
    // Minimum value of timelimit for route
    public DateTime? MinTimeLimit { get; set; }
}
