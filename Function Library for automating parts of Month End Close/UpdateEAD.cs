/*== Automatically Update Earliest Apply for GJ ==============================

    MonthEnd Library | Update Earliest Apply Date | General Journal Only

    Project:   2406_MonthEnd
    Author:    Kevin Veldman
    Created:   2024/06/23
    Version:   1.0

        Request Parameters:
            none

        Response Parameters:
            none


    Purpose:
        Update Earliest Apply Date in General Journal at Month-End
    

    NOTE:
        Some Code has been changed to remove Protected Information

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com

============================================================================*/

//
// Call EAD Business Object to update Earliest Apply Date on General Journal
// 
    CallService<Erp.Contracts.EADSvcContract>( svc => {

      var eadTS = svc.GetByID();
      
      var eadTypeRows = eadTS.EADType.Where( x => x.EADType != "GJ" );
      
      foreach ( var row in eadTypeRows )
      {
          row.EarliestApplyDate = DateTime.Today;
          row.RowMod = "U";
      }
      

      svc.Update( ref eadTS );
        
    });




/*== CHANGE LOG ==============================================================


============================================================================*/