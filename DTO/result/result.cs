using System;
namespace DTO.result
{
    public class result
    {
        public bool success { get; set; }
        public string message { get; set; }
        public object data { get; set; }

        public result(bool success, string message, object data)
        {
            this.success = success;
            this.message = message;
            this.data = data;
        }
    }
}