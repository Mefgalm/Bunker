using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using Bunker.Business.Entities;
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
        public AuthService(BunkerDbContext dbContext) : base(dbContext)
        {
        }

        public BaseResponse<LoginResponse> Login(string email, string password)
        {
            const string emailOrPasswordIsIncorrect = "Email or password is wrong";

            var player = _dbContext.Players
                                   .Include(x => x.Roles)
                                   .ThenInclude(x => x.Role)
                                   .FirstOrDefault(x => x.Email == email);

            if (player == null
             || !HashPassword(player.PasswordSalt, password).SequenceEqual(player.PasswordHash))
                return BaseResponse<LoginResponse>.Fail(emailOrPasswordIsIncorrect);

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

            _dbContext.Players.Add(new Player
            {
                Email        = request.Email,
                FirstName    = request.FirstName,
                LastName     = request.LastName,
                NickName     = request.NickName,
                PasswordSalt = playerSalt,
                PasswordHash = HashPassword(playerSalt, request.Password),
            });

            _dbContext.SaveChanges();

            return BaseResponse<object>.Success();
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        private byte[] HashPassword(byte[] salt, string password) =>
            KeyDerivation.Pbkdf2(password: password,
                                 salt: salt,
                                 prf: KeyDerivationPrf.HMACSHA1,
                                 iterationCount: 10000,
                                 numBytesRequested: 256 / 8);
    }
}