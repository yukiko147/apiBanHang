using BackendWebBanLinhKien.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackendWebBanLinhKien.Data
{
    public class Token
    {
        public string SecretKey { get; set; }

        public string TaoToken(Nguoidung nguoidung)
        {
            var tokenhander = new JwtSecurityTokenHandler();
            var secretByte=Encoding.UTF8.GetBytes(this.SecretKey);
            var tokendes = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id",nguoidung.id),
                    new Claim("Email",nguoidung.email),
                    new Claim("Matkhau",nguoidung.matkhau),
                    new Claim("Hoten",nguoidung.hoten),
                    new Claim("Quyen",nguoidung.quyen.ToString()),
                    new Claim("Trangthai",nguoidung.trangthai.ToString()),
                    new Claim("Them",nguoidung.them.ToString()),
                    new Claim("Sua",nguoidung.sua.ToString()),
                    new Claim("Xoa",nguoidung.xoa.ToString()),
                    new Claim("NgayLap",nguoidung.ngaylap.ToString()),
                    new Claim("NgaySua",nguoidung.ngaysua.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretByte), SecurityAlgorithms.HmacSha512Signature)
            }; 
            var token=tokenhander.CreateToken(tokendes);
            return tokenhander.WriteToken(token);
        }

        public ClaimsPrincipal GiaiToken(string token)
        {
            ClaimsPrincipal claims = null;
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(this.SecretKey))
            };

            try
            {
                claims = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
                return claims;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
        }
    }
}
