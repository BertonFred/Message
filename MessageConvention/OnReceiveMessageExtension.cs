using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AgentFramework
{
    public static class OnReceiveMessageExtension
    {
        /// <summary>
        /// Renvois la liste des types de message traiter par la classe correspondant au type
        /// </summary>
        /// <param name="TheType"></param>
        /// <param name="typeOfMsg"></param>
        /// <returns></returns> 
        static public List<Type> GetProcessedMessages(this Type TheType, Type typeOfMsg = null)
        {
            List<Type> lstResult = new();
            // si le type n'est pas une classe, il ne peut pas traité de message
            if (TheType.IsClass == false)
                return lstResult;

            typeOfMsg ??= typeof(IMessage);
            MethodInfo[] mis = TheType.GetMethods();
            MethodInfo[] miMsgHandlers = mis.Where(mi => mi.IsPublic == true &&
                                                         mi.Name.Equals(OnReceiveMessageMethodeName) == true)
                                            .ToArray();
            foreach (MethodInfo mi in miMsgHandlers)
            {
                ParameterInfo[] pis = mi.GetParameters();
                if (pis.Length == 0)
                    continue;

                if (pis[0].ParameterType == typeOfMsg ||
                    pis[0].ParameterType.IsAssignableTo(typeOfMsg))
                {
                    lstResult.Add(pis[0].ParameterType);
                }
            }

            return lstResult;
        }

        /// <summary>
        /// Renvois la liste des types de message traiter par la classe de l'objet
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="typeOfMsg"></param>
        /// <returns></returns>
        static public List<Type> GetProcessedMessages<T>(this T Target, Type typeOfMsg = null)
            where T : class
        {
            Type typeOfThis = Target.GetType();
            return typeOfThis.GetProcessedMessages(typeOfMsg);
        }

        /// <summary>
        /// Retourne la liste des target qui implemente le message indiqué
        /// </summary>
        /// <typeparam name="TMSG"></typeparam>
        /// <param name="This"></param>
        /// <returns></returns>
        static public List<T> GetOnReceiveMessageList<T,TMSG>(this IEnumerable<T> Targets) 
            where TMSG : IMessage
        {
            Type typeSearch = typeof(TMSG);
            return Targets.GetOnReceiveMessageList(typeSearch);
        }

        /// <summary>
        /// Retourne la liste des target qui implemente la methode OnReceiveMessage 
        /// pour le type de message indiqué, ou une liste vide
        /// </summary>
        /// <typeparam name="TMSG"></typeparam>
        /// <param name="Targets"></param>
        /// <returns></returns> GetOnReceiveMessageList
        static public List<T> GetOnReceiveMessageList<T>(this IEnumerable<T> Targets, Type typeSearch=null)
        {
            List<T> lstResult = new();
            
            foreach (T target in Targets)
            {
                Type typeOfTarget = target.GetType();
                MethodInfo[] mis = typeOfTarget.GetMethods();
                MethodInfo[] miMsgHandlers = mis.Where(mi => mi.IsPublic == true &&
                                                             mi.Name.Equals(OnReceiveMessageMethodeName) == true)
                                                .ToArray();
                foreach (MethodInfo mi in miMsgHandlers)
                {
                    ParameterInfo[] pis = mi.GetParameters();
                    if (pis.Length == 0)
                        continue;
                    if ( (typeSearch == null && pis[0].ParameterType.IsAssignableTo(typeof(IMessage))) ||
                          pis[0].ParameterType == typeSearch)
                    {
                        lstResult.Add(target);
                    }
                }
            }

            return lstResult;
        }

        /// <summary>
        /// Envois le message a toutes les parts qui savent le traiter
        /// renvois true si au moins une part traite lm message
        /// </summary>
        static public bool SendMessage(this IEnumerable<object> Targets, IMessage msg)
        {
            bool MessageSend = false;

            // recupere la liste dess part qui traite le message
            List<object> PartsForMsg = Targets.GetOnReceiveMessageList(msg.GetType());
            Type typeReelMsg = msg.GetType();

            // Transmettre le message a toutes les parts qui savent le traiter
            foreach (IPart part in PartsForMsg)
            {
                MessageSend = part.SendMessage(msg);
            }

            return MessageSend;
        }

        /// <summary>
        /// Envois le message à un objet s'il support le message.
        /// C'est a dire qu'il dispose d'une methode OnReceiveMessageMethodeName et qu'elle prend en parametre le type de message
        /// </summary>
        static public bool SendMessage(this object objTarget, IMessage msg)
        {
            bool MessageSend = false;
            Type typeReelMsg = msg.GetType();

            // Transmettre le message a toutes les parts qui savent le traiter
            // invoquer par convention la methode de traitement, si elle existe
            MethodInfo mi = objTarget.GetType().GetMethod(OnReceiveMessageMethodeName, new Type[] { typeReelMsg });
            if (mi != null)
            {
                mi.Invoke(objTarget, new object[] { msg });
                MessageSend = true;
            }

            return MessageSend;
        }

        public const string OnReceiveMessageMethodeName = nameof(Agent.OnReceiveMessage);
    }
}