using System;

namespace Commons
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "OK";
        public T Data { get; set; }
    }
}
