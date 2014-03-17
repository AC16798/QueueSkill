using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Messaging;
using System.Threading;
using RequestQ.requestQLib;

namespace WorkerFileValidation
{
    class Program
    {
        static QueueType thisQType = QueueType.ValidationQueue;
        static QueueConfig qc;
        static ReqMessageQueue myQueue;
        static ReqMessageQueue qOut;

        static void Main(string[] args)
        {
            //QManager qMgr = new QManager();
            //qMgr.LoadQueues(ConfigurationSettings.AppSettings);
            Console.WriteLine("Application Queue Name " + Enum.GetName(thisQType.GetType(), thisQType) + " started.");
            ProcessInital();

            // Create an instance of MessageQueue. Set its formatter.           
            myQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });

            // Add an event handler for the ReceiveCompleted event.
            myQueue.ReceiveCompleted +=
                new ReceiveCompletedEventHandler(MyReceiveCompleted);

            // Define wait handles for multiple operations.
            WaitHandle[] waitHandleArray = new WaitHandle[10];
            for (int i = 0; i < 10; i++)
            {
                // Begin asynchronous operations.
                waitHandleArray[i] =
                    myQueue.BeginReceive().AsyncWaitHandle;
            }

            // Specify to wait for all operations to return.
            WaitHandle.WaitAll(waitHandleArray);

            return;

        }

        static void ProcessInital()
        {
            var ts = new TimeSpan(0, 0, 10);
            qc = new QueueConfig(ConfigurationSettings.AppSettings);
            myQueue = new ReqMessageQueue(qc.queueName);
            qOut = new ReqMessageQueue(qc.queueOut);

        }

        //*************************************************** 
        // Provides an event handler for the ReceiveCompleted 
        // event. 
        //*************************************************** 

        private static void MyReceiveCompleted(Object source,
            ReceiveCompletedEventArgs asyncResult)
        {
            try
            {
                // Connect to the queue.
                MessageQueue mq = (MessageQueue)source;

                // End the asynchronous receive operation.
                Message m = mq.EndReceive(asyncResult.AsyncResult);
                HandleMessage(m);
                // Process the message here.
                Console.WriteLine("Message received: " + m.Label);

            }
            catch (MessageQueueException e)
            {
                Console.WriteLine("MessageQueueException:" + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Application Exception:" + e.Message);
            }
            // Handle other exceptions. 

            return;
        }

        static void HandleMessage(Message msg)
        {
            //msg.Label += "Processed by " + requestQLib.Utility.GetDescriptionFromEnumValue(thisQType);

            qOut.Send(msg);
            System.Threading.Thread.Sleep(1000); // 2 sec
        }
    }
}
