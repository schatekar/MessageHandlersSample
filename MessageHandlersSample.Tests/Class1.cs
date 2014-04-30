using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MessageHandlersSample.MessageHandlers;
using NUnit.Framework;
using System.Net.Http;

namespace MessageHandlersSample.Tests
{
    [TestFixture]
    public class LanguageHandlerTests
    {
        internal class DummyHandler : DelegatingHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return new TaskFactory<HttpResponseMessage>().StartNew(() => new HttpResponseMessage(HttpStatusCode.OK), cancellationToken);
            }
        }

        internal class LanguageHandlerInternal : LanguageHandler
        {
            public Task<HttpResponseMessage> SendAyncInternal(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                return base.SendAsync(request, cancellationToken);
            }
        }

        [Test]
        public void SetsThreadCulture()
        {
            var request = new HttpRequestMessage();
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-GB"));

            var languageHandler = new LanguageHandlerInternal
            {
                InnerHandler = new DummyHandler()
            };

            var response = languageHandler.SendAyncInternal(request, new CancellationToken());

            Assert.That(Thread.CurrentThread.CurrentCulture.Name, Is.EqualTo("en-GB"));
        }
    }
}
