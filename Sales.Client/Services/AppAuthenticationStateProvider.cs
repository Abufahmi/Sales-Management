using Microsoft.AspNetCore.Components.Authorization;
using Sales.Client.Models;
using Sales.Library.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Sales.Client.Services
{
    public class AppAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly LocalStorageService localStorageService;
        private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public AppAuthenticationStateProvider(LocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var stringToken = await localStorageService.GetToken();
            if (stringToken == null || string.IsNullOrEmpty(stringToken))
                return new AuthenticationState(_anonymous);

            var deserializeToken = Serialization.DeSerializeJsonString<TokenModel>(stringToken);
            if (deserializeToken == null)
                return new AuthenticationState(_anonymous);

            Session? session = GetSession(deserializeToken.Token);
            if (session == null)
                return new AuthenticationState(_anonymous);

            ClaimsPrincipal claimsPrincipal = GetClaimsPrincipal(session);
            return new AuthenticationState(claimsPrincipal);
        }

        private ClaimsPrincipal GetClaimsPrincipal(Session session)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, session.UserId!),
                new (ClaimTypes.Name, session.UserName!),
                new (ClaimTypes.Email, session.Email!),
                new (ClaimTypes.Role, session.RoleName!),
            }, "JwtAuth"));
        }

        private Session? GetSession(string? token)
        {
            if (token == null || string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var deserializeToken = handler.ReadJwtToken(token);
            var userId = deserializeToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var userName = deserializeToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            var email = deserializeToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            var role = deserializeToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

            if (userId == null || userName == null || email == null || role == null)
                return null;

            return new Session
            {
                UserId = userId.Value,
                UserName = userName.Value,
                Email = email.Value,
                RoleName = role.Value,
            };
        }

        public async Task<bool> CreateAuthenticationStateAsync(TokenModel tokenModel)
        {
            if (tokenModel == null || tokenModel.Token == null)
            {
                await DeleteAuthenticationStateAsync();
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
                return false;
            }

            var serializeToken = Serialization.SerializeObj(tokenModel);
            await localStorageService.SetToken(serializeToken);
            Session? session = GetSession(tokenModel.Token);
            if (session == null)
                return false;

            var claimsPrincipal = GetClaimsPrincipal(session);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
            return true;
        }

        public async Task DeleteAuthenticationStateAsync()
        {
            await localStorageService.RemoveToken();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}
