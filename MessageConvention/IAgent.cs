using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework
{
    public interface IAgent 
    {
        List<IPart> Parts { get; set; }
    }
}
