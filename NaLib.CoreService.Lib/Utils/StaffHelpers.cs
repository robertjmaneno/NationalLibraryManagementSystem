using Microsoft.IdentityModel.Tokens;
using NaLib.CoreService.Lib.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NaLib.CoreService.Lib.Utils
{
    public class StaffHelpers
    {
        /// <summary>
        /// Hashes a password using SHA-256 with a randomly generated salt.
        /// </summary>
        /// <param name="password">The plaintext password to hash.</param>
        /// <returns>The hashed password as a Base64 string, including the salt.</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be null or whitespace.", nameof(password));
            }

            var saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var passwordWithSaltBytes = new byte[passwordBytes.Length + saltBytes.Length];
            Buffer.BlockCopy(passwordBytes, 0, passwordWithSaltBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, passwordWithSaltBytes, passwordBytes.Length, saltBytes.Length);

            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(passwordWithSaltBytes);
                var hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];
                Buffer.BlockCopy(hashBytes, 0, hashWithSaltBytes, 0, hashBytes.Length);
                Buffer.BlockCopy(saltBytes, 0, hashWithSaltBytes, hashBytes.Length, saltBytes.Length);
                return Convert.ToBase64String(hashWithSaltBytes);
            }
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            var hashWithSaltBytes = Convert.FromBase64String(storedHash);
            var saltBytes = new byte[16];
            Buffer.BlockCopy(hashWithSaltBytes, hashWithSaltBytes.Length - saltBytes.Length, saltBytes, 0, saltBytes.Length);

            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var passwordWithSaltBytes = new byte[passwordBytes.Length + saltBytes.Length];
            Buffer.BlockCopy(passwordBytes, 0, passwordWithSaltBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, passwordWithSaltBytes, passwordBytes.Length, saltBytes.Length);

            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(passwordWithSaltBytes);
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    if (hashBytes[i] != hashWithSaltBytes[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static class JwtTokenGenerator
        {
            private static readonly string _secretKey = "Ngdheyejy37829!!gdghd939xghxhhnNalibdghreshjsjkgdftfstbgshsgsrjdj64";
            private static readonly string _issuer = "NaLib";
            private static readonly string _audience = "NaLibUsers"; 

            public static string GenerateToken(User user)
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.RoleId.ToString()), 
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("LibraryId", user.LibraryId.ToString()),
            };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiration = DateTime.UtcNow.AddHours(1);
                var token = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _audience,
                    claims: claims,
                    expires: expiration,
                    signingCredentials: credentials
                );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }

    }
}
