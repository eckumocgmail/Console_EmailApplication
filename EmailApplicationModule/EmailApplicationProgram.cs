using MailKit.Security;

using MimeKit;

using System;
using System.IO;
using System.Net;
using System.Net.Mail;

internal class EmailApplicationProgram
{
    internal static void Start(string[] args)
    {        
        var agent = new MailRuService("2LDtctEezQuQyxtcaasv");
        agent.Clear();
        agent.Send(
            "kba-2018@mail.ru", 
            "files", 
            "eckumoc@gmail.com", 
            Directory.GetFiles(@"D:\Projects-Modules\App\Program")
        );
        agent.Recieve();
    }
}
 