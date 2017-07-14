using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri listenUri = new Uri("http://127.0.0.1:1357/listener");
            Binding binding = new BasicHttpBinding();

            //创建，开启信道工厂
            IChannelFactory<IRequestChannel> channelFactory = binding.BuildChannelFactory<IRequestChannel>();
            channelFactory.Open();

            //创建，开启请求信道
            IRequestChannel channel = channelFactory.CreateChannel(new EndpointAddress(listenUri));
            channel.Open();


            //发送请求消息，接收回复
            Message replyMessage = channel.Request(CreateRequestMessage(binding));
            Console.WriteLine(replyMessage);
            Console.Read();

        }

        static Message CreateRequestMessage(Binding binding)
        {
            string action = "http://www.test.com/calculatorservice/Add";
            XNamespace ns = "http://www.test.com";
            XElement body = new XElement(
                new XElement(ns + "Add",
                new XElement(ns + "x", 1),
                new XElement(ns + "y", 2)));
            return Message.CreateMessage(binding.MessageVersion, action, body);
        }
    }
}
