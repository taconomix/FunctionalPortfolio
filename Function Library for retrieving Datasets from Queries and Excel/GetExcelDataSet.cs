/*== Get DataSet from MS Excel File ========================================

    QueryUtility Library | GetExcelDataSet | Convert Excel to DataSet


    Project:   2401_QueryLib
    Author:    Kevin Veldman
    Created:   2024/01/18
    Version:   1.0.1


        Request: (System.String) XLFile (full file path)
        Response:  (System.DataSet) Results


    Purpose: 
       Return all data from a Microsoft Excel file as a System.Data.DataSet
       Each Worksheet in file will be a System.Data.DataTable in the DataSet

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com
===========================================================================*/



using ( var stream = File.Open(XLFile, FileMode.Open, FileAccess.Read) )
{
    IExcelDataReader reader;

    Result = ExcelReaderFactory.CreateReader(stream).AsDataSet();
}


/* 
    Assemblies Added: 
        ExcelDataReader.dll
        ExcelDataReader.DataSet.dll

    Both assemblies are free and open source under the MIT License
    https://github.com/ExcelDataReader/ExcelDataReader

    Added usings:
        using ExcelDataReader;
        using System.IO;
*/