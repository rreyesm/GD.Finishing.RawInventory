using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stock_Finishing.RepositoryAPI
{
    public interface IRepositoryApi
    {
        Task<HttpResponseResult<T>> Get<T>(string url);
        Task<HttpResponseResult<object>> Put<T>(string url, T send);
        Task<HttpResponseResult<TResponse>> Put<TResponse, TRequest>(string url, TRequest send);
        Task<HttpResponseResult<object>> Post<T>(string url, T send);
        Task<HttpResponseResult<TResponse>> Post<TResponse, TRequest>(string url, TRequest send);
        Task<HttpResponseResult<TResponse>> Delete<TResponse>(string url);
    }
    public class RepositoryApi : IRepositoryApi
    {
        private readonly HttpClient httpClient;

        public RepositoryApi(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpClient.Timeout = TimeSpan.FromMinutes(30);
        }

        public JsonSerializerOptions OptionByDefaultJSON => new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };


        public async Task<HttpResponseResult<TResponse>> Delete<TResponse>(string url)
        {
            var responseHttp = await httpClient.DeleteAsync(url);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await DeserializeResponse<TResponse>(responseHttp, OptionByDefaultJSON);
                return new HttpResponseResult<TResponse>(response, false, responseHttp);
            }
            else
            {
                return new HttpResponseResult<TResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);
            }
        }


        public async Task<HttpResponseResult<T>> Get<T>(string url)
        {
            var responseHttp = await httpClient.GetAsync(url);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await DeserializeResponse<T>(responseHttp, OptionByDefaultJSON);
                return new HttpResponseResult<T>(response, false, responseHttp);
            }
            else
                return new HttpResponseResult<T>(default, true, responseHttp);
        }

        public async Task<HttpResponseResult<object>> Post<T>(string url, T send)
        {
            var sendJson = JsonSerializer.Serialize(send);
            var sendContent = new StringContent(sendJson, Encoding.UTF8, "application/json");
            var responseHttp = await httpClient.PostAsync(url, sendContent);
            return new HttpResponseResult<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }
        public async Task<HttpResponseResult<TResponse>> Post<TResponse, TRequest>(string url, TRequest send)
        {
            var sendJson = JsonSerializer.Serialize(send);
            var sendContent = new StringContent(sendJson, Encoding.UTF8, "application/json");
            var responseHttp = await httpClient.PostAsync(url, sendContent);

            // Leer el contenido de la respuesta
            var responseContent = await responseHttp.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Content: {responseContent}");

            if (responseHttp.IsSuccessStatusCode)
            {
                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    try
                    {
                        var response = JsonSerializer.Deserialize<TResponse>(responseContent, OptionByDefaultJSON);
                        return new HttpResponseResult<TResponse>(response, false, responseHttp);
                    }
                    catch (JsonException ex)
                    {
                        // Manejo de error de deserialización
                        var result = new HttpResponseResult<TResponse>(default, true, responseHttp);
                        result.HttpResponseMessage.Content = new StringContent($"Error al deserializar la respuesta: {ex.Message}");
                        return result;
                    }
                }
                else
                {
                    // Respuesta vacía
                    var result = new HttpResponseResult<TResponse>(default, false, responseHttp);
                    result.HttpResponseMessage.Content = new StringContent("200 OK. La respuesta de la API está vacía.");
                    return result;
                }
            }
            else
            {
                var result = new HttpResponseResult<TResponse>(default, true, responseHttp);
                result.HttpResponseMessage.Content = new StringContent($"Error en la solicitud: {responseHttp.ReasonPhrase}");
                return result;
            }
        }

        public async Task<HttpResponseResult<object>> Put<T>(string url, T send)
        {
            var sendJson = JsonSerializer.Serialize(send);
            var sendContent = new StringContent(sendJson, Encoding.UTF8, "application/json");
            var responseHttp = await httpClient.PutAsync(url, sendContent);
            return new HttpResponseResult<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseResult<TResponse>> Put<TResponse, TRequest>(string url, TRequest send)
        {
            var sendJson = JsonSerializer.Serialize(send);
            var sendContent = new StringContent(sendJson, Encoding.UTF8, "application/json");
            var responseHttp = await httpClient.PutAsync(url, sendContent);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await DeserializeResponse<TResponse>(responseHttp, OptionByDefaultJSON);
                return new HttpResponseResult<TResponse>(response, false, responseHttp);
            }
            else
                return new HttpResponseResult<TResponse>(default, true, responseHttp);
        }
        private async Task<T> DeserializeResponse<T>(HttpResponseMessage httpResponseMessage, JsonSerializerOptions jsonSerializerOptions)
        {
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, jsonSerializerOptions);
        }
    }
}
