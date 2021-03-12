using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework
{
    public class PartEffector : Part
    {
        public PartEffector(IAgent Parent)
            : base(Parent)
        {
        }

        public void OnReceiveMessage(MessageAttack msg)
        {
            Logger.Info($"OnReceiveMessage MessageAttack {msg.GetType()}");
        }

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    }
}
