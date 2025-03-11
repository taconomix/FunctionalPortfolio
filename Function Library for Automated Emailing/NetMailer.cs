/*== Send Emails using System.Net.Mail =======================================

    EmailUtility Library | IceMailer | Send Email via System.Net.Mailer

    Project:   2307_EmailAutomation
    Author:    Kevin Veldman
    Created:   2024/01/11
    Version:   1.1.0


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
    
    var files  = ((string[])Data["AttachFiles"]).ToList();
    var CCList = ((string[])Data["CCList"]).ToList();
    var BCList = ((string[])Data["BCList"]).ToList();



//
// Helper delegate for attachments
//
    Func<string,Attachment> AttachmentData = s =>
    {
        var ThisFile = this.ThisLib.getAttachment( s );
        var data = new Attachment( ThisFile.Path );

        if ( ThisFile.IsInline )
        {
            data.ContentDisposition.Inline = true;
            data.ContentId = ThisFile.ContentID;
        }

        return data;
    };



//
// Split email and display name if present
//
    string sendName = sendFrom;

    bool hasDisplayName = ( sendFrom.Contains("<") && sendFrom.Contains(">") );

    if ( hasDisplayName ) 
    { 
        int Start = sendFrom.IndexOf("<", 0) + 1;
        sendName = sendFrom.Substring(0, Start - 2);
        sendFrom = sendFrom.Substring(Start, (sendFrom.Length-Start-1));
    }



//
// Construct Email Message
//
    using ( var message = new MailMessage() ) 
    {

        message.From = new MailAddress( sendFrom, sendName );
        message.To.Add( new MailAddress (sendTo) );


        foreach ( string email in CCList ) 
        {
            message.CC.Add( new MailAddress (email.Trim()) );
        }

        foreach ( string email in BCList ) 
        {
            message.Bcc.Add( new MailAddress (email.Trim()) );
        }


        message.Subject = (string)Data["Subj"];
        message.IsBodyHtml = (bool)Data["HTML"];
        message.Body = (string)Data["Body"];


        foreach ( string file in files )
        {
            message.Attachments.Add( AttachmentData (file) );
        }



        using ( var smtp = new SmtpClient( "smtp.office365.com", 587 ) ) 
        {
            smtp.EnableSsl = true;

            try 
            {
                var creds = new Dictionary<string,string>() 
                {

                };

                smtp.Credentials = new NetworkCredential( sendFrom, creds[sendFrom] );
                smtp.Send( message );

            } 
            catch ( Exception ex ) 
            {
                string logName = Path.Combine("C:","EpicorData","Logs","smtpErrors.log");
                string logDtls = $"\n{DateTime.Now:G}:\n{Data["AttachFiles"]}\n\n{ex.Message}\n";

                File.AppendAllText( logName, logDtls );
            }
        }
    }





    /*__ Reference _______________________________________________________

        // Added Usings 

            using System.Net;
            using System.Net.Mail;
            using System.IO;
            using Newtonsoft.Json;
            
    ____________________________________________________________________*/
