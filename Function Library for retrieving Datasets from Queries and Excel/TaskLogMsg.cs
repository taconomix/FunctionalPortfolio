/*== Add Function Task Log Message ==========================================

    QueryUtility Library | TaskLogMsg | Log Message to Task Monitor Log


    Project:   2401_QueryLib
    Author:    Kevin Veldman
    Created:   2024/01/18
    Version:   1.0.0


    Request: (System.String) Message

    Response: none


    Purpose: 
       Add Log Message to Function Task in System Monitor Task Log

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com
===========================================================================*/

var ActiveEFxTasks = Db.SysTask.Where( t => 
                                    t.Company == Session.CompanyID && 
                                    t.TaskDescription.ToLower() == "run epicor function" && 
                                    t.TaskStatus.ToLower() == "active");


foreach ( var task in ActiveEFxTasks) 
{

    var log = Db.SysTaskLog.Where( t => 
                                t.SysTaskNum == task.SysTaskNum && 
                                t.MsgText.ToLower().Contains(this.LibraryID.ToLower()) )
                            .FirstOrDefault();


    if ( log == null )
        continue;


    this.CallService<Ice.Contracts.SysMonitorTasksSvcContract>( sm=> 
    {     
        sm.WriteToTaskLog( Message, log.SysTaskNum, Epicor.ServiceModel.Utilities.MsgType.Info );
    });
}