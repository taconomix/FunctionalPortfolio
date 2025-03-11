/*== Email WIP Rec Report to Finance =========================================

    MonthEnd Library | Email WIP Rec Report

    Project:   2406_MonthEnd
    Author:    Kevin Veldman
    Created:   2024/06/23
    Version:   1.0

        Request Parameters:
            none

        Response Parameters:
            none


    Purpose:
        Call Function to Generate Unposted WIP Rec, Email report to Finance

    NOTE:
        Some Code has been changed to remove Protected Information

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com
============================================================================*/


string WipRecReport = this.EfxLib.AutoReports.GenWipRec();


string sendTo = "sendToAddress@Company.com";
string sendCC = "rfptSolutions@gmail.com";
string subj = "Wip Rec Rpt";

bool FileExists = WipRecReport != "" && System.IO.File.Exists(WipRecReport);

string body = FileExists? "Wip Reconciliation Report Attached": "No Unposted WIP Found, No Report Generated";

this.EfxLib.util_Email.IceMailer("Company", sendTo, "", sendCC, subj, body, false, WipRecReport);




/*== CHANGE LOG ==============================================================


============================================================================*/