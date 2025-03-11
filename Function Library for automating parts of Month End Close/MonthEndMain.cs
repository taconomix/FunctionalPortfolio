/*== Execute Month End Automation ============================================

    MonthEnd Library | Execute Month-End Functions | Email & Log Reports

    Project:   2406_MonthEnd
    Author:    Kevin Veldman
    Created:   2024/06/23
    Version:   1.0

        Request Parameters:
            none

        Response Parameters:
            none


    Purpose:
        Call Month End functions, email WIP Rec, Log any Issues


    NOTE:
        Some Code has been changed to remove Protected Information

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com

============================================================================*/


//
// Add Delegate for Logging Month-End Automation Errors
//
    string ErrorPath = $@"E:\EpicorData\Companies\EPI06\Log\Automation\MonthEnd\";

    Action<string> WriteError = ErrorMsg =>
    {
        string Err = $"{ErrorPath}{DateTime.Now:yyyyMMdd_HHmmss}.txt";
        
        var file = new System.IO.StreamWriter(Err);
        file.WriteLine(ErrorMsg);
        file.Close();
    };


//
// Run and Send WIP Rec Report
//
    try 
    {
        this.ThisLib.SendWipRec();
    }
    catch (Exception ex)
    {
        WriteError(ex.Message);
    }


//
// Update Earliest Apply Dates
//
    try 
    {
        this.ThisLib.UpdateEAD();
    }
    catch (Exception ex)
    {
        WriteError(ex.Message);
    }


//
// Close Previous Month
//
    try 
    {
        this.ThisLib.ClosePrevMonth();
    }
    catch (Exception ex)
    {
        WriteError(ex.Message);
    }





/*== CHANGE LOG ==============================================================


============================================================================*/