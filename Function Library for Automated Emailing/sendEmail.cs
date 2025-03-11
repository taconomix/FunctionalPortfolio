/*== Send Emails w Requested Parameters =====================================

    EmailUtility Library | SendEmail | Create email in JSON for SMTP Mailing

    Project:   2307_EmailAutomation
    Author:    Kevin Veldman
    Created:   2023/07/06
    Version:   3.0.0


        Request Parameters:
            sendFrom    - System.String (see references)
            sendTo      - System.String
            sendCC      - System.String (see references)
            sendBCC     - System.String
            Subject     - System.String
            Body        - System.String
            isHTML      - System.Bool  
            Attachments - System.String (see references)

        Response Parameters:


    NOTE:
        Some Code has been changed to remove Protected Information

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com
===========================================================================*/



//
// Enumerate real values from delimited string
//
    Func<string,char,List<string>> getRealValues = (s, c) =>
        (string.IsNullOrEmpty(s)?c.ToString():s).Split(c).Where( x => x.Trim().Length > 0 ).ToList();



//
// Handle Attachments
//
    var enumAttachments = getRealValues( Attachments, '~' );

    var files = enumAttachments.Where( x => x.Contains(@":\") || x.Contains(@"\\") );
    var names = enumAttachments.Where( x => files.All( f => !f.Contains(x) ) );

    var matchFileNames = names.Count() == files.Count();



//
// Serialize Email Data to JSON String
//
    var Email = new {

      SendTo   = sendTo.Replace(",",""),
      SendFrom = sendFrom,
      
      CCString = sendCC ?? string.Empty,
      CCList   = getRealValues( sendCC, ';' ),
      
      BCString = sendBCC ?? string.Empty,
      BCList   = getRealValues( sendBCC, ';' ),

      AttachFiles = files,
      AttachNames = names,

      Subj = Subject,
      Body = Body,
      HTML = isHTML

    };


    var JsonEmail = JsonConvert.SerializeObject(Email);



//
// Send Email ( Use Ice.Mail.SmtpMailer when possible )
//
    bool IsCompanyDefault = sendFrom == "default@company.net";
    bool InlineAttachment = files.Any( x => x.Contains("?") );

    bool UseIceMailer = IsCompanyDefualt && !InlineAttachment;

    try 
    {
        if ( UseIceMailer )
        {
            this.ThisLib.IceMailer( JsonEmail );
        }
        else
        {
            this.ThisLib.NetMailer( JsonEmail );
        }

    }
    catch ( Exception ex )
    {
        string logName = Path.Combine( "C:","EpicorData","Logs","smtpErrors.log" );
        string logDtls = $"\n{DateTime.Now:G}:\n{Attachments}\n\n{ex.Message}\n";
        
        System.IO.File.AppendAllText( logName, logDtls );
    }






    /*__ Reference _______________________________________________________


        // Added Usings 

            using System.Net;
            using System.Net.Mail;
            using System.IO;
            using Newtonsoft.Json;



        // Parameter Formatting 
            
            sendTo may be "Display Name <email@domain.com>" or just email

            sendCC/BCC may include multiple emails in a string (delimiter = ;)

            Ice.Mail.SmtpMailer: 
                
                Uses Company default email ( noreply@company.net )
                Attachments Example:
                    "C:\FilePath.txt~Name.txt"

                Files / Names must be in same order
            

            System.Net.Mail.SmtpClient:

                Works with any domain email
                Attachments Example:
                    "C:\filepath.csv~\\Server\filepath.txt"


    ____________________________________________________________________*/