using System;
using System.Collections.Generic;
using System.Text;

namespace PMS.Shared.Exceptions
{
    public class ErrorResponse
    {
        public Error Error { get; set; }
    }
    public class Error
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public List<ErrorDetail> Details { get; set; }
    }
    public class ErrorDetail
    {
        public string Target { get; set; }
        public string Message { get; set; }
    }
}
