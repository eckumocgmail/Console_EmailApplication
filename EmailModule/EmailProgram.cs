using System;
using System.Threading;

namespace Console22_EmailProgram
{
    using static System.Threading.Tasks.Task;

    internal class Program
    {
        static void Main(string[] args)
        {
            Run(() => {
                TcpServerEndpoint.Run("127.0.0.1", 13000);
            });
            Thread.Sleep(1000);
            /*var service = new EmailService();
            service.SendEmail("eckumoc@gmail.com","XXX","this is a test");*/
        }
    }
}
