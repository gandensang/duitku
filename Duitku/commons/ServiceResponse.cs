using System;
using System.Collections.Generic;
using System.Text;

namespace Duitku.commons
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Ok";
        public T Data { get; set; }
    }
}
