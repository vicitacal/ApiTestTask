using ApiTestTask.Services.SearchDataProviderServices;
using System.ComponentModel;

namespace ApiTestTask.Services.SearchServices
{
    public class MainSearchService : ISearchService
    {
        private readonly IEnumerable<ISearchDataProviderService> _services;

        public MainSearchService(IEnumerable<ISearchDataProviderService> services) {
            _services = services;
        }

        public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            foreach (var service in _services)
            {
                if (await  service.IsAvailableAsync(cancellationToken)) { return true; }
            }
            return false;
        }

        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            var converter = TypeDescriptor.GetConverter(typeof(ProviderRequest));
            var targetRequest = (ProviderRequest?)converter.ConvertFrom(request) ?? throw new Exception("Cannot convert request");
            var routeTasks = new List<Task<ProviderResponse>>();
            foreach (var service in _services)
            {
                routeTasks.Add(service.SearchAsync(targetRequest, cancellationToken));
            }
            await Task.WhenAll(routeTasks);
            IEnumerable<Route> routes = routeTasks.SelectMany(t => t.Result.Routes);
            if (!routes.Any()) { throw new Exception("Empty providers request"); }
            return CalculateResponce(routes);
        }

        private SearchResponse CalculateResponce(IEnumerable<Route> routes)
        {
            decimal maxPrice = decimal.MinValue;
            decimal minPrice = decimal.MaxValue;
            int maxMinutes = int.MinValue;
            int minMinutes = int.MaxValue;
            foreach (var route in routes)
            {
                if (route.Price > maxPrice) { maxPrice = route.Price; }
                if (route.Price < minPrice) { minPrice = route.Price; }
                var routeTime = route.OriginDateTime - route.DestinationDateTime;
                if (routeTime.TotalMinutes > maxMinutes) { maxMinutes = (int)routeTime.TotalMinutes; }
                if (routeTime.TotalMinutes < minMinutes) { minMinutes = (int)routeTime.TotalMinutes; }
            }
            return new SearchResponse()
            {
                Routes = routes.ToArray(),
                MaxPrice = maxPrice,
                MinPrice = minPrice,
                MaxMinutesRoute = maxMinutes,
                MinMinutesRoute = minMinutes
            };
        }
    }
}
