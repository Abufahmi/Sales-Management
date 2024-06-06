using Sales.Client.Repositories;
using Sales.Library.Models;

namespace Sales.Client.Services
{
    public class CustomHttpHandler : DelegatingHandler
    {
        private readonly LocalStorageService storageService;
        private readonly IAccountRepository accountRepository;

        public CustomHttpHandler(LocalStorageService storageService, IAccountRepository accountRepository)
        {
            this.storageService = storageService;
            this.accountRepository = accountRepository;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
           CancellationToken cancellationToken)
        {
            var result = await base.SendAsync(request, cancellationToken);
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var stringToken = await storageService.GetToken();
                if (stringToken == null) return result;

                var token = request.Headers?.Authorization?.Parameter;
                var tokenModel = Serialization
                    .DeSerializeJsonString<TokenModel>(stringToken);
                if (tokenModel == null || tokenModel.Token == null || tokenModel.RefreshToken == null)
                    return result;

                var newToken = await GetRefreshTokenAsync(tokenModel.RefreshToken);
                if (newToken == null) return result;
                if (request.Headers?.Authorization != null)
                {
                    request.Headers.Authorization = new System.Net.Http.Headers
                        .AuthenticationHeaderValue("Bearer", newToken);
                    return await base.SendAsync(request, cancellationToken);
                }
            }

            return result;
        }

        private async Task<string?> GetRefreshTokenAsync(string? refreshToken)
        {
            if (refreshToken == null) return null;
            var refreshModel = new RefreshTokenModel { RefreshToken = refreshToken };
            TokenModel? tokenModel = await accountRepository.GetRefreshTokenAsync(refreshModel);
            if (tokenModel == null) return null;

            var serializeToken = Serialization.SerializeObj(tokenModel);
            if (serializeToken == null) return null;

            await storageService.SetToken(serializeToken);
            return tokenModel.Token;
        }
    }
}
