using System;
using System.Collections.Generic;
using System.Text;

namespace HN.Scheduler.Model
{
    public class ResponseData
    {
        public string Message { get; set; }
        public object Response { get; set; }
        public StatusCode Code { get; set; }
        public ResponseData(string message, object response, StatusCode statusCode)
        {
            Message = message;
            Response = response;
            Code = statusCode;
        }
    }
    public enum StatusCode
    { 
        Success = 1,
        Fail = 0
    }
}
