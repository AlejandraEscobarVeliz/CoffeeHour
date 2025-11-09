using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeHour.Core.Exceptions
{
    /// <summary>
    /// Excepción personalizada para reglas de negocio.
    /// </summary>
    public class BusinessException : Exception
    {
        public int StatusCode { get; set; } = 400;
        public string ErrorCode { get; set; }

        public BusinessException(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}

