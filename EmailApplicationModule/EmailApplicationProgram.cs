using System;

 
internal class EmailApplicationProgram
{        
    internal static void Start(string[] args)
    {

 
        var email = new EmailService( );
        email.Recieve();
    }
}
 