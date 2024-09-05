using Newtonsoft.Json;

namespace BlogClient.APIRoutes
{
    public class ApiRoute
    {
        public string Method { get; set; }
        public string Route { get; set; }
        public string Action { get; set; }
    }

    public class ApiRoutesService
    {
        private readonly IReadOnlyList<ApiRoute> _routes;
        private readonly string _baseUrl = "https://localhost:7175";

        public ApiRoutesService()
        {
            var json = File.ReadAllText("APIRoutes/apiRoutes.json");
            _routes = JsonConvert.DeserializeObject<List<ApiRoute>>(json).AsReadOnly();
        }



        public ApiRoute GetRoute(string action)
        {
            var apiRoute = _routes.FirstOrDefault(r => r.Action == action);
            if (apiRoute != null)
            {
                apiRoute.Route = $"{_baseUrl}{apiRoute.Route}";
            }
            return apiRoute;

        }

        public ApiRoute GetRouteByMethodAndRoute(string method, string route)
        {
            var apiRoute = _routes.FirstOrDefault(r => r.Method == method && r.Route == route);
            if (apiRoute != null)
            {
                apiRoute.Route = $"{_baseUrl}{apiRoute.Route}";
            }
            return apiRoute;
        }
    }
}
