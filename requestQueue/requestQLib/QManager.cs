using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Configuration;
using RequestQ.requestQLib;

namespace RequestQ.requestQLib
{
    public class QManager
    {
        private IList<ReqMessageQueue> queues;
        private QueueConfig qConfiguration;
        private  ReqMessageQueue queueName;
        private  ReqMessageQueue queueFileProcess ;
        private  ReqMessageQueue queuePrintBatch ;
        private  ReqMessageQueue queueFileValidation ;
        private  ReqMessageQueue queueRequest ;
        private  ReqMessageQueue queueOut ;

        public ReqMessageQueue QueueName { get { return queueName; } }
        public ReqMessageQueue QueueFileProcess { get { return queueFileProcess; } }
        public ReqMessageQueue QueuePrintBatch { get { return queuePrintBatch; } }
        public ReqMessageQueue QueueFileValidation { get { return queueFileValidation; } }
        public ReqMessageQueue QueueRequest { get { return queueRequest; } }
        public ReqMessageQueue QueueOut { get { return queueOut; } }

        public QManager()
        {
            queues = new List<ReqMessageQueue>();
        }
        public IList<ReqMessageQueue> Queues { get { return queues; } }

        ReqMessageQueue GetQueueByType(QueueType qType)
        {
            return (ReqMessageQueue)queues.Where(x => x.ReqQueueType == qType);
        }

        public IList<Message> GetMessagesAndJournal(QueueType qType)
        {
            IList<Message> messages = GetMessages(qType);
            ReqMessageQueueJournal q = new ReqMessageQueueJournal(qConfiguration.GetQueueName(qType));
            IList<Message> messagesj = q.GetAllMessages().ToList<Message>();
            return messages.Union<Message>(messagesj).ToList();
        }

        public IList<Message> GeJournals(QueueType qType)
        {
            ReqMessageQueue q = new ReqMessageQueueJournal(qConfiguration.GetQueueName(qType));
            return q.GetAllMessages().ToList<Message>();
        }

        public IList<Message> GetMessages(QueueType qType)
        {
            ReqMessageQueue q = new ReqMessageQueue(qConfiguration.GetQueueName(qType));
            return q.GetAllMessages().ToList<Message>();
        }

        public void LoadQueues(QueueConfig qc)
        {
            qConfiguration = qc;                     

             queueName = new ReqMessageQueue(qc.queueName);
             queueFileProcess = new ReqMessageQueue(qc.queueFileProcess);
             queuePrintBatch = new ReqMessageQueue(qc.queuePrintBatch);
             queueFileValidation = new ReqMessageQueue(qc.queueFileValidation);
             queueRequest = new ReqMessageQueue(qc.queueRequest);
             queueOut = new ReqMessageQueue(qc.queueOut);
        }

    }
}
