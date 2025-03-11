/*== Get DataSet from Epicor BAQ ===========================================

    QueryUtility Library | GetBAQDataSet | Convert BAQ Results to DataSet

    Project:   2401_QueryLib
    Author:    Kevin Veldman
    Created:   2024/01/18
    Version:   1.2.0


        Request: (System.String) queryName (BAQ Name)
        Response: (System.Data.DataSet) Results


    Purpose: 
       Call any saved BAQ and return the results as a System.Data.DataSet
   
    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com
===========================================================================*/


CallService<DynamicQuerySvcContract>( svc => 
{

    var dsQuery = svc.GetByID( queryName );  // BAQ Name

    if ( dsQuery == null ) 
        return;


    var p = svc.GetQueryExecutionParameters( dsQuery );
    Results = svc.Execute( dsQuery, p );
});