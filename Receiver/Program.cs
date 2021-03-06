﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri listenUri = new Uri("http://127.0.0.1:1357/listener");
            Binding binding = new BasicHttpBinding();

            //创建，开启信道监听器
            IChannelListener<IReplyChannel> channelListener = binding.BuildChannelListener<IReplyChannel>(listenUri);
            channelListener.Open();

            //创建，开启回复信道
            IReplyChannel channel = channelListener.AcceptChannel(TimeSpan.MaxValue);
            channel.Open();

            //监听
            while (true)
            {
                //接收请求消息
                RequestContext requestContext = channel.ReceiveRequest(TimeSpan.MaxValue);
                Console.WriteLine(requestContext.RequestMessage);
                //消息回复
                requestContext.Reply(CreateReplyMessage(binding));
            }

        }

        static Message CreateReplyMessage(Binding binding)
        {
            string action = "http://www.test.com/calculatorservice/AddResponse";
            XNamespace ns = "http://www.test.com";
            XElement body = new XElement(
                new XElement(ns + "AddResponse",
                new XElement(ns + "AddResult", 3)));
            return Message.CreateMessage(binding.MessageVersion, action, body);
        }
    }
}
