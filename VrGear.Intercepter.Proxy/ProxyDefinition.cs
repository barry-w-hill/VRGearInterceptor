using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NRepeat
{
    public class ProxyDefinition
    {
        public IPAddress ServerAddress { get; set; }
        public IPAddress ClientAddress { get; set; }
        public int ServerPort { get; set; }
        public int ClientPort { get; set; }
    }
}
