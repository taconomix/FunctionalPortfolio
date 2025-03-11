/*== Trigger Duplicate Job Alert==============================================

    Method Directive BPM | Post-Processing | Erp.BO.ReportQty.ReportQty()

    Project:   2308_ShopData
    Author:    Kevin Veldman
    Created:   2023/08/24
    Version:   1.2.0
    
    Info: 
        Trigger EFX to Send MSTeams Alert on scans: shipped jobs, possible dupe jobs

    NOTE:
        Some Code has been changed to remove Protected Information

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com
============================================================================*/


/*_ Widgets _____________________________________
    
    Condition 0:
        Directive has been enabled in the <stopOverscan> directive
    
    Custom Code 0:
        See below
_______________________________________________*/




/*==================================================================
    Widget: Custom Code 0
==================================================================*/


    //=========================================================
    //    Set varibles from Pre-Processing
    //=========================================================

        string jobNum = callContextBpmData.Character01 ?? "Null";
        string empID  = callContextBpmData.Character02 ?? "Null";

        decimal oprSeq    = callContextBpmData.Number01;
        decimal prodQty   = callContextBpmData.Number02;
        decimal scanQty   = callContextBpmData.Number03;
        decimal repullQty = callContextBpmData.Number04;
        decimal packNum   = callContextBpmData.Number05;

        string opDesc = Db.JobOper.First(x => x.JobNum==jobNum && x.OprSeq==oprSeq).OpDesc;
        string techName = (empID == "")? empID: Db.EmpBasic.FirstOrDefault( x => x.EmpID == empID ).Name;




    //=========================================================
    //    Send Teams alerts
    //=========================================================

        string EfxLib  = "EfxCaller";
        string EfxFunc = string.Empty;


        if ( packNum > 0 ) {

            System.DateTime? shipDate = callContextBpmData.Date01;

            EfxFunc = "SS-AlertShipJobScan";
            InvokeFunction( EfxLib, EfxFunc, jobNum, opDesc, techName, packNum, shipDate );  


        } else {

            decimal scanTime = callContextBpmData.Number20 - 3;
            decimal dupeTime = scanTime - 900;
            
            bool ignoreErr = Db.LaborDtl.Any( x => 
                                            x.JobNum==jobNum && 
                                            x.OprSeq==oprSeq && 
                                            x.CreateTime < scanTime && 
                                            x.CreateTime > dupeTime && 
                                            x.CreateDate == DateTime.Today );

            if ( ignoreErr ) 
                return;

            EfxFunc = "SS-AlertDupeJob";
            InvokeFunction( EfxLib, EfxFunc, jobNum, opDesc, prodQty, scanQty, repullQty, techName );

        }






    /*__ Reference ________________________________________________________

        
    _____________________________________________________________________*/