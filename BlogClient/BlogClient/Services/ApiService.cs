
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace BlogClient.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void SetAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("AccessToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        public async Task<ApiResponse<T>> GetAsync<T>(string url)
        {
            SetAuthorizationHeader();
            var apiResponse = new ApiResponse<T>();

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    apiResponse.Data = JsonConvert.DeserializeObject<T>(jsonResponse);
                    apiResponse.Success = true;
                }
                else
                {
                    apiResponse.Success = false;
                    apiResponse.ErrorMessage = GetErrorMessage(response.StatusCode);
                    apiResponse.StatusCode = response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.ErrorMessage = "API service is unreachable.";
                apiResponse.StatusCode = HttpStatusCode.InternalServerError;
            }

            return apiResponse;
        }


        public async Task<ApiResponse<U>> PostAsync<T, U>(string url, T data)
        {
            SetAuthorizationHeader();
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var apiResponse = new ApiResponse<U>();
            try
            {

                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    //If the return type is not object it goes to catch
                    if (typeof(U) != typeof(string))
                    {
                        apiResponse.Data = JsonConvert.DeserializeObject<U>(jsonResponse);
                    }
                    apiResponse.Success = true;
                }
                else
                {
                    apiResponse.Success = false;
                    apiResponse.ErrorMessage = GetErrorMessage(response.StatusCode);
                    apiResponse.StatusCode = response.StatusCode;
                }
            }
            catch
            {
                apiResponse.Success = false;
                apiResponse.ErrorMessage = "API service is unreachable.";
                apiResponse.StatusCode = HttpStatusCode.InternalServerError;
            }
            return apiResponse;

        }


        public async Task<ApiResponse<U>> DeleteAsync<T, U>(string url, int id)
        {
            SetAuthorizationHeader();
            var apiResponse = new ApiResponse<U>();
            var response = await _httpClient.DeleteAsync($"{url}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (typeof(U) != typeof(string))
                {
                    apiResponse.Data = JsonConvert.DeserializeObject<U>(jsonResponse);
                }
                apiResponse.Success = true;
            }
            else
            {

                apiResponse.Success = false;
                apiResponse.ErrorMessage = GetErrorMessage(response.StatusCode);
                apiResponse.StatusCode = response.StatusCode;
            }
            return apiResponse;
        }
        private string GetErrorMessage(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.NotFound => "Data not found.",
                HttpStatusCode.Unauthorized => "Unauthorized access.",
                HttpStatusCode.BadRequest => "Invalid request.",
                HttpStatusCode.Conflict => "Conflict occurred.",
                HttpStatusCode.InternalServerError => "Server error.",
                _ => "An unexpected error occurred."
            };
        }
    }

    public class ApiResponse<U>
    {
        public bool Success { get; set; }
        public U? Data { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
