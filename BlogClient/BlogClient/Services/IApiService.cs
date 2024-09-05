namespace BlogClient.Services
{
    public interface IApiService
    {
        Task<ApiResponse<T>> GetAsync<T>(string url);
        Task<ApiResponse<U>> PostAsync<T, U>(string url, T data);
        Task<ApiResponse<U>> DeleteAsync<T, U>(string url, int id);
    }
}
