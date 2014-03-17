using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Messaging;

namespace RequestQ.requestQLib
{
    public class MessageCreater
    {
        private object messageBody;

        public string FileName { get; set; }

        public MessageCreater(){}

        public MessageCreater(object data)
        {
            messageBody = data;
        }
        
        public Message CreateMessage(QueueType qType)
        {
            var extenalMessage = new ExternalMessage();

            if (messageBody != null)
            {
                extenalMessage = new ExternalMessage(messageBody) {FileName = FileName};
            }

            var theMessage = new Message(extenalMessage)
                                 {
                                     Formatter = new XmlMessageFormatter(new[] { "RequestQ.requestQLib.ExternalMessage" }),
                                     Label = ((int) qType).ToString() + "|" + Guid.NewGuid()
                                 };

            return theMessage;
        }
    }

    [Serializable]
    public class ExternalMessage
    {
        public string FileName { get; set; }
        //public FileStream FileStream { get; set; }

        public ExternalMessage() {}

        public ExternalMessage(object o)
        {
            Content = new MessageContent { InnerMessageContent = o };
        }

        public MessageContent Content { get; set; }
    }

    [Serializable]
    public class MessageContent
    {
        public object InnerMessageContent { get; set; }
    }
}
