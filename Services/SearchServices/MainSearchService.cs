using ApiTestTask.Services.SearchDataProviderServices;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel;

namespace ApiTestTask.Services.SearchServices {
    public class MainSearchService : ISearchService
    {
        public const string RoutesCacheKey = "Routes";
        private readonly IEnumerable<ISearchDataProviderService> _services;
        private readonly IMemoryCache _memoryCache;

        public MainSearchService(IEnumerable<ISearchDataProviderService> services, IMemoryCache memoryCache) {
            _services = services;
            _memoryCache = memoryCache;
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
            IEnumerable<Route> routes = null!;
            if (request.Filters?.OnlyCached == true) {
                if (!_memoryCache.TryGetValue(RoutesCacheKey, out routes)) { throw new Exception("Empty cache"); }
            } else {
                var converter = TypeDescriptor.GetConverter(typeof(ProviderRequest));
                var targetRequest = (ProviderRequest?)converter.ConvertFrom(request) ?? throw new Exception("Cannot convert request");
                var routeTasks = new List<Task<ProviderResponse>>();
                foreach (var service in _services)
                {
                    routeTasks.Add(service.SearchAsync(targetRequest, cancellationToken));
                }
                await Task.WhenAll(routeTasks);
                routes = routeTasks.SelectMany(t => t.Result.Routes);
                _memoryCache.Set(RoutesCacheKey, routes, DateTimeOffset.FromUnixTimeSeconds(3*60*60));
            }
            return new RouteWorker(routes)
                        .CheckAny()
                        .RemoveNotActual()
                        .CalculateResponse();
        }
    }

    class RouteWorker {

        public RouteWorker(IEnumerable<Route> targetRoutes) {
            _targetRoutes = targetRoutes;
        }

        public RouteWorker CheckAny() {
            if (!_targetRoutes.Any()) { throw new Exception("Empty providers request"); }
            return this;
        }

        public RouteWorker RemoveNotActual() {
            _targetRoutes = _targetRoutes.Where(r => r.TimeLimit < DateTime.Now);
            return this;
        }

        public RouteWorker ApplyFilter(SearchFilters? filter) {
            if (filter == null) { return this; }

            if (filter.MaxPrice != null) {
                _targetRoutes = _targetRoutes.Where(r => r.Price < filter.MaxPrice);
            }
            if (filter.MinTimeLimit != null) {
                _targetRoutes = _targetRoutes.Where(r => r.TimeLimit > filter.MinTimeLimit);
            }
            if (filter.DestinationDateTime != null) {
                _targetRoutes = _targetRoutes.Where(r => r.DestinationDateTime < filter.DestinationDateTime);
            }

            return this;
        }

        public SearchResponse CalculateResponse() {
            decimal maxPrice = decimal.MinValue;
            decimal minPrice = decimal.MaxValue;
            int maxMinutes = int.MinValue;
            int minMinutes = int.MaxValue;
            foreach (var route in _targetRoutes) {
                if (route.Price > maxPrice) { maxPrice = route.Price; }
                if (route.Price < minPrice) { minPrice = route.Price; }
                var routeTime = route.OriginDateTime - route.DestinationDateTime;
                if (routeTime.TotalMinutes > maxMinutes) { maxMinutes = (int)routeTime.TotalMinutes; }
                if (routeTime.TotalMinutes < minMinutes) { minMinutes = (int)routeTime.TotalMinutes; }
            }
            return new SearchResponse() {
                Routes = _targetRoutes.ToArray(),
                MaxPrice = maxPrice,
                MinPrice = minPrice,
                MaxMinutesRoute = maxMinutes,
                MinMinutesRoute = minMinutes
            };
        }

        private IEnumerable<Route> _targetRoutes;
    }
}
