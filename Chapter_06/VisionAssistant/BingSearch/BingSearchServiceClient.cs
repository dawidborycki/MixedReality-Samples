#region Using

using MixedReality.Common.ArgumentValidation;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VisionAssistant.BingSearch.Models;

#endregion

namespace VisionAssistant.BingSearch
{
    public class BingSearchServiceClient
    {
        #region Fields

        private HttpClient httpClient;

        #endregion

        #region Methods (Public)

        public BingSearchServiceClient(string apiKey, string endPoint)
        {
            Check.IsNull(apiKey, "apiKey");
            Check.IsNull(endPoint, "endPoint");

            httpClient = new HttpClient
            {
                BaseAddress = new Uri(endPoint)
            };

            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
        }

        public async Task<SearchResult> Search(string query)
        {
            var queryString = WebUtility.UrlEncode(query);

            var getUrl = $"search?q={queryString}&mkt=en-us&count=10";

            var response = await httpClient.GetAsync(getUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Unexpected status code");
            }

            return await response.Content.ReadAsAsync<SearchResult>();
        }

        #endregion
    }
}
