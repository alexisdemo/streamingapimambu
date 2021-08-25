using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Headers;
using System.Net;
using System.Collections.Concurrent;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MambuStreamingReader.entities;
using MambuDbServer;
using MambuDbServer.entities;
using System;
using System.IO;

namespace MambuStreamingReader
{
    public class MambuClientService
    {

        //static ConcurrentQueue<Entity> concurrentQueue = new ConcurrentQueue<Entity>();
        static HttpClient httpDataClient = null;
        //static int requestLimit = 8;
        //static SemaphoreSlim throttler = new SemaphoreSlim(requestLimit);
        //Mambu configuration values
        private string subscriptionId = string.Empty;
        private string mambuBaseUrl = "https://mbuasalinas.sandbox.mambu.com";

        //Streaming API consumers Key        
        private string apiKey = "Ip2GJsEADoY1VBczEZUvVtHwGHeDnvcQ";
        private string topic = string.Empty;
        private int timeToExecute = 0;


        static bool donwloadData = true;
        static CancellationTokenSource downloadCancellationTokenSource = new CancellationTokenSource();

        public MambuClientService()
        {
            httpDataClient = new HttpClient();
        }

        public MambuClientService(string _apiKey, string _mambuBaseUrl, string _topic, int _time) : this()
        {
            apiKey = _apiKey;
            mambuBaseUrl = _mambuBaseUrl;
            topic = _topic;
            timeToExecute = _time;
        }

        async Task<string> GetSubcriptions()
        {

            var request = "{\"owning_application\": \"demo-app-4\", \"event_types\": [\"" + topic +   "\"]}";

            

            var data = new StringContent(request, Encoding.UTF8, "application/json");

            httpDataClient.DefaultRequestHeaders.Add("apikey", apiKey);
            httpDataClient.DefaultRequestHeaders.Add("Accept", "application/vnd.mambu.v2+json");
            //httpDataClient.DefaultRequestHeaders.Add("User-Agent", "Mambu Streaming API Sample");
            httpDataClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var cts = new CancellationToken();
            //string suscriptionId = "";
            try
            {
                var clientEndpoint = mambuBaseUrl + "/api/v1/subscriptions";
                //var clientResponse = await httpDataClient.PostAsync(clientEndpoint, data, cts);

                var clientResponse = await httpDataClient.PostAsync(clientEndpoint, data, cts);
                if (clientResponse.StatusCode == HttpStatusCode.OK || clientResponse.StatusCode == HttpStatusCode.Created)
                {
                    string result = clientResponse.Content.ReadAsStringAsync().Result;
                    //var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

                    JObject o = JObject.Parse(result);

                    subscriptionId = (string)o["id"];
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return subscriptionId;
        }

        async Task StreamRead(string baseUrl, string subcriptionId, string apiKey)
        {
            using (HttpClient httpClient = new HttpClient())
            {

                httpClient.Timeout = TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite);
                var requestUri = baseUrl + subcriptionId + "/events?batch_flush_timeout=1&batch_limit=10";

                var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
                request.Headers.Add("apiKey", apiKey);
                var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                var responseHeaders = response.Headers;

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    downloadCancellationTokenSource.Cancel();
                    WaitHandle[] handles = { downloadCancellationTokenSource.Token.WaitHandle };
                    WaitHandle.WaitAny(handles);
                    throw new Exception("Not found events");
                }

                var mambuStreamingId = response.Headers.GetValues("x-mambu-streamid").FirstOrDefault();

                var stream = await response.Content.ReadAsStreamAsync();
                bool flag = true;
                DateTime startTime = DateTime.Now;
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream && flag)
                    {

                        //We are ready to read the stream
                        var currentLine = reader.ReadLine();
                        try
                        {
                            StreamData currentEvent = JsonConvert.DeserializeObject<StreamData>(currentLine);

                            if (currentEvent.events != null && currentEvent.events.Count > 0)
                            {
                                Task.Run(() => CommitCursor(baseUrl, subcriptionId, apiKey, mambuStreamingId, currentEvent));
                            }
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }

                        if (startTime.AddSeconds(timeToExecute) < DateTime.Now)
                        {
                            flag = false;
                        }
                    }
                }
            }
        }

        async Task CommitCursor(string baseUrl, string subcriptionId, string apiKey, string eventStreamingId, StreamData currentEvent)
        {
            Cursor cursor = currentEvent.cursor;
            using (HttpClient httpClient = new HttpClient())
            {
                var url = baseUrl + subcriptionId + "/cursors";
                httpClient.DefaultRequestHeaders.Add("apiKey", apiKey);
                httpClient.DefaultRequestHeaders.Add("X-Mambu-Streamid", eventStreamingId);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var cursors = new List<Cursor>();
                cursors.Add(cursor);

                string jsonString = JsonConvert.SerializeObject(new { items = cursors });

                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    foreach (var mEvent in currentEvent.events)
                    {
                        //TODO : mostrar mensaje de ok
                        MySqlConnForMambu my = new MySqlConnForMambu();
                        var result = (object)mEvent.body;

                        my.InsertDataBaseMambu(result.ToString());
                    }
                }
                else
                {
                    throw new Exception("No se pudo procesar mensaje");
                }

            }
        }

        public async Task Ejecutar()
        {
            try
            {
                string susId = await GetSubcriptions();
                await StreamRead(mambuBaseUrl + "/api/v1/subscriptions/", susId, apiKey);
                MostrarProcesadosEnBaseDatos();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MostrarProcesadosEnBaseDatos()
        {
            MySqlConnForMambu my = new MySqlConnForMambu();

            List<Eventos> lista = my.ReadDataBaseMambu();

            foreach (var item in lista)
            {
                //string auxText = txvResult.Buffer.Text;
                //txvResult.Buffer.Text = string.Join("\n", auxText, item.Mensaje);
            }


        }

    }
}
