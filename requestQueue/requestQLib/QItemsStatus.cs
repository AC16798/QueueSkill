using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Data;

namespace RequestQ.requestQLib
{
    public class QItemsStatus
    {
        private QManager qmanager;

        public QItemsStatus(QManager manager)
        {
            qmanager = manager;
        }

        public IEnumerable<ReqMessageStatus> GetStatus()
        {
            IList<Message> list1 = qmanager.GetMessagesAndJournal(QueueType.RequestQueue);
            IList<Message> list2 = qmanager.GetMessages(QueueType.FileProcessQueue);
                        
            System.Messaging.XmlMessageFormatter stringFormatter = new System.Messaging.XmlMessageFormatter(new string[] { "System.String" });
            
            //for (int index = 0; index < messages.Length; index++)
            //{
            //    string test = System.Convert.ToString(messages[index].Priority);
            //    messages[index].Formatter = stringFormatter;
            //    messageTable.Rows.Add(new string[] { messages[index].Label, messages[index].Body.ToString(), "New", messages[index].ArrivedTime.ToShortTimeString() });

            //}

            IEnumerable<ReqMessageStatus> qfileStatus = (from item1 in list1
                                   join item2 in list2
                                   on item1.Label equals item2.Label into g
                                   from o in g.DefaultIfEmpty()
                                   select new ReqMessageStatus { ID = item1.Id, Label = item1.Label,
                                                                 Body = string.Empty,
                                                                 Status = (o == null ? "New" : "In FileProcess Queue"),
                                                                 DateTime = (o == null ? item1.ArrivedTime.ToString("yyyy/MM/dd HH:mm:ss") : o.SentTime.ToString("yyyy/MM/dd HH:mm:ss"))
                                   });

            IEnumerable<Message> list3 = qmanager.GetMessages(QueueType.PrintBatchQueue);

            IEnumerable<ReqMessageStatus> qPrintBatchStatus = (from item1 in qfileStatus
                                                         join item2 in list3
                                                          on item1.Label equals item2.Label into g
                                                               from o in g.DefaultIfEmpty()
                                                         select new ReqMessageStatus
                                                         {
                                                             ID = item1.ID,
                                                             Label = item1.Label,
                                                             Body = string.Empty,
                                                             Status = (o == null ? item1.Status : "In PrintBatch Queue"),
                                                             DateTime = (o == null ? item1.DateTime : o.SentTime.ToString("yyyy/MM/dd HH:mm:ss"))
                                                         });
                        
            IList<Message> list4 = qmanager.GetMessages(QueueType.ValidationQueue);

            IEnumerable<ReqMessageStatus> qValQ = (from item1 in qPrintBatchStatus
                                                               join item2 in list4
                                                               on item1.Label equals item2.Label into g
                                                               from o in g.DefaultIfEmpty()
                                                               select new ReqMessageStatus
                                                               {
                                                                   ID = item1.ID,
                                                                   Label = item1.Label,
                                                                   Body = string.Empty,
                                                                   Status = (o == null ? item1.Status : "In Validation Queue"),
                                                                   DateTime = (o == null ? item1.DateTime : o.SentTime.ToString("yyyy/MM/dd HH:mm:ss"))
                                                               });

            IList<Message> list5= qmanager.GetMessages(QueueType.QueueOut);

            IEnumerable<ReqMessageStatus> qOutQ = (from item1 in qValQ
                                                   join item2 in list5
                                                   on item1.Label equals item2.Label into g
                                                   from o in g.DefaultIfEmpty()
                                                   select new ReqMessageStatus
                                                   {
                                                       ID = item1.ID,
                                                       Label = item1.Label,
                                                       Body = string.Empty,
                                                       Status = (o == null ? item1.Status : "In Queue delivery"),
                                                       DateTime = (o == null ? item1.DateTime : o.SentTime.ToString("yyyy/MM/dd HH:mm:ss"))
                                                   });

            IList<Message> list6 = qmanager.GetMessages(QueueType.QueueOut);

            IEnumerable<ReqMessageStatus> qCompletedQ = (from item1 in qOutQ
                                                   join item2 in list6
                                                   on item1.Label equals item2.Label into g
                                                   from o in g.DefaultIfEmpty()
                                                   select new ReqMessageStatus
                                                   {
                                                       ID = item1.ID,
                                                       Label = item1.Label,
                                                       Body = string.Empty,
                                                       Status = (o == null ? item1.Status : "Ready to delivery"),
                                                       DateTime = (o == null ? item1.DateTime : o.SentTime.ToString("yyyy/MM/dd HH:mm:ss"))
                                                   });

            IList<Message> list7 = qmanager.GeJournals(QueueType.QueueOut);
            IEnumerable<ReqMessageStatus> qCompletedOut = (from item1 in qCompletedQ
                                                         join item2 in list7
                                                         on item1.Label equals item2.Label into g
                                                         from o in g.DefaultIfEmpty()
                                                         select new ReqMessageStatus
                                                         {
                                                             ID = item1.ID,
                                                             Label = item1.Label,
                                                             Body = string.Empty,
                                                             Status = (o == null ? item1.Status : "Requested item is completed"),
                                                             DateTime = (o == null ? item1.DateTime : o.SentTime.ToString("yyyy/MM/dd HH:mm:ss"))
                                                         });

            return qCompletedOut;
        }
    }
}
