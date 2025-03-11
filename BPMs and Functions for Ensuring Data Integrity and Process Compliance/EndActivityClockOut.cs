/*== End Activity open LaborDtl & Clock out Employees ========================

    AutomationMES Library | End Activity & Clock Out | MES Automation

    Project:   2502_MESAutomation
    Author:    Kevin Veldman
    Created:   2025/02/24
    Version:   1.1.0

        Request Parameters: None
        Response Parameters: None


    Purpose:
        End Activity on all open LaborDtl Transactions
        Clock out all employees at end of day

    NOTE:
        Some Code has been changed to remove Protected Information

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com

============================================================================*/

//
// Retrieve Active Labor Transactions
//
    var ActiveLabor = Db.LaborDtl.Where( x => x.ActiveTrans ).Select( s => new {s.LaborHedSeq, s.EmployeeNum} );

    if ( ActiveLabor == null || ActiveLabor.Count() <= 0 ) return;



//
// Logging Delegate & Exclusions List
//
    Action<string> LogMsg = s => 
    {
        this.EfxLib.util_Common.SysTaskMsg( s, this.LibraryID );
    };


    var LeaveSignedIn = new List<string> { "0854" };




//
// Loop through active transactions and end activity & clock out
//
    foreach ( var row in ActiveLabor )
    {
        // Skip transactions for excluded employees
        if ( LeaveSignedIn.Contains( row.EmployeeNum ) )
        {
            continue;
        }


        try
        {
            // End Activity
            this.CallService<LaborSvcContract>( lbrSvc => {

                var laborDtl = lbrSvc.GetByID( (int)row.LaborHedSeq );
                string ldNum = "";
                
                foreach ( var dtl in laborDtl.LaborDtl)
                {

                    if (dtl.ActiveTrans)
                    {
                        ldNum += $"{dtl.LaborDtlSeq} ";
                        
                        dtl.RowMod="U";
                    
                        lbrSvc.EndActivity( ref laborDtl );
                        lbrSvc.Update( ref laborDtl );
                    }
                }
                
                LogMsg($"SUCCESS: LaborHed-{row.LaborHedSeq}\n{ldNum}");
            });
        }
        catch ( Exception ex )
        {
            // Log errors in System Monitor TaskLog
            LogMsg( $"ERROR: LaborHed-{row.LaborHedSeq} | EmpID-{row.EmployeeNum}" );
            LogMsg( ex.Message );
        }
    }