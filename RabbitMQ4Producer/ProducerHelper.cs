using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ4Producer
{
    public static class ProducerHelper
    {
        private static byte[] GetBody(string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }

        /// <summary>
        /// 产品最终只能被一个客户消费
        /// Producer -> (Exchange -> Work Queue) -> Consumer
        /// </summary>
        public static void PublishForDistributing(string hostName, string queue, params string[] messages)
        {//queue 与  routingKey一样
            var factory = new ConnectionFactory() { HostName = hostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    /**
                     * we need to make sure that RabbitMQ will never lose our queue. In order to do so, we need to declare it as durable,change needs to be applied to both the producer and consumer code
                     **/
                    channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    var properties = channel.CreateBasicProperties();
                    /**
                     * we need to make sure that RabbitMQ will never lose our queue. In order to do so,we need to mark our messages as persistent.
                     **/
                    properties.Persistent = true;
                    foreach (var msg in messages)
                    {
                        channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: properties, body: GetBody(msg));
                        Console.WriteLine(" [x] Sent {0}", msg);
                    }
                }
            }
        }

        /// <summary>
        /// Fanout,发布给所有订阅者  P -> Exchange -> all*(Binding Queue ->  Consumer)
        /// Firstly, whenever we connect to Rabbit we need a fresh, empty queue. To do this we could create a queue with a random name, or, even better - let the server choose a random queue name for us.
        /// Secondly, once we disconnect the consumer the queue should be automatically deleted.
        /// we supply no parameters to queueDeclare() we create a non-durable, exclusive, autodelete queue with a generated name
        /// The messages will be lost if no queue is bound to the exchange yet, but that's okay for us; if no consumer is listening yet we can safely discard the message.
        /// </summary>
        public static void PublishForSubscribe(string hostName, string exchange, params string[] messages)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout);
                    foreach (var msg in messages)
                    {
                        channel.BasicPublish(exchange: exchange, routingKey: string.Empty, basicProperties: null, body: GetBody(msg));
                        Console.WriteLine(" [x] Sent {0}", msg);
                    }
                }
            }
        }

        /// <summary>
        /// Direct,发布给所有满足Routing规则的订阅者
        /// P -> Exchange -> some*(Binding Queue ->  Consumer)
        /// to subscribe only to a subset of the messages
        /// </summary>
        public static void PublishForRoutingOfSubscribe(string hostName, string exchange, string routingKey, params string[] messages)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct);
                    foreach (var msg in messages)
                    {
                        channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: null, body: GetBody(msg));
                        Console.WriteLine(" [x] Sent {0}", msg);
                    }
                }
            }
        }

        /// <summary>
        /// 发布给匹配上的订阅者
        /// Topic,Receiving messages based on a pattern (topics).
        /// P -> Exchange -> (some or match)*(Binding Queue ->  Consumer)
        /// When a queue is bound with "#" (hash) binding key - it will receive all the messages, regardless of the routing key - like in fanout exchange.()
        /// When special characters "*" (star) and "#" (hash) aren't used in bindings, the topic exchange will behave just like a direct one.(PublishForRoutingOfSubscribe)
        /// </summary>
        public static void PublishForTopic(string hostName, string exchange, string routingKey, params string[] messages)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic);
                    foreach (var msg in messages)
                    {
                        channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: null, body: GetBody(msg));
                        Console.WriteLine(" [x] Sent {0}", msg);
                    }
                }
            }
        }
    }
}
