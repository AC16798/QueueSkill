using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;

namespace RequestQ.requestQLib
{
    public class ReqMessageQueue : MessageQueue
    {
        private QueueType qType;
        // e.g. ./public/queuename
        private string qPath;

        public ReqMessageQueue(string path) : base(path) {
            qPath = path;
            inital();
        }
        
        private void inital()
        {   bool isTransactionalQueue = false;
            MessageReadPropertyFilter.SetAll();
            
            if (!System.Messaging.MessageQueue.Exists(qPath))
            {
                MessageQueue q = System.Messaging.MessageQueue.Create(qPath, isTransactionalQueue);
                q.UseJournalQueue = true;
            }
                        
        }

        public QueueType ReqQueueType { get { return qType; } internal set {qType = value;} }
        
    }

    public class ReqMessageQueueJournal : ReqMessageQueue
    {
        public ReqMessageQueueJournal(string path)
            : base(path + ";journal")
        {
            
        }
    }
}
