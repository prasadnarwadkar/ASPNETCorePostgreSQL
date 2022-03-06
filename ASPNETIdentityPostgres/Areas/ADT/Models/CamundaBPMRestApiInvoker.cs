using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ASPNETIdentityPostgres.Areas.ADT.Models
{
    public static class CamundaBPMRestApiInvoker
    {
        public static Uri CamundaRestBaseUri { get; internal set; }

        public static void SetCamundaRestBaseUri(Uri restUri)
        {
            CamundaRestBaseUri = restUri;
        }

        public static async Task<List<CamundaProcessInstVar>> getProcInstVars(string processInstID)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var url = new Uri(CamundaRestBaseUri, "variable-instance?processInstanceIdIn=" + processInstID).ToString();

            HttpResponseMessage result = new HttpResponseMessage();

            using (var handler = new HttpClientHandler() { })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                try
                {
                    result = await client.GetAsync("");

                    result.EnsureSuccessStatusCode();
                }
                catch (Exception)
                {
                    return JsonConvert.DeserializeObject<List<CamundaProcessInstVar>>("[]", settings);
                }

                var content = await result.Content.ReadAsStringAsync();

                var vars = JsonConvert.DeserializeObject<List<CamundaProcessInstVar>>(content, settings);

                return vars;
            }
        }
    }
}
