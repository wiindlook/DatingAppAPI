using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Errors
{
    public class ApiException
    {
        public ApiException(int statusCode, string message=null, string details=null) //status code pt o exceptie o sa fie mereu 500, se da null in caz ca nu na dam nicio valoare,
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}
