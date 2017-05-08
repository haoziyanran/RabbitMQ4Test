using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ4Producer
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
            TestPublishForDistributing();
            //TestPublishForSubscribe();
            //TestPublishForRoutingOfSubscribe();
            //TestPublishForTopic();
            Console.ReadKey();
        }

        static void TestPublishForDistributing()
        {
            List<string> msgs = new List<string>();
            for (int i = 0; i < 15; i++)
            {
                msgs.Add("hello!" + i); 
            }
            ProducerHelper.PublishForDistributing("localhost",  "queueName", msgs.ToArray());
            ProducerHelper.PublishForDistributing("localhost", "queueName", "Hello other!");

            //ObjectProducer.PublishForDistributing("localhost", "queueName", new Object
            //{
            //    attr1 = "",
            //    attr2 = ""
            //});
        }

        static void TestPublishForSubscribe()
        {
            for (int i = 0; i < 15; i++)
            {
                ProducerHelper.PublishForSubscribe("localhost", "testSubsribe", "testSubsribe!" + i);
            }
        }

        static void TestPublishForRoutingOfSubscribe()
        {
            //发布两个routing消息，只接收其中一个
            for (int i = 0; i < 15; i++)
            {
                ProducerHelper.PublishForRoutingOfSubscribe("localhost", "testRouting", "RoutingKey1", "RoutingOfSubscribe!" + i);
            }

            ProducerHelper.PublishForRoutingOfSubscribe("localhost", "testRouting", "NRoutingKey", "OtherRoutingOfSubscribe!");
        }

        static void TestPublishForTopic()
        {
            //发布两个routing消息，只接收其中一个
            for (int i = 0; i < 3; i++)
            {
                ProducerHelper.PublishForTopic("localhost", "testTopic", "uih.rabbit.one", "uih.rabbit.one!" + i);
            }
            for (int i = 0; i < 3; i++)
            {
                ProducerHelper.PublishForTopic("localhost", "testTopic", "uih.rabbit.one.one", "uih.rabbit.one.one!" + i);
            }
            for (int i = 0; i < 3; i++)
            {
                ProducerHelper.PublishForTopic("localhost", "testTopic", "uih.rabbit.topic", "uih.rabbit.topic!" + i);
            }
        }
    }
}
