using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MoneyManager_DesktopApp.Models.ViewModels.Interfaces;
using Splat;

namespace MoneyManager_DesktopApp.Services;

public interface IHttpClientService
{
    Task<HttpResponseMessage> GetListOfItems(string uri);
    Task<HttpResponseMessage> GetRecordsForCategoryId(int categoryId, string uri);
    Task<HttpResponseMessage> DeleteItem(int id, string uri);
    Task<HttpResponseMessage> CreateItem<T>(T record, string uri);
    Task<HttpResponseMessage> UpdateItem<T>(T record, string uri) where T : IId;
}

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _http = new HttpClient();
    private readonly JwtTokenService _jwtToken = Locator.Current.GetService<JwtTokenService>();
    
    private readonly HttpRequestMessage _hrm = new HttpRequestMessage();

    private HttpRequestMessage Hrm(HttpMethod httpMethod, string uri)
    {
        var request = new HttpRequestMessage(httpMethod, uri);
        request.Headers.Add("X-Api-Key", ConfigurationManager.AppSettings["X-Api-Key"]);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken.Token);
        return request;
    }
    
    public async Task<HttpResponseMessage> GetItem()
    {
        var uri = @"https://moneymanager.hostingasp.pl/api/tracker";

            using var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var token = _jwtToken.Token;
            request.Headers.Add("X-Api-Key", ConfigurationManager.AppSettings["X-Api-Key"]);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await _http.SendAsync(request);

    }

    public async Task<HttpResponseMessage> GetListOfItems(string uri)
    {
        return await _http.SendAsync(Hrm(HttpMethod.Get, uri));
    }

    public async Task<HttpResponseMessage> GetRecordsForCategoryId(int categoryId, string uri)
    {
        throw new NotImplementedException();
    }

    public async Task<HttpResponseMessage> DeleteItem(int id, string uri)
    {
        throw new NotImplementedException();
    }

    public async Task<HttpResponseMessage> CreateItem<T>(T record, string uri)
    {
        throw new NotImplementedException();
    }

    public async Task<HttpResponseMessage> UpdateItem<T>(T record, string uri) where T : IId
    {
        throw new NotImplementedException();
    }
}