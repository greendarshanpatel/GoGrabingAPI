using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrabAPI.Utilities
{
    public class Response
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public Object Data { get; set; }




        public Response(object data, int status = 200, string message = "Successfull")
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
