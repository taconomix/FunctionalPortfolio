/*== Export BAQ (no parameters) to CSV or XML ================================

    QueryUtility Library | BAQExport | Export BAQ to CSV


    Project:   2401_QueryLib
    Author:    Kevin Veldman
    Created:   2024/01/18
    Version:   1.0.0

    Request:
        (System.String) QueryID
        (System.String) ExportPath
        (System.String) TaskNote

    Response: none
    

    Purpose: 
       Programatically Export BAQ to CSV or XML using BAQ Export Process

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com
===========================================================================*/



CallService<Ice.Contracts.DynamicQueryExportSvcContract>( svc => 
{
    var dqeTS = svc.GetNewParameters();

    var row = dqeTS.DynQueryExpParam[0];

    row.QueryID = QueryID;

    row.ExportFilename = ExportPath; 
    row.ExportFormat = "CSV"; // or "XML";

    row.OutputLabels = true;

    row.TaskNote = TaskNote; // Note in System Monitor Task Log

    svc.SubmitToAgent( dqeTS, "", 0, 0, "SystemAgent" );
    
} );


/* Export Path Examples:

    @"\\ServerName\EpicorData\Companies\CO\Log\Payroll\Summary_20240112.csv"; 
        // "C:\EpicorData\... throws error
    
    "My_BAQ.xml"; 
        // Saves in function call user's \Company\Processes\Username\ folder
*/