using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ocelot.Requester
{
    /// <summary>
    /// This class was made to make unit testing easier when HttpClient is used.
    /// </summary>
    public class HttpClientWrapper : IHttpClient
    {
        public HttpClient Client { get; }
        
        public bool ConnectionClose { get; }
        public DelegatingHandler ClientMainHandler { get; }

        public HttpClientWrapper(HttpClient client, DelegatingHandler clientMainHandler, bool connectionClose = false)
        {
            Client = client;
            ClientMainHandler = clientMainHandler;
            ConnectionClose = connectionClose;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            request.Headers.ConnectionClose = ConnectionClose;
            return Client.SendAsync(request, cancellationToken);
        }
    }
}
