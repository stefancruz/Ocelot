namespace Ocelot.Cache
{
    using Ocelot.Request.Middleware;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CacheKeyGenerator : ICacheKeyGenerator
    {
        public string GenerateRequestCacheKey(DownstreamRequest downstreamRequest)
        {
            string hashedContent = null;
            string contentLanguage = "";

            if (downstreamRequest.Headers != null && downstreamRequest.Headers.TryGetValues("Content-Language", out IEnumerable<string> values))
            {
                contentLanguage = values.FirstOrDefault();
            }

            StringBuilder downStreamUrlKeyBuilder = new StringBuilder($"{downstreamRequest.Method}-{downstreamRequest.OriginalString}{contentLanguage}");

            if (downstreamRequest.Content != null)
            {
                string requestContentString = Task.Run(async () => await downstreamRequest.Content.ReadAsStringAsync()).Result;
                downStreamUrlKeyBuilder.Append(requestContentString);
            }

            hashedContent = MD5Helper.GenerateMd5(downStreamUrlKeyBuilder.ToString());
            return hashedContent;
        }
    }
}
