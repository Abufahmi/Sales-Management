﻿using Sales.Context.Models;
using static Sales.Context.Helpers.Responses;

namespace Sales.Context.Services
{
    public interface IAccountService
    {
        Task<LoginResponse> LoginAsync(Login login);
        Task<DefaultResponse> RegisterAsync(Register register);
    }
}