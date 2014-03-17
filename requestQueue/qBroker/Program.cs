using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Configuration;
using System.Threading;

namespace qBroker
{
    class Program
    {
        static string queueName;
        

        static void Main(string[] args)
        {
            qBrokerService qService = new qBrokerService(ConfigurationManager.AppSettings["queueName"].ToString(),
                ConfigurationManager.AppSettings["queueFileProcess"].ToString(),
                ConfigurationManager.AppSettings["queuePrintBatch"].ToString(),
                ConfigurationManager.AppSettings["queueFileValidation"].ToString());

            qService.ProcessMessage();
                        
            // Keep the service running until the Enter key is pressed.
            Console.WriteLine("The Q Broker is ready.");
            Console.WriteLine("Press the Enter key to terminate service.");
            Console.ReadLine();            
        }
    }

    public class qBrokerService
    {
        private string qName = string.Empty;
        private string qFileProcess = string.Empty;
        private string qPrintBatch = string.Empty;
        private string qFileVal = string.Empty;

        private MessageQueue RequestQueue;
        private MessageQueue ReqQueueFileProcess;
        private MessageQueue ReqQueuePrintBatch;
        private MessageQueue ReqQueueFileValidation;

        public qBrokerService(string queueName, string queueFileProcess, string queuePrintBatch , string queueFileValidation)
        {
            qName = queueName;
            qFileProcess = queueFileProcess;
            qPrintBatch = queuePrintBatch;
            qFileVal = queueFileValidation;

            InitalQueues();
        }

        public void InitalQueues()
        { 
            RequestQueue = new MessageQueue(qName, false);

            if (!MessageQueue.Exists(qFileProcess))
                MessageQueue.Create(qFileProcess, true);//Creates a transactional queue.

            ReqQueueFileProcess = new MessageQueue(qFileProcess, false);;
            ReqQueueFileProcess.MessageReadPropertyFilter.SetAll();

            if (!MessageQueue.Exists(qPrintBatch))
                MessageQueue.Create(qPrintBatch, true);//Creates a transactional queue.

            ReqQueuePrintBatch = new MessageQueue(qPrintBatch, false); ;
            ReqQueuePrintBatch.MessageReadPropertyFilter.SetAll();

            if (!MessageQueue.Exists(qFileVal))
                MessageQueue.Create(qFileVal, true);//Creates a transactional queue

            ReqQueueFileValidation = new MessageQueue(qFileVal, false);
            ReqQueueFileValidation.MessageReadPropertyFilter.SetAll();

        }

        public void ProcessMessage()
        {
            WaitHandle[] waitHandleArray = new WaitHandle[10];
            RequestQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(MyReceiveCompleted);
            for (int i = 0; i < 10; i++)
            {
                // Begin asynchronous operations.
                waitHandleArray[i] =
                    RequestQueue.BeginReceive().AsyncWaitHandle;
            }
            WaitHandle.WaitAll(waitHandleArray);
                       
        }

        private void MyReceiveCompleted(Object source,
           ReceiveCompletedEventArgs asyncResult)
        {
            try
            {
                // Connect to the queue.
                MessageQueue mq = (MessageQueue)source;

                // End the asynchronous receive operation.
                Message m = mq.EndReceive(asyncResult.AsyncResult);
                RoutingMessage(m);
                // Process the message here.
                Console.WriteLine("Message received.");

            }
            catch (MessageQueueException)
            {
                // Handle sources of MessageQueueException.
            }

            // Handle other exceptions. 

            return;
        }

        private void RoutingMessage(Message msg)
        {
            string strPri = msg.Label.ToString().Substring(0, 1);
            //if (RequestQueue.GetAllMessages().Count() > 0)
            {
                //Message msg = RequestQueue.Receive();
                if (strPri == "1")//(msg.Priority == MessagePriority.Normal)
                    ReqQueueFileProcess.Send(msg);
                else
                if (strPri == "2")//(msg.Priority == MessagePriority.High)
                    ReqQueuePrintBatch.Send(msg);
                else
                if (strPri == "3")//(msg.Priority == MessagePriority.Low)
                    ReqQueueFileValidation.Send(msg);
                else
                    ReqQueueFileProcess.Send(msg);

            }
        }
    }
}
