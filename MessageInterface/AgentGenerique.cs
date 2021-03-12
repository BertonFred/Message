using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework
{
    public class AgentGenerique : IAgent
    {
        public AgentGenerique() 
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

        public List<IPart> Parts { get; set; }

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    }
}
