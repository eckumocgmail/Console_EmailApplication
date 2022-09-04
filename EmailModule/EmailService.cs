
using MailKit.Net.Pop3;
using MailKit.Security;
using MimeKit;

using System;
using System.IO;
using System.Text;

/// <summary>
/// Сервис умеет отправлять электронную почту и считывать входящие письма
/// </summary>
public class EmailService
{
    private string EmailName;
    private string EmailAddress;
    private string EmailPassword;
    private string SmtpHost;
    private int SmtpPort;        
    private string PopHost;
    private int PopPort;
       

    public EmailService()
    {
        this.EmailName = "Администрация сайта";
        this.EmailAddress = "kba-2018@mail.ru";
        this.EmailPassword = "TSTUiyuat11)";
        this.SmtpHost = "smtp.mail.ru";
        this.SmtpPort = 587;
        this.PopHost = "pop.mail.ru";
        this.PopPort = 995;
    }


    /// <summary>
    /// Отправка сообщения по электронной почте
    /// </summary> 
    public void SendEmail(string email, string subject, string message)
    {
        using (var smtp = new MailKit.Net.Smtp.SmtpClient())
        {
            smtp.ServerCertificateValidationCallback = (s, c, h, e) =>
            {
                System.Console.Write(s.GetType().FullName);
                System.Console.WriteLine(s);

                System.Console.Write(s.GetType().FullName);
                System.Console.WriteLine(c);

                System.Console.Write(s.GetType().FullName);
                System.Console.WriteLine(h);

                System.Console.Write(s.GetType().FullName);
                System.Console.WriteLine(e);
                return true;
            };

            //smtp.Connect(smtpHost, smtpPort, SecureSocketOptions.StartTls);
            //smtp.Connect(smtpHost, smtpPort);



            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(this.EmailName, EmailAddress));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            var data = ToByteArray(emailMessage);
            string text = Encoding.UTF8.GetString(data);

            Console.WriteLine(text);

            smtp.Authenticate("eckumoc@gmail.com", "shahraeN");
            smtp.Send(emailMessage);
            smtp.Disconnect(true);
        }
        
        /*
                
      
        }*/
    }

    private byte[] ToByteArray(MimeMessage emailMessage)
    {
        using (var stream = new MemoryStream())
        {
            emailMessage.WriteTo(FormatOptions.Default, stream);
            byte[] data = stream.ToArray();
            return data;
        }            
    }

    private FormatOptions GetDosFormatOptions()
    {
        
        return new FormatOptions()
        {
            NewLineFormat = NewLineFormat.Dos,
            MaxLineLength = 80,
            International = false,
            ParameterEncodingMethod = ParameterEncodingMethod.Default
        };
    }


    public class FileItem
    {
        public string Mime { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public DateTime Changed { get; set; }
    }

    /// <summary>
    /// Отправка сообщения по электронной почте с прикреплёнными файлами
    /// </summary> 
    public void SendEmail(string email, string subject, string message, 
                            FileItem[] resources)
    {
        using (var smtp = new MailKit.Net.Smtp.SmtpClient())
        {
            smtp.ServerCertificateValidationCallback = (s, c, h, e) =>
            {
                return true;
            };

            smtp.Connect(SmtpHost, SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(EmailAddress, EmailPassword);
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(this.EmailName, EmailAddress));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            var builder = new BodyBuilder();
                
            builder.TextBody = message;
            if (resources != null)
            {
                foreach(FileItem resource in resources)
                {
                    System.IO.File.WriteAllBytes(resource.Name, resource.Data);
                    builder.Attachments.Add(resource.Name);
                }
            }                             
            emailMessage.Body = builder.ToMessageBody();
               
            smtp.Send(emailMessage);
            smtp.Disconnect(true);
        }
    }


    /// <summary>
    /// Получение входящих сообщений
    /// </summary>
    public void Recieve()
    {
        using (var client = new Pop3Client())
        {                
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            client.Connect(PopHost, PopPort);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(EmailAddress, this.EmailAddress );
            for (int i = 0; i < client.Count; i++)
            {
                MimeMessage message = client.GetMessage(i);                    
            }
            client.Disconnect(true);
        }
    }
}
