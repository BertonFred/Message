using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework
{
    public class Agent : IAgent
    {
        public Agent() 
        {
            Parts = new();
            Parts.Add(new PartEffector(this));
            Parts.Add(new PartMover(this));
        }

        public void OnReceiveMessage()
        {
            Logger.Info($"OnReceiveMessage pas de parametre");
        }

        public void OnReceiveMessage(IMessage msg) 
        {
            Logger.Info($"OnReceiveMessage generic : {msg.GetType()}");

            Parts.SendMessage(msg);
        }


        public void OnReceiveMessage(MessageMoveTo msg)
        {
            Logger.Info($"OnReceiveMessage MessageMoveTo {msg.GetType()}");

        }

        public void OnReceiveMessage(MessageFollowMe msg)
        {
            Logger.Info($"OnReceiveMessage MessageFollowMe {msg.GetType()}");
        }

        public List<IPart> Parts { get; set; }

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    }
}
