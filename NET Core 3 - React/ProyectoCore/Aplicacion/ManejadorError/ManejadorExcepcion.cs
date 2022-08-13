using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Aplicacion.ManejadorError
{
    /// <summary>
    /// 
    /// </summary>
    public class ManejadorExcepcion : Exception
    {
        /// <summary>
        /// 
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
