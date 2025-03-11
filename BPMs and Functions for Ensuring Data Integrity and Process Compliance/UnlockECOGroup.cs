/*== Unlock an ECO Group =====================================================

    Epicor Functions | Custom Code | 2025/01/28 

    Parameters:
        Request:   (string) GroupID
        Response:  (string) ReturnMsg

    References:
        Table:     ECOGroup
        Services:  Erp.BO.EngWorkBench


    Purpose:
        Unlock an ECO Group that is locked and in Single User mode

    NOTE:
        Some Code has been changed to remove Protected Information

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com

============================================================================*/


//
// Confirm Group Exists
//
    bool GroupExists = Db.ECOGroup.Any( x=> x.GroupID == ECOGroup );

    if (!GroupExists)
    {
        ReturnMsg = $"ECO Group {ECOGroup} does not exist.";
        return;
    }


//
// Unlock Group using EngWorkBench Service
//
    CallService<Erp.Contracts.EngWorkBenchSvcContract>( svc => {
        
        var ts = svc.GetByID( ECOGroup );
        
            string ipGroupID = ECOGroup;
            string ipPartNum = "";
            string ipRevisionNum = "";
            string ipAltMethod = "";
            string ipProcessMfgID = "";
            DateTime? ipAsOfDate = DateTime.Today;
            bool ipCompleteTree = false;
            bool ipValidPassword = false;
            bool ipReturn = true;
            bool ipGetDatasetForTree = false;
            bool ipUseMethodForParts = false;
            string ipAuditText = "ECO Group Unlock Function";
            string opMessage = "";
            string opResultString = "";
        
        
        svc.GroupUnLock(ipGroupID, ipPartNum, ipRevisionNum, ipAltMethod, ipProcessMfgID, ipAsOfDate, ipCompleteTree, ipReturn, ipGetDatasetForTree, ipUseMethodForParts, ref ts  );
    });


//
// Confirm Successfully Unlocked
//
    bool Unlocked = Db.ECOGroup.Any( x => x.GroupID == ECOGroup && !x.GrpLocked );

    if (Unlocked)
    {
        ReturnMsg = $"ECO Group {ECOGroup} is now unlocked.";
    }
    else
    {
        ReturnMsg = $"Something went wrong.\n\nThe 'GroupUnLock' Method was executed successfully, but the group remains locked.\n\nPlease confirm no one is actively working with the group in Engineering Workbench.";
    }

