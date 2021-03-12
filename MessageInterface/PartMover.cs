using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework
{
    public class PartMover: Part,
                            IProcessMessage<MessageMoveTo>,
                            IProcessMessage<MessageFollowMe>
    {
        public PartMover(IAgent Parent)
            : base(Parent)
        {
        }

        public void OnReceiveMessage(MessageMoveTo msg)
        {
            Logger.Info($"OnReceiveMessage MessageMoveTo {msg.GetType()}");
        }

        public void OnReceiveMessage(MessageFollowMe msg)
        {
            Logger.Info($"OnReceiveMessage MessageFollowMe {msg.GetType()}");
        }

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    }
}
