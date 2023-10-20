using ApiTestTask.Services.SearchServices;

namespace ApiTestTask.Services.SearchDataProviderServices; 

public class ProviderRequest {
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public DateTime DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public SearchFilters Filter { get; set; } = new();
}

public class ProviderResponse {
    public ProviderResponse(ProviderRoute[] routes) {
        Routes = routes;
    }

    public ProviderRoute[] Routes { get; set; }
}

public class ProviderRoute {
    public ProviderRoute(RoutePoint departure, RoutePoint arrival, decimal price, DateTime timeLimit) {
        Departure = departure;
        Arrival = arrival;
        Price = price;
        TimeLimit = timeLimit;
    }

    public RoutePoint Departure { get; set; }
    public RoutePoint Arrival { get; set; }
    public decimal Price { get; set; }
    public DateTime TimeLimit { get; set; }
}

public class RoutePoint {
    public string Point { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
