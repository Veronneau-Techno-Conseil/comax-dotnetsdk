using CommunAxiom.Commons.Shared.OIDC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunAxiom.DotnetSdk.Helpers.OIDC
{
    public abstract class AuthClient<TClient> where TClient : IDisposable
    {
        private IOptionsMonitor<OIDCSettings> _oidcSettings;
        private readonly IConfiguration _configuration;


        private DateTime _tokenExpires = DateTime.UtcNow;
        private TokenData _tokenData = null;
        private ITokenProvider? _impersonatedTokenProvider;

        public AuthClient(IConfiguration configuration, IOptionsMonitor<OIDCSettings> optionsMonitor)
        {
            _configuration = configuration;
            _oidcSettings = optionsMonitor;
        }

        public bool ShouldImpersonate { get; set; } = false;

        public void SetImpersonatedTokenProvider(ITokenProvider? impersonatedTokenProvider)
        {
            _impersonatedTokenProvider= impersonatedTokenProvider;
        }

        private async Task<string> FetchToken()
        {
            if (_tokenData == null || DateTime.UtcNow > _tokenExpires)
            {
                _tokenData = null;
                //Comax.Central.CentralApi centralApi = new Comax.Central.CentralApi();
                TokenClient tokenClient = new TokenClient(_oidcSettings.CurrentValue);
                bool success = false;
                TokenData res = null;
                if(ShouldImpersonate)
                {
                    if (_impersonatedTokenProvider == null)
                        throw new InvalidOperationException("Impersonated token provider is null");
                    var t = await _impersonatedTokenProvider.FetchToken();
                    (success, res) = await tokenClient.Impersonate(t);

                }
                else
                {
                    var settings = _oidcSettings.CurrentValue;
                    (success, res) = await tokenClient.AuthenticateClient(settings.Scopes, settings.ClientId, settings.Secret);
                }

                if (success)
                {
                    _tokenData = res;
                    _tokenExpires = DateTime.UtcNow.AddSeconds(res.expires_in - 300);
                }
            }
            return _tokenData?.access_token;
        }

        protected abstract TClient CreateClient(HttpClient httpClient);

        private async Task<HttpClient> CreateClient()
        {
            HttpClient client = new HttpClient();
            var t = await FetchToken();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", t);
            return client;
        }

        public async Task WithClient(Func<TClient, Task> func)
        {
            using var cl = await CreateClient();
            using var tcl = CreateClient(cl);

            await func(tcl);
        }

        public async Task<TRes> WithClient<TRes>(Func<TClient, Task<TRes>> func)
        {
            using var cl = await CreateClient();
            using var tcl = CreateClient(cl);

            return await func(tcl);
        }
    }


}
