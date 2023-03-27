using Entities.DataTransferObjects;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CompanyEmployees.Client.Services;

public class HttpClientPatchService : IHttpClientServiceImplementation
{
    private static readonly HttpClient _httpClient = new();

    public HttpClientPatchService()
    {
        _httpClient.BaseAddress = new Uri("http://localhost:5000/api/");
        _httpClient.Timeout = new TimeSpan(0, 0, 30);
        _httpClient.DefaultRequestHeaders.Clear();
    }

    public async Task ExecuteAsync()
	{
		await PatchEmployeeAsync();
        await PatchEmployeeWithHttpRequestMessageAsync();
    }

    private static async Task PatchEmployeeAsync()
    {
        var patchDoc = new JsonPatchDocument<EmployeeForUpdateDto>();
        patchDoc.Replace(e => e.Name, "Sam Raiden Updated");
        patchDoc.Remove(e => e.Age);

        /*
         * https://code-maze.com/using-httpclient-to-send-http-patch-requests-in-asp-net-core/
         * 
         * The important thing to notice here is that we don’t use JsonSerializer.Serialize() method from 
         * the System.Text.Json library but we use JsonConvert.SerializeObject() method from the 
         * Newtonsoft.Json library. 
         * We have to do this, otherwise, we get 400 bad request from our API since the patch document 
         * isn’t serialized well with System.Text.Json.
         */
        string serializedDoc = JsonConvert.SerializeObject(patchDoc);
        var requestContent = new StringContent(serializedDoc, Encoding.UTF8, "application/json-patch+json");

        string uri = Path.Combine("companies", "C9D4C053-49B6-410C-BC78-2D54A9991870", "employees", "80ABBCA8-664D-4B20-B5DE-024705497D4A");
        HttpResponseMessage response = await _httpClient.PatchAsync(uri, requestContent);

        response.EnsureSuccessStatusCode();
    }

    private static async Task PatchEmployeeWithHttpRequestMessageAsync()
    {
        var patchDoc = new JsonPatchDocument<EmployeeForUpdateDto>();
        patchDoc.Replace(e => e.Name, "Sam Raiden");
        patchDoc.Add(e => e.Age, 28);

        string uri = Path.Combine("companies", "C9D4C053-49B6-410C-BC78-2D54A9991870", "employees", "80ABBCA8-664D-4B20-B5DE-024705497D4A");
        string serializedDoc = JsonConvert.SerializeObject(patchDoc);

        var request = new HttpRequestMessage(HttpMethod.Patch, uri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Content = new StringContent(serializedDoc, Encoding.UTF8);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}
