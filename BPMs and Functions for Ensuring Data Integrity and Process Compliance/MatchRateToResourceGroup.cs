/*== Update Employee Labor Rates to Resource Group Rate ======================

    SyncEmployee Library | Match Employee to Resource Group Rate | BPM Triggered

    Project:   2412_SyncEmpRates
    Author:    Kevin Veldman
    Created:   2024/12/06
    Version:   1.0

        Request Parameters:
            System.String  - GroupID (Resource Group ID)
            System.Decimal - NewRate (Updated Group Labor Rate)

        Response Parameters:
            none


    Purpose:
        Update Employee Labor Rates to match Resource Group Labor Rate
        Triggered by Standard Data Directive on ResourceGroup Table
    

    NOTE:
        Some Code has been changed to remove Protected Information

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com

============================================================================*/

//
// Get list of Employees tied to Resource Group
//
    var EmpIdList = Db.EmpBasic.Where( x => x.Company == Session.CompanyID 
                                         && x.ResourceGrpID == GroupID )
                               .Select( x => x.EmpID )
                               .ToList();


//
// Update Labor Rate for all Employees
//
    CallService<EmpBasicSvcContract>( svc => {

        foreach ( string id in EmpIdList )
        {
            var tsEmpBasic = svc.GetByID(id);
            var emp = tsEmpBasic.EmpBasic.FirstOrDefault();
            
            emp.LaborRate = NewRate;
            emp.RowMod = "U";
            
            svc.Update( ref tsEmpBasic );
        }
    });




/*== CHANGE LOG ==============================================================


============================================================================*/


    /*__ Reference _______________________________________________________
        
        using Erp.Contracts;

        ref: Services -> ERP.BO.EmpBasic
        ref: Tables -> ERP.EmpBasic

    ____________________________________________________________________*/