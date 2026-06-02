using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API_NET.Models;
using Microsoft.IdentityModel.Tokens;

namespace API_NET.Utilities
{
    public class Utils
    {
        private readonly IConfiguration _configuration;
        public Utils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string encryptSHA256(string plaintext)
        {
            // COMPUTAR EL HASH
            SHA256 Sha256Hash = SHA256.Create();
            byte[] bytes = Sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plaintext));

            // FORMA DEL VIDEO - CONVERTIR  DE BYTES A STRING
            StringBuilder sb = new StringBuilder();
            //for (int i = 0;i< bytes.Length; i++) {
            //    sb.Append(bytes[i].ToString("x2"));
            //}
            //return sb.ToString();

            // AUTOCOMPLETADO
            return Convert.ToBase64String(bytes);
        }

        public string generateJWT(User user)
        {
            // Claims: Datos que se incluyen en el token.
            var claims = new[]
            {
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) UN ID PARA EL TOKEN NO ES OBLIGATORIO
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                // Añadir otro datos: role etc...
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            /* CREAR LA CLAVE DE SEGURIDAD */
            // SymmetricSecurityKey: FIRMA EL TOKEN, CLAVE DEBE TENER MIN 32 CARACTERES
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));

            /* DEFINIR LAS CREDENCIALES DE FIRMA */
            // SigningCredentials: ESPECIFICA LA CLAVE SEGURIDAD Y EL ALGORITMO
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            /* CONFIGURAR TOKEN */
            var token = new JwtSecurityToken(
                //issuer: "API_NET", // EMISOR DEL TOKEN
                //audience: null, // A QUIEN ESTA DESTINADO EL TOKEN
                claims: claims, // INFORMACION DEL USUARIO
                expires: DateTime.UtcNow.AddHours(1),// FECHA DE EXPIRACION
                signingCredentials: credentials // CREDENCIALES DE FIRMA
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
