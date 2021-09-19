using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OChat.Core.Common.Repositories;
using OChat.Domain;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using OChat.Core.Services.Exceptions;

namespace OChat.Core.Services
{
    public record AuthenticationResult(String Token, Boolean IsAuthenticated);

    public class AuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthenticationService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task RegisterUser(String username, String email, String password)
        {
            var userInDb = await _userRepository.GetByUsernameAsync(username);

            if (userInDb is not null)
                throw new UsernameAlreadyExistsException("User with username already exists.");

            var user = new User
            {
                Username = username,
                Email = email
            };

            String passwordHash = _passwordHasher.HashPassword(user, password);

            user.PasswordHash = passwordHash;

            await _userRepository.SaveEntityAsync(user);
        }

        public async Task<AuthenticationResult> Authenticate(String username, String password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
                return new AuthenticationResult(Token: null, IsAuthenticated: false);

            var token = CreateJSONWebToken(user);

            return new AuthenticationResult(Token: token, IsAuthenticated: true);
        }
        
        private string CreateJSONWebToken(User user)
        {
            const String key = "BIG_SECRET_KEY_THAT_NOONE_SHOULD_KNOW_1238977394872123!@#$%";

            var tokenHandler = new JwtSecurityTokenHandler();
            var convertedKey = Encoding.ASCII.GetBytes(key);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email)

                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(convertedKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
