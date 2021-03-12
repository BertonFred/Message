using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework
{
    public class  MessageFollowMe : Message
    {
        public string Me { get; set; }
        public double Distance { get; set; }
    }
}
