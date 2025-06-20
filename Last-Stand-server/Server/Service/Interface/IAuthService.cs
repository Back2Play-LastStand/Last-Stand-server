﻿namespace Server.Service.Interface;

public interface IAuthService
{
    Task<bool> RegisterAsync(string playerId, string password, string email);
    Task<(bool IsSuccess, bool IsNewAccount)> LoginAsync(string playerId, string password);
}