using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class SmsRequest
    {
         public string To { get; set; }
        public string Message { get; set; }
    }
}