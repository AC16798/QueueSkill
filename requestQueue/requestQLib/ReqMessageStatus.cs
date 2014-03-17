using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RequestQ.requestQLib
{
    public class ReqMessageStatus
    {
        public string ID {get; set;}
        public string Label { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
        public string DateTime { get; set; }
    }
}
