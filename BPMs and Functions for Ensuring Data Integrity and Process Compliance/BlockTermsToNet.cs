/*== Block removal of CC Terms ==============================================

    SalesOrder | MasterUpdate() | Pre-Processing

    Project:    Zendesk Ticket #45561
    Author:     Kevin Veldman
    Created:    2024/09/25
    Version:    1.0
    
    Purpose: 
        Restrict any change of OrderHed.TermsCode from CC or Rcpt to Net
        Restriction ignored for Web Users, Security Managers, SecGroup="AC"

    NOTE:
        Some Code has been changed to remove Protected Information

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com

===========================================================================*/

//
// Exit if no updated order or if user is WebUser
//
    var oh = ds.OrderHed.Where( x => x.Updated() ).FirstOrDefault();

    if ( oh == null ) return;


    var user = callContextClient.CurrentUserId; 
    var SkipUsers = new List<string>{ };

    if ( SkipUsers.Contains(user.ToLower()) ) return;



//
// If terms were changed from CC or Rcpt to any other, check user
//
    var CCTermsList = new List<string>{"CC","Rcpt"};

    if ( CCTermsList.Contains(oh.TermsCode) ) return;

    bool isRestricted = ds.OrderHed.Any( y => y.Unchanged() && 
                                            y.SysRowID == oh.SysRowID && 
                                            y.TermsCode != oh.TermsCode && 
                                            CCTermsList.Contains(y.TermsCode) );


    if ( !isRestricted ) return;



//
// Throw an error if user is not in Security Group "AC" or a Security Manager
//
    var info = "Changing the Terms for this order is restricted to Security Group AC. ";
    var head = "Restricted Terms Change";

    var isCustAdmin = Db.UserFile.Any(x => x.DcdUserID == user && (x.GroupList.Contains("AC") || x.SecurityMgr) );


    if ( isCustAdmin ) 
    {
        info += "You are a member of Security Group AC, Save Allowed.";
        this.PublishInfoMessage(info, Ice.Common.BusinessObjectMessageType.Information, Ice.Bpm.InfoMessageDisplayMode.Individual, head, head);
    } 
    else 
    {
        info += "Please contact your Epicor administrator if you need assistance.";
        throw new Ice.BLException( info );
    }






/*== CHANGE LOG ==============================================================

    2024/09/25:
        +Initial Deployment

============================================================================*/