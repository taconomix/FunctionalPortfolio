/*== Send Emails using Ice.Mail.SmtpMailer ===================================
    
    EmailUtility Library | IceMailer | Send Email via Ice.SMTPMailer

    Project:   2307_EmailAutomation
    Author:    Kevin Veldman
    Created:   2024/01/11
    Version:   1.0.0


        Request Parameters: System.String - jsonEmail
        Response Parameters:


    Purpose:
        Format alert data to HTML for Email Alerts content
    

    NOTE:
        Some Code has been changed to remove Protected Information

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com
===========================================================================*/


//
// Parse Email Data from Json Object
//
    string dsTemplate = $"{{\"Data\": [{jsonEmail}]}}";

    var Email = JsonConvert.DeserializeObject<DataSet>(dsTemplate);

    DataRow Data = Email.Tables["Data"].Rows[0];


    var sendFrom = (string)Data["SendFrom"];
    var sendTo   = (string)Data["SendTo"  ];
    var files    = (List<string>)Data["AttachFiles"];
    var names    = (List<string>)Data["AttachNames"];




using ( var mailer = new Ice.Mail.SmtpMailer(this.Session) ) 
{
    var message = new Ice.Mail.SmtpMail();

    message.SetFrom( $"{Data["SendFrom"]}" );
    message.SetTo  ( $"{Data["SendTo"  ]}" );

    message.SetCC ( $"{Data["CCString"]}" );
    message.SetBcc( $"{Data["BCString"]}" );

    message.SetSubject($"{Data["Subj"]}");
    message.SetBody($"{Data["Body"]}");
    message.IsBodyHtml = (bool)Data["HTML"]; 


    var attachments = new Dictionary <string,byte[]>();
    bool MatchNames = files.Count() == names.Count();


    try 
    {
        for ( int i = 0; i < files.Count(); i++ )
        {
            var AttachmentData = this.ThisLib.getAttachment( files[i] );
            string fileName  = MatchNames? names[i]: Path.GetFileName( AttachmentData.Path );
            byte[] fileBytes = File.ReadAllBytes( AttachmentData.Path );

            attachments.Add( fileName, fileBytes );
        }

        mailer.Send( message, attachments );

    } 
    catch ( Exception ex ) 
    {
        string logName = Path.Combine( "C:","EpicorData","Logs","smtpErrors.log" );
        string logDtls = $"\n{DateTime.Now:G}:\n{files}\n{names}\n\n{ex.Message}\n";

        System.IO.File.AppendAllText( logName, logDtls );
    }
}






/*== CHANGE LOG ==============================================================



============================================================================*/




    /*__ Reference _______________________________________________________

        // Added Usings 

            using System.Net;
            using System.Net.Mail;
            using System.IO;
            using Newtonsoft.Json;

    ____________________________________________________________________*/
