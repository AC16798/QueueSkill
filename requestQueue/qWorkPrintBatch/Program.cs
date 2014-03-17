using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;

namespace qWorkPrintBatch
{

    class Program
    {
        static MessageQueue _mq = null;

        static void Main(string[] args)
        {
            _mq = new MessageQueue(@".\private$\RequestQFileProcess", QueueAccessMode.Receive);
            _mq.ReceiveCompleted += new ReceiveCompletedEventHandler(_mq_ReceiveCompleted);
            _mq.Formatter = new ActiveXMessageFormatter();
            MessagePropertyFilter filter = new MessagePropertyFilter();
            filter.Label = true;
            filter.Body = true;
            filter.AppSpecific = true;
            _mq.MessageReadPropertyFilter = filter;
            DoReceive();

            Console.ReadLine();
            _mq.Close();
        }





        static void _mq_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            _mq.EndReceive(e.AsyncResult);
            Console.WriteLine(e.Message.Body);
            DoReceive();
        }

        static private void DoReceive()
        {
            _mq.BeginReceive();
        }

    }
}
