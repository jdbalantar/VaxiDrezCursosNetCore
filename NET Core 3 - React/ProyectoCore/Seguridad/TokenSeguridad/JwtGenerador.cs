using Aplicacion.Contratos;
using Dominio;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Seguridad.TokenSeguridad
{
    /// <summary>
    /// Clase que genera el Token
    /// </summary>
    public class JwtGenerador : IJwtGenerador
    {
        /// <summary>
        /// Método que genera el Token
        /// </summary>
        /// <param name="usuario">Parámetro que recibe la información del usuario que inicia sesión a través del controller</param>
        /// <returns>Un String que contiene el Token</returns>
        /// <exception cref="NotImplementedException"></exception>
        public string CrearToken(Usuario usuario)
        {
            // Creamos una Lista de Claims
            // Los Claims es la data el usuario que se desea compartir con el cliente
            var claims = new List<Claim>
            {
                // Este Claim representa el nombre del usuario
                // JwtRegisteredClaimNames accedemos a los atributos del usuario, 
                // el segundo parámetro es la data del usuario que representa el atributo de la izquierda
                // en este caso NameId == UserName
                new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName)
            };

            // Crearemos las credenciales de acceso

            // SymmetricSecurityKey recibe un parámetro, la cual es la llave secreta de tu servidor.
            // Con esta llave pueden descifrar los tokens de tus usuarios
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi Palabra Secreta"));

            // SigningCredentials recibev dos parámetros. La llave y el algoritmo de encriptacion usado para el token
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // Creamos la infor´mación del Token
            var tokenDescripcion = new SecurityTokenDescriptor
            {
                // Le pasamos los Claims (información que devolverá del usuario)
                Subject = new ClaimsIdentity(claims),
                // Tiempo de expiración del Token (Tiempo por el que el usuario estará en sesión)
                // Le voy a decir que esté vivo para 30 dias
                Expires = DateTime.Now.AddDays(30),
                // Le indicamos el tipo de acceso para las credenciales
                SigningCredentials = credenciales
            };

            // Con esta variable manejamos la información del Token
            var tokenManejador = new JwtSecurityTokenHandler();

            // Por último, creamos el token usando la variable tokenManejador
            var token = tokenManejador.CreateToken(tokenDescripcion);
            // Y usando tokenManejador, escribimos el token y lo devolvemos
            return tokenManejador.WriteToken(token);
        }
    }
}
