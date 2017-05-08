using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ4Consumer
{
    class Object
    {
        public string attr1 { get; set; }
        public string attr2 { get; set; }
    }
    
    class Program
    {

        static void Main(string[] args)
        {
            TestConsumeForDistributing();
            //TestConsumeForSubscribe();
            //TestConsumeForRoutingOfSubscribe();
            //TestConsumeForTopic();
        }

        static void TestConsumeForDistributing()
        {
            ConsumerHelper.ConsumeForDistributing("localhost", "queueName", (msg) =>
            {
                Console.WriteLine(" [x] Received {0}", msg);
                Thread.Sleep(3 * 1000);
                Console.WriteLine(" [x] Done");
            });
       
            //MyConsumer.ConsumeForDistributing<Object>("localhost", "queueName", (msg) =>
            //{
            //    Console.WriteLine(" [x] Received {0}", msg.attr);
            //    Thread.Sleep(300 * 1000);
            //    Console.WriteLine(" [x] Done");
            //});
        }

        static void TestConsumeForSubscribe()
        {
            ConsumerHelper.ConsumeForSubscribe("localhost", "testSubsribe", (msg) =>
            {
                Console.WriteLine(" [x] Received {0}", msg);
                //Thread.Sleep(5 * 1000);
                Console.WriteLine(" [x] Done");
            });
        }

        static void TestConsumeForRoutingOfSubscribe()
        {
            ConsumerHelper.ConsumeForRoutingOfSubscribe("localhost", "testRouting", new string[] { "RoutingKey1", "RoutingKey2" }, (msg) =>
            {
                Console.WriteLine(" [Routing] Received {0}", msg);
                //Thread.Sleep(5 * 1000);
                Console.WriteLine(" [Routing] Done");
            });
        }

        static void TestConsumeForTopic()
        {
            //符合预期  uih.rabbit.one.one  -> lost
            ConsumerHelper.ConsumeForTopic("localhost", "testTopic", new string[] { "uih.rabbit.*", "uih.*.topic" }, (msg) =>
            {
                Console.WriteLine(" [Routing] Received {0}", msg);
                //Thread.Sleep(5 * 1000);
                Console.WriteLine(" [Routing] Done");
            });

            //ConsumerHelper.ConsumeForTopic("localhost", "testTopic", new string[] { "uih.#" }, (msg) =>
            //{
            //    Console.WriteLine(" [Routing] Received {0}", msg);
            //    //Thread.Sleep(5 * 1000);
            //    Console.WriteLine(" [Routing] Done");
            //});
        }
    }
}
