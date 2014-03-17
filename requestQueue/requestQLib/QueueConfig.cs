using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Configuration;

namespace RequestQ.requestQLib
{
    public class QueueConfig
    {
        private string _queueName;
        private string _queueFileProcess;
        private string _queuePrintBatch;
        private string _queueFileValidation;
        private string _queueRequest;
        private string _queueOut;

        public NameValueCollection values { get; private set; }
        public QueueConfig(NameValueCollection AppSettings)
        {
                values = AppSettings;
                _queueName = AppSettings[AppSettings["queueName"].ToString()].ToString();
                _queueFileProcess = AppSettings["queueFileProcess"].ToString();
                _queuePrintBatch = AppSettings["queuePrintBatch"].ToString();
                _queueFileValidation = AppSettings["queueFileValidation"].ToString();
                _queueRequest = AppSettings["queueRequest"].ToString();
                _queueOut = AppSettings["queueOut"].ToString();

            //foreach (string key in AppSettings)
            //{
            //    string value = AppSettings[key];
            //    if (value.IndexOf("queues") > 0)
            //    {
            //        ReqMessageQueue q = new ReqMessageQueue(value);
            //        if (q != null)
            //            queues.Add(q);
            //    }
            //}
            //    ConfigurationManager.AppSettings["queueName"].ToString(),
            //    ConfigurationManager.AppSettings["queueFileProcess"].ToString(),
            //    ConfigurationManager.AppSettings["queuePrintBatch"].ToString(),
            //    ConfigurationManager.AppSettings["queueFileValidation"].ToString());
        }

        public string GetQueueName(QueueType qType)
        {
            if (qType == QueueType.FileProcessQueue)
                return values["queueFileProcess"].ToString();//"queueFileProcess";
            if (qType == QueueType.PrintBatchQueue)
                return values["queuePrintBatch"].ToString();//"queuePrintBatch";
            if (qType == QueueType.ValidationQueue)
                return values["queueFileValidation"].ToString();//"queueFileValidation";
            if (qType == QueueType.QueueOut)
                return values["queueOut"].ToString();//"queueOut";
            if (qType == QueueType.RequestQueue)
                return values["queueRequest"].ToString();//"queueRequest";
            else
                return string.Empty;

        }
        public string queueName { get { return _queueName; } set { _queueName = value; } }
        public string queueFileProcess { get { return _queueFileProcess; } set { _queueFileProcess = value; } }
        public string queuePrintBatch { get { return _queuePrintBatch; } set { _queuePrintBatch = value; } }
        public string queueFileValidation { get { return _queueFileValidation; } set { _queueFileValidation = value; } }
        public string queueRequest { get { return _queueRequest; } set { _queueRequest = value; } }
        public string queueOut { get { return _queueOut; } set { _queueOut = value; } }
    }
}
