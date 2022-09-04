using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Console22_EmailProgram.EmailModule
{
    public class SendSmtpMessage: System.Net.Mail.SmtpClient
    {
        static bool mailSent = false;
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }
        public static void Run( )
        {

            string smtpHost = "127.0.0.1";
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(smtpHost);

            string email = "jane@contoso.com";
            string name = "Jane " + (char)0xD8 + " Clayton";
            MailAddress from = new MailAddress(email, name, System.Text.Encoding.UTF8);

            string emailTo = "ben@contoso.com";
            MailAddress to = new MailAddress(emailTo);

            string messageText = "This is a test email message sent by an application. ";
            string subject = "test";
            MailMessage message = new MailMessage(from, to);
            message.Body = messageText;
            
            //string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
            //message.Body += Environment.NewLine + someArrows;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = subject;
            // message.Subject = "test message 1" + someArrows;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            // Set the method that is called back when the send operation ends.
            client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            // The userState can be any object that allows your callback
            // method to identify this send operation.
            // For this example, the userToken is a string constant.
            string userState = "test message1";
            client.SendAsync(message, userState);

            Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.");
            string answer = Console.ReadLine();
            // If the user canceled the send, and mail hasn't been sent yet,
            // then cancel the pending operation.
            if (answer.StartsWith("c") && mailSent == false)
            {
                client.SendAsyncCancel();
            }
            // Clean up.
            message.Dispose();
            Console.WriteLine("Goodbye.");
        }
    }
}
