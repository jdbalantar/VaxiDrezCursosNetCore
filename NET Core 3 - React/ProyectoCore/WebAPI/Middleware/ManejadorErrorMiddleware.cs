using System;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebAPI.Middleware
{
    /// <summary>
    /// Esta clase es ejecutada en el momento en el que las validaciones (FluentValidation) detectan un error.
    /// Entonces, para evitar mostrarle una excepción y código de error sin manejar, al cliente, se manejarán en esta clase,
    /// por ejemplo, si un parámetro está vacío y debe ser Required
    /// </summary>
    public class ManejadorErrorMiddleware
    {
        /// <summary>
        /// Parámetro que contiene la información de los Middleware
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Parámetro que se usa para añadir errores e información al Log
        /// </summary>
        private readonly ILogger<ManejadorErrorMiddleware> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next">Parámetro que contiene la información de los Middleware</param>
        /// <param name="logger">Parámetro que se usa para añadir errores e información al Log</param>
        public ManejadorErrorMiddleware(RequestDelegate next, ILogger<ManejadorErrorMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Esta clase se invoca cuando se dispara el ManejadorErrorMiddleware.
        /// Si todo está correcto, se ejecuta la sentencia "try" y pasa al siguiente middleware (es decir, se ejecuta la transacción con normalidad).
        /// El método next(context) se encarga de llevar todo el contexto al siguiente Middleware para que la transacción continúe con normalidad
        /// En cambio, si hay un error en la validación, se captura la excepción y se crea una nueva Excepción y se dispara el método
        /// ManejadorExcepcionAsincrono, el cuál implementa una lógica para saber qué tipo de excepción devolverle al usuario
        /// </summary>
        /// <param name="context">En el contexto van todos los parámetros, data y requerimientos que el usuario requiere de la API</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await ManejadorExcepcionAsincrono(context, e, _logger);
            }
        }

        /// <summary>
        /// Este método implementa la lógica para manejar el tipo de excepción que se va a disparar.
        /// Si la excepción es de tipo Http (API - Validaciones) Entonces, imprimirá con detalle el tipo de error,
        /// realiza una colección de tipo "errores" y devuelve el código del error (200, 300, 400, etc). ----\n
        /// Por otro lado, si el error es genérico, es decir, si hay un error en el código, no se le mostrará esta excepción
        /// al usuario, sino que se 
        /// </summary>
        /// <param name="context">En el contexto van todos los parámetros, data y requerimientos que el usuario requiere de la API</param>
        /// <param name="ex">El tipo de excepción que se captura</param>
        /// <param name="logger">Parámetro que se usa para añadir errores e información al Log</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task ManejadorExcepcionAsincrono(HttpContext context, Exception ex, ILogger<ManejadorErrorMiddleware> logger)
        {
            object errores = null;

            switch (ex)
            {
                case ManejadorExcepcion me:
                    logger.LogError(ex, "Manejador Error");
                    errores = me._errores;
                    context.Response.StatusCode = (int)me._codigo;
                    break;
                case Exception e:
                    logger.LogError(ex, "Error de Servidor");
                    errores = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    break;
            }
            context.Response.ContentType = "application/json";
            if (errores != null)
            {
                string resultados = JsonConvert.SerializeObject(new { errores });
                await context.Response.WriteAsync(resultados);
            }
        }
    }
}
