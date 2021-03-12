using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework
{
    public class Part : IPart
    {
        public Part(IAgent Parent) 
        {
            this.Parent = Parent;
        }

        public IAgent Parent { get; init; }

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    }
}
