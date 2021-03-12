using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework
{
    /// <summary>
    /// Declaration de la capcité a prendre en charque un message de type T_MSG
    /// via la methode de traitement du message OnReceiveMessage
    /// </summary>
    /// <typeparam name="T_MSG"></typeparam>
    public interface IProcessMessage<T_MSG> : IProcessMessage
        where T_MSG : IMessage
    {
        void OnReceiveMessage(T_MSG msg);
    }

    /// <summary>
    /// Interface marker
    /// permet de savoir qu'une classe implemente un IProcessMessage
    /// Cette interface est utilisée pour identifier les classes qui implemnte un IProcessMessage<T_MSG> 
    /// </summary>
    public interface IProcessMessage
    {
    }
}
