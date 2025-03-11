/*== Get HTML Signature Block for Email ======================================

    EmailUtility Library | getSignature | Get HTML Format Signature Block

    Project:   2307_EmailAutomation
    Author:    Kevin Veldman
    Created:   2023/08/25
    Version:   2.0.0


        Request Parameters: sigType - System.String

        Response Parameters: sigBlock - System.String


    Purpose:
        Retrieve HTML Signature Block for Automated External Emails
    

    NOTE:
        Some Code has been changed to remove Protected Information

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com
===========================================================================*/

string udCodePrefix = "SIG_";

switch ( sigType ) {

    case "AR": udCodePrefix += "AR";
        break;

    case "DEV": udCodePrefix += "IT";
        break;

    case "CSR": udCodePrefix += "CS";
        break;

    default: udCodePrefix += "CS"; 
        break;
}


Func<string,string> getSigLine = s =>
    this.EfxLib.util_Common.getUDCLongDesc( "EMAIL", s );


string sigName = getSigLine( udCodePrefix + "NAME" );
string sigDept = getSigLine( udCodePrefix + "DEPT" );
string sigMail = getSigLine( udCodePrefix + "MAIL" );
string sigCall = getSigLine( "SIG_PHONE"  );
string noReply = getSigLine( "SIG_NORPLY" );
string noRply2 = getSigLine( "SIG_NOREP2" );



var sig = new System.Text.StringBuilder("<p>Sincerely,</p>");

    sig.Append( $@"
        <p style='color: #4c5966;>
            <b>{sigName}</b>
            <br>{sigDept}
            <br>{sigCall}
            <br><a href='mailto:{sigMail}'>{sigMail}</a>
        </p>" );

    sig.Append($@"
        <p style='color: #91A3B0;'>
            <em><b>{noReply} </b><br>
            {noRply2}</em>
        </p>");



sigBlock = sig.ToString();

