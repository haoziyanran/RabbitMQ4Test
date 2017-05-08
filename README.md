# RabbitMQ4Test

	What is RabbitMQ?
a.	MQ全称为Message Queue，消息队列（MQ）是一种应用程序对应用程序的通信方法。
b.	消息中间件是一种由消息传送机制或消息队列模式组成的中间件技术 -> RabbitMQ。
c.	one Service Side -> many Client。
d.	遵循先进先出原则。

	Why Use?
a.	异步、复杂业务解耦。
b.	支持负载均衡、消息持久化、支持路由。
c.	拥有成熟的社区、开源免费、稳定。

	What can RabbitMQ do for you?
a.	普通的生产者消费者模式，支持多个客户端支持负载均衡、支持路由。
   Producer -> (Exchange -> Work Queue) -> Consumer
b.	Fanout，发布给所有订阅者  
   Producer -> Exchange -> all * (Binding Queue ->  Consumer)
c.	Direct，发布给所有满足Routing规则的订阅者
   Producer -> Exchange -> some * (Binding Queue ->  Consumer)
d.	Topic，发布给匹配上的订阅者
   Producer -> Exchange -> (some or match) * (Binding Queue ->  Consumer)

