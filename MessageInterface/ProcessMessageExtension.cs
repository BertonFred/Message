using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AgentFramework
{
    public static class ProcessMessageExtension
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

            // Si le type n'est pas basé sur un IProcessMessage
            // il ne traite pas de message
            if (TheType.IsAssignableTo(typeof(IProcessMessage)) == false)
                return lstResult;

            // Recupereation des interfaces de type de messages
            Type[] Interfaces = TheType.GetInterfaces();
            foreach (Type oneInterface in Interfaces)
            {
                // Est-ce un type generique
                if (oneInterface.IsGenericType == false)
                    continue;

                Type typeGenericType = oneInterface.GetGenericTypeDefinition();
                if (typeGenericType == typeof(IProcessMessage<>) == false)
                    continue;

                lstResult.Add(oneInterface.GenericTypeArguments[0]);
            }

            return lstResult;
        }

        /// <summary>
        /// Renvois la liste des types de message traiter par la classe de l'objet
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="typeOfMsg"></param>
        /// <returns></returns>
        static public List<Type> GetProcessedMessages(this IProcessMessage Target, Type typeOfMsg = null)
        {
            return ((object)Target).GetProcessedMessages(typeOfMsg);
        }

        /// <summary>
        /// Renvois la liste des types de message traiter par la classe de l'objet
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="typeOfMsg"></param>
        /// <returns></returns>
        static public List<Type> GetProcessedMessages(this object objTarget, Type typeOfMsg = null)
        {
            if ( (objTarget is IProcessMessage) == false ) 
                return new();

            Type typeOfThis = objTarget.GetType();
            return typeOfThis.GetProcessedMessages(typeOfMsg);
        }

        ///// <summary>
        ///// Retourne la liste des target qui implemente le message indiqué
        ///// </summary>
        ///// <typeparam name="TMSG"></typeparam>
        ///// <param name="This"></param>
        ///// <returns></returns>
        //static public List<T> GetOnReceiveMessageList<T,TMSG>(this IEnumerable<T> Targets)
        //    where T : IProcessMessage
        //    where TMSG : IMessage
        //{
        //    Type typeSearch = typeof(TMSG);
        //    return Targets.GetOnReceiveMessageList(typeSearch);
        //}

        /// <summary>
        /// Retourne la liste des target qui implemente la methode OnReceiveMessage 
        /// pour le type de message indiqué, ou une liste vide
        /// </summary>
        /// <typeparam name="TMSG"></typeparam>
        /// <param name="objTargets"></param>
        /// <returns></returns> GetOnReceiveMessageList
        static public List<IProcessMessage<TMSG>> GetOnReceiveMessageList<TMSG>(this IEnumerable<IProcessMessage<TMSG>> objTargets)
            where TMSG : IMessage
        {
            return objTargets.Where(objTarget => objTarget is IProcessMessage<TMSG>)
                             .ToList();
        }
        static public List<IProcessMessage<TMSG>> GetOnReceiveMessageList<TMSG>(this IEnumerable<object> objTargets)
            where TMSG : IMessage
        {
            return objTargets.Where(objTarget => objTarget is IProcessMessage<TMSG>)
                             .Cast<IProcessMessage<TMSG>>()
                             .ToList();
        }

        /// <summary>
        /// Envois le message msg à tous les objets qui sont des IProcessMessage
        /// donc qui ont une implementation de traitement de message
        /// Seul les objets qui supportent l'interface IProcessMessage<TMSG> sont solicités
        /// Facilite la detection d'erreur a la compilation, par exemple cette methodes n'est possible 
        /// que si l'objet etendu est de type IProcessMessage
        /// </summary>
        static public void SendMessage<TMSG>(this IEnumerable<IProcessMessage> objTargets, TMSG msg)
            where TMSG : IMessage
        {
            ((IEnumerable<object>)objTargets).SendMessage(msg);
        }

        /// <summary>
        /// Envois le message msg à tous les objets qui supportent l'interface IProcessMessage<TMSG>.
        /// Les objets qui ne support pas cette interface ne receverons pas ce message
        /// </summary>
        static public void SendMessage<TMSG>(this IEnumerable<object> objTargets, TMSG msg)
            where TMSG : IMessage
        {
            // recupere la liste des objets qui traite le message TMSG et donc implemente l'interface IProcessMessage<TMSG>
            List<IProcessMessage<TMSG>> TargetsForMsg = objTargets.GetOnReceiveMessageList<TMSG>();

            // Transmettre le message a toutes les parts qui savent le traiter
            TargetsForMsg.ForEach(t => t.SendMessage(msg));
        }

        /// <summary>
        /// Envois le message TMSG à un objet qui support l'interface IProcessMessage<TMSG>.
        /// C'est a dire qu'il dispose d'une methode OnReceiveMessage(TMSG) 
        /// </summary>
        static public void SendMessage<TMSG>(this IProcessMessage<TMSG> objTarget, TMSG msg)
            where TMSG : IMessage
        {
            objTarget.OnReceiveMessage(msg);
        }

        public const string OnReceiveMessageMethodeName = nameof(Agent.OnReceiveMessage);
    }
}