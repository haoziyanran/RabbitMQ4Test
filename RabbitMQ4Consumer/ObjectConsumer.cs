using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ4Consumer
{
    public class ObjectConsumer
    {
        public static void ConsumeForDistributing<T>(string hostName, string queue, Action<T> action) where T : class
        {
            ConsumerHelper.ConsumeForDistributing(hostName, queue, (msg) => 
            {
                T msgObj;
                if (typeof(T).Equals(typeof(string)))
                {
                    msgObj = msg as T;
                }
                else
                {
                    msgObj = JsonConvert.DeserializeObject<T>(msg);
                }
                action(msgObj);
            });
        }
    }
}
