using MailKit.Net.Pop3;
using MailKit.Security;

using MimeKit;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Сервис работает с протоколами электронной почты
/// </summary>
public class EmailService: EmailOptions
{
    /*private string emailName;
    private string emailAddress;
    private string emailPassword;
    private string smtpHost;
    private int smtpPort;
    private string popHost;
    private int popPort;*/
    

    public EmailService()
    {
        this.EmailName = "Администрация сайта";
        this.EmailAddress = "kba-2018@mail.ru";
        this.EmailPassword = "T*7ylhrzTR7B";
        this.SmtpHost = "smtp.mail.ru";
        this.SmtpPort = 587;
        this.PopHost = "pop.mail.ru";
        this.PopPort = 995;
    }

    public EmailService( EmailOptions options ): base( options )
    {
        
    }


    /// <summary>
    /// Отправка сообщения по электронной почте
    /// </summary> 
    public void SendEmail(string email, string subject, string message)
    {
        using (var smtp = new MailKit.Net.Smtp.SmtpClient())
        {
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

            smtp.Connect(SmtpHost, SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(EmailAddress, EmailPassword);
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(this.EmailName, EmailAddress));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            smtp.Send(emailMessage);
            smtp.Disconnect(true);

        }
    }


    /*/// <summary>
    /// Отправка сообщения по электронной почте с прикреплёнными файлами
    /// </summary> 
    public void SendEmail(string email, string subject, string message,
                            ApplicationCommon.CommonTypes.TypeFile[] resources)
    {
        using (var smtp = new MailKit.Net.Smtp.SmtpClient())
        {
            smtp.ServerCertificateValidationCallback = (s, c, h, e) =>
            {
                return true;
            };

            smtp.Connect(smtpHost, smtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailAddress, emailPassword);
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(this.emailName, emailAddress));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            var builder = new BodyBuilder();

            builder.TextBody = message;
             if (resources != null)
            {
                foreach (ApplicationCommon.CommonTypes.TypeFile resource in resources)
                {
                    System.IO.File.WriteAllBytes(resource.Name, resource.Data);
                    builder.Attachments.Add(resource.Name);
                }
            } 
            emailMessage.Body = builder.ToMessageBody();

            smtp.Send(emailMessage);
            smtp.Disconnect(true);
        }
    }*/


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
            client.Authenticate(EmailAddress, this.EmailAddress);
            for (int i = 0; i < client.Count; i++)
            {
                MimeMessage message = client.GetMessage(i);
            }
            client.Disconnect(true);
        }
    }
}