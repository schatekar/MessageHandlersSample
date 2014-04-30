using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MessageHandlersSample.MessageHandlers
{
    public class LanguageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var cultureCode = request.Headers.AcceptLanguage.First().Value;

            var culture = new CultureInfo(cultureCode);

            Thread.CurrentThread.CurrentCulture = culture;

            return base.SendAsync(request, cancellationToken);
        }
    }
}