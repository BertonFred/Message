using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework
{
    public class AgentAvecMsgInterface : IAgent,
                                         IProcessMessage<MessageAttack>,
                                         IProcessMessage<MessageMoveTo>
    {
        public AgentAvecMsgInterface() 
        {
            Parts = new();
            Parts.Add(new PartEffector(this));
            Parts.Add(new PartMover(this));
        }

        public void OnReceiveMessage(IMessage msg) 
        {
            Logger.Info($"OnReceiveMessage generic : {msg.GetType()}");

            Parts.SendMessage(msg);
        }

        public void OnReceiveMessage(MessageAttack msg)
        {
            Logger.Info($"OnReceiveMessage MessageAttack");
            Parts.SendMessage(msg);
        }

        public void OnReceiveMessage(MessageMoveTo msg)
        {
            Logger.Info($"OnReceiveMessage MessageMoveTo");
        }

        public List<IPart> Parts { get; set; }

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    }
}
