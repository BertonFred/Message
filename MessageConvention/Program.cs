using System;
using System.Collections.Generic;

namespace AgentFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            DumpClassOnReceiveMessageCapability();
            TestMessagePolymorpheSurAgent();

            Console.ReadKey();
        }

        static void TestMessagePolymorpheSurAgent()
        {
            Logger.Info($"TestMessagePolymorpheSurAgent");
            Logger.Info($"Agent");
            Agent agent = new();
            agent.OnReceiveMessage(new MessageMoveTo()); // connu de l'agent
            agent.OnReceiveMessage(new MessageFollowMe()); // connu de l'agent
            agent.OnReceiveMessage(new MessageAttack()); // inconnu de l'agent
            Logger.Info($"**");
            Logger.Info($"AgentGenerique");
            AgentGenerique agentGenerique = new();
            agentGenerique.OnReceiveMessage(new MessageMoveTo()); // connu de l'agent
            agentGenerique.OnReceiveMessage(new MessageFollowMe()); // connu de l'agent
            agentGenerique.OnReceiveMessage(new MessageAttack()); // inconnu de l'agent
        }

        static void DumpClassOnReceiveMessageCapability()
        {
            Logger.Info($"DumpClassOnReceiveMessageCapability");
            DumpClassOnReceiveMessageCapability(typeof(Agent));
            DumpClassOnReceiveMessageCapability(typeof(AgentGenerique));
            DumpClassOnReceiveMessageCapability(typeof(OnReceiveMessageExtension));
            DumpClassOnReceiveMessageCapability(typeof(Part));
            DumpClassOnReceiveMessageCapability(typeof(PartEffector));
            DumpClassOnReceiveMessageCapability(typeof(PartMover));
            Logger.Info($"**");
        }

        static void DumpClassOnReceiveMessageCapability(Type typeOfClass)
        {
            Logger.Info($"Message traiter par : {typeOfClass.Name}");
            foreach (Type typeMsg in typeOfClass.GetProcessedMessages())
                Logger.Info($"  Msg : {typeMsg.Name}");
        }

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    }
}
