﻿using CadastroUsuarioAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace CadastroUsuarioAPI.Utils
{
    public class TokenUtils
    {
        private readonly ConfigurationManager _configuration;

        public const string ClaimUserId = "UserId";

        public TokenUtils(ConfigurationManager configuration)
        {
            _configuration= configuration;
        }

        public string Gerar(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Token:SecretKey"]);
            var tokeDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, usuario.Username),
                        new Claim(ClaimTypes.Role, usuario.Role),
                        new Claim(ClaimUserId, usuario.Id.ToString()),
                    }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokeDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
