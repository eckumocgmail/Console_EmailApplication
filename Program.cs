using AngleSharp.Dom.Events;

using System;
using System.Collections.Generic;
using System.IO;

internal class Program
{
    public static void Main(string[] args)
    {
 
        var agent = new MailRuService("2LDtctEezQuQyxtcaasv");
        /*agent.Clear();
        agent.Send(
            "kba-2018@mail.ru",
            "files",
            "eckumoc@gmail.com",
            Directory.GetFiles(@"D:\Projects-Modules\App\Program")
        );
        agent.Recieve();*/
        switch (InputApplicationProgram.SingleSelect("Электронная почта", new List<string>() {
            "Написать",
            "Входящие",
            "Выход"
        }, ref args))
        {
            case "Написать":
                agent.Send(
                    InputApplicationProgram.InputEmail("Адрес", null, ref args),
                    InputApplicationProgram.InputText("Тема", null, ref args),
                    InputApplicationProgram.InputText("Сообщение", null, ref args)
                );
                Main(args);
                break;
            case "Входящие":
                OnMessageSelected(agent.Recieve().UserSelectSingle(message => message.ToString(), ref args),ref args);
                break;          
            case "Выход":
                break;
            default: throw new ArgumentException();
        }
    }

    private static void OnMessageSelected(MailRuService.MessageModel messageModel, ref string[] args)
    {
        Console.Clear();
        messageModel.ToJsonOnScreen().WriteToConsole();
        switch (InputApplicationProgram.SingleSelect(messageModel.ToString(), new List<string>() {
            "Ответить",            
            "Назад"
        }, ref args))
        {
            case "Ответить":
                var agent = new MailRuService("2LDtctEezQuQyxtcaasv");
                agent.Send(
                    messageModel.Sender,
                    messageModel.Subject,
                    InputApplicationProgram.InputText("Сообщение", null, ref args)
                );
                Main(args);
                break;       
            case "Назад":
                Main(args);
                break;
            default: throw new ArgumentException();

        }
}
 