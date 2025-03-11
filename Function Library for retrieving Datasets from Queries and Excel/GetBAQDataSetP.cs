/*== Get DataSet from Epicor BAQ with BAQ Params ===========================

    QueryUtility Library | GetBAQDataSetP | Convert BAQ Results to DataSet


    Project:   2401_QueryLib
    Author:    Kevin Veldman
    Created:   2024/01/18
    Version:   2.1.1


        Request: 
            (System.String) QueryName (BAQ ID)
            (System.String) InputVals // '~'-separated values

        Response: 
            (System.Data.DataSet) Results


    Purpose: 
       Call any saved BAQ and return the results as a System.Data.DataSet
       Can take any number of parameters as a '~'-separated string

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com

===========================================================================*/

CallService<DynamicQuerySvcContract>( svc => 
{

    var dsQuery = svc.GetByID(QueryName);

    if ( dsQuery == null ) 
        return;


    string[] inputParams = InputVals.Split('~');
    
    var dsBAQ = svc.GetQueryExecutionParameters(dsQuery);
    int i = 0;

    foreach ( var parm in dsBAQ.ExecutionParameter ) 
    {

        bool Overload = ( i >= inputParams.Length );

        if ( Overload ) 
        {
            parm.IsEmpty        = true;
            parm.ParameterValue = "";
        } 
        else 
        {
            parm.IsEmpty        = inputParams[i].Length == 0;
            parm.ParameterValue = inputParams[i];
        }

        i++;
    }

    Results = svc.Execute(dsQuery, dsBAQ);

});