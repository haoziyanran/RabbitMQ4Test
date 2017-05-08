using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ4Producer
{
    public class ObjectProducer
    {
        public static void PublishForDistributing<T>(string hostName, string queue, params T[] messages)
        {
            var msgs = GetStringArr(messages);
            ProducerHelper.PublishForDistributing(hostName, queue, msgs);
        }
    
        private static string[] GetStringArr<T>(params T[] messages)
        {
            string[] msgs;
            if (typeof(T).Equals(typeof(string)))
            {
                msgs = messages as string[];
            }
            else
            {
                msgs = messages.Select(o => JsonConvert.SerializeObject(o)).ToArray();
            }
            return msgs;
        }
    }
}
