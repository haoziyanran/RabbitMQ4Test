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
    public static class ConsumerHelper
    {
        private static string GetString(byte[] body)
        {
            return Encoding.UTF8.GetString(body);
        }

        /// <summary>
        /// 生产-消费
        /// Producer -> Work Queue -> Consumer
        /// </summary>
        public static void ConsumeForDistributing(string hostName, string queue, Action<string> action)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    /**
                     * we need to make sure that RabbitMQ will never lose our queue. In order to do so, we need to declare it as durable,change needs to be applied to both the producer and consumer code
                     **/
                    channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    /**
                     * with the prefetchCount = 1 setting.This tells RabbitMQ not to give more than one message to a worker at a time.
                     * Or, in other words, don't dispatch a new message to a worker until it has processed and acknowledged the previous one. 
                     * */
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        var message = GetString(e.Body);
                        action(message);
                        //应答RabbitMQ服务，该消息已确认接收完毕！
                        channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
                    };
                    /**
                     * noAck,是否需要应答。
                     * 为true -> 不用应答，once RabbitMQ delivers a message to the customer it immediately removes it from memory
                     * 为false -> 等待应答,当事情已完成再应答RabbitMQ服务，该消息已确认接收完毕！
                     * If a consumer dies (its channel is closed, connection is closed, or TCP connection is lost) without sending an ack, RabbitMQ will understand that a message wasn't processed fully and will re-queue it. If there are other consumers online at the same time, it will then quickly redeliver it to another consumer. 
                     **/
                    channel.BasicConsume(queue: queue, noAck: false, consumer: consumer);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// 发布给所有订阅者Fanout
        /// P -> Exchange -> all*(Binding Queue ->  Consumer)
        /// </summary>
        public static void ConsumeForSubscribe(string hostName, string exchange, Action<string> action)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout);
                    var queue = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queue, exchange: exchange, routingKey: string.Empty);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        var message = GetString(e.Body);
                        action(message);
                    };
                    channel.BasicConsume(queue: queue, noAck: true, consumer: consumer);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// 发布给某些订阅者
        /// P -> Exchange -> some*(Binding Queue ->  Consumer)
        /// </summary>
        public static void ConsumeForRoutingOfSubscribe(string hostName, string exchange, string[] routingKeys, Action<string> action)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct);
                    var queue = channel.QueueDeclare().QueueName;
                    foreach (var routingKey in routingKeys.Distinct())
                    {//每个routingKey绑定一个
                        channel.QueueBind(queue: queue, exchange: exchange, routingKey: routingKey);
                    }
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        var message = GetString(e.Body);
                        action(message);
                    };
                    channel.BasicConsume(queue: queue, noAck: true, consumer: consumer);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// 发布给匹配上的订阅者
        /// P -> Exchange -> (some or match)*(Binding Queue ->  Consumer)
        /// "*" mathch one word,"#"match many words;如 "*.orange.*" only match three words;"lazy.#" match start with "lazy." 
        /// </summary>
        public static void ConsumeForTopic(string hostName, string exchange, string[] patternRoutingKeys, Action<string> action)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic);
                    var queue = channel.QueueDeclare().QueueName;
                    foreach (var routingKey in patternRoutingKeys.Distinct())
                    {//每个routingKey绑定一个
                        channel.QueueBind(queue: queue, exchange: exchange, routingKey: routingKey);
                    }
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        var message = GetString(e.Body);
                        action(message);
                    };
                    channel.BasicConsume(queue: queue, noAck: true, consumer: consumer);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}
