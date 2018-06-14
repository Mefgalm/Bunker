using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using Bunker.Business.Entities;
using Bunker.Business.Entities.Dictioneries;
using Bunker.Business.Extensions;
using Bunker.Business.Interfaces.Infrastructure;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;
using Bunker.Business.Interfaces.Responses;
using Bunker.Business.Interfaces.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace Bunker.Business.Services
{
    public class AuthService : BaseService, IAuthService
    {
        public AuthService(BunkerDbContext dbContext, IErrorMessageProvider errorMessageProvider) 
            : base(dbContext, errorMessageProvider)
        {
        }

        public BaseResponse<LoginResponse> Login(string email, string password)
        {
            var player = _dbContext.Players
                                   .Include(x => x.Roles)
                                   .ThenInclude(x => x.Role)
                                   .FirstOrDefault(x => x.Email == email);

            if (player == null
             || !HashPassword(player.PasswordSalt, password).SequenceEqual(player.PasswordHash))
                return BaseResponse<LoginResponse>.Fail(_errorMessageProvider.EmailOrPasswordIsIncorrect);

            return BaseResponse<LoginResponse>.Success(new LoginResponse
            {
                Roles    = player.Roles.Select(x => x.Role.Name),
                PlayerId = player.Id.ToString()
            });
        }

        public BaseResponse<object> Register(RegisterRequest request)
        {
            var validationResult = Validate<object>(request);

            if (!validationResult.Ok)
                return validationResult;

            byte[] playerSalt = GenerateSalt();

            var player = new Player
            {
                Email        = request.Email,
                FirstName    = request.FirstName,
                LastName     = request.LastName,
                NickName     = request.NickName,
                PasswordSalt = playerSalt,
                PasswordHash = HashPassword(playerSalt, request.Password)
            };

            var playerRole = new PlayerRole
            {
                Player = player,
                RoleId = RoleDictionary.Init.Identifier()
            };
            
            _dbContext.Players.Add(player);
            _dbContext.PlayerRoles.Add(playerRole);

            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        private static byte[] HashPassword(byte[] salt, string password) =>
            KeyDerivation.Pbkdf2(password: password,
                                 salt: salt,
                                 prf: KeyDerivationPrf.HMACSHA1,
                                 iterationCount: 10000,
                                 numBytesRequested: 256 / 8);
    }
}