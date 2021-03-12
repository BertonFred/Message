using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework
{
    public abstract class  Message : IMessage
    {
        public string Source { get; set; }
        public string Destination { get; set; }
    }
}
