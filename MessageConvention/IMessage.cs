using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework
{

    /// <summary>
    /// Interface d'un message
    /// </summary>
    public interface IMessage
    {
        string Source { get; set; }
        string Destination { get; set; }
    }
}
