/*== Automatically Close Financial Period at Month-End =======================

    MonthEnd Library | Close Financial Period | Auto-Fill Parameters

    Project:   2406_MonthEnd
    Author:    Kevin Veldman
    Created:   2024/06/23
    Version:   1.0

        Request Parameters:
            none

        Response Parameters:
            none


    Purpose:
        On the first of every month, close previous month Fiscal Period


    NOTE:
        Some Code has been changed to remove Protected Information

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com

============================================================================*/

//
// Call ClosePeriodSvc Business Object to close previous Period
// 
    var dt = DateTime.Today.AddMonths(-1);

    int month = dt.Month;
    int year = dt.Year;

    CallService<Erp.Contracts.ClosePeriodSvcContract>( svc => {

        var periodTS = svc.GetByID("MAIN", "MAIN", year, "", month);
        
        if (periodTS == null) return;
        
        var pdRow = periodTS.GLBookPer.FirstOrDefault();
        
        if (pdRow == null) return;
        
        pdRow.ClosedPeriod = true;
        pdRow.RowMod = "U";
        
        svc.Update( ref periodTS );
    });





/*== CHANGE LOG ==============================================================


============================================================================*/