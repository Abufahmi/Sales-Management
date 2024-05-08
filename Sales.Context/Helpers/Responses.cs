using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Context.Helpers
{
    public class Responses
    {
        public record DefaultResponse(bool Flag, string? message = null);
        public record LoginResponse(bool Flag, string? message = null, string? token = null, 
            string? refreshToken = null);
    }
}
