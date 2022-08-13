using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Aplicacion.ManejadorError
{
    /// <summary>
    /// Estclase manejara los errores de tipo REST. Es decir, cuando FluentValidation detecte un error
    /// esta será la clase que será invocada para lanzar la excepción
    /// </summary>
    public class ManejadorExcepcion : Exception
    {
        /// <summary>
        /// Contiene los valores de los códigos de estado definidos para HTTP.
        /// </summary>
        public HttpStatusCode _codigo { get; }

        /// <summary>
        /// 
        /// </summary>
        public object _errores { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="errores"></param>
        public ManejadorExcepcion(HttpStatusCode codigo, object errores = null)
        {
            _codigo = codigo;
            _errores = errores;
        }
    }
}
