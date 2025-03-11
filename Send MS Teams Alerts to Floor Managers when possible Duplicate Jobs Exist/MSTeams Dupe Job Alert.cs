/*== Send Teams Alerts when Possible Dupe Jobs Exist =========================

    ShopAlerts Library | DupeJobAlert | Send MS Teams Alert

    Project:   2308_ShopData
    Author:    Kevin Veldman
    Created:   2023/08/24
    Version:   1.0.1

        Request Parameters:
            System.String    - jobNum
            System.String    - opDesc
            System.String    - techinician
            System.Decimal   - packNum
            System.DateTime? - shipDate

        Response Parameters:

    Purpose:
        Trigger by BPM when RptQty occurs on Same Job Operation
        Send Alert to Teams Channel

    NOTE:
        Some Code has been changed to remove Protected Information

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com

==========================================================================*/

    /*==============================================================
        Prep REST Request
    ==============================================================*/

    var webhookURL = (string)this.EfxLib.util_Common.getUDCLongDesc("KVE","SHOPHOOK");

    var title = "Duplicate Job Alert";
    var themeColor = "C51F30";
    var alertIcon = (string)this.EfxLib.util_Common.getUDCLongDesc("KVE","ALERTIMAGE");
    int responseStatusCodeTeams = 0;

    var client = new RestClient(webhookURL);




    /*==============================================================
        Set content for REST Request
    ==============================================================*/

    var data = new {

        context = "https://schema.org/extensions",
        type = "MessageCard",
        summary = "Incoming Alert Message!",
        themeColor = themeColor,

        markdown = true,
        sections = new [] {
            
            new {

                activityTitle = title,
                activitySubtitle = "Recent scan indicates possible duplicate job in fabrication",
                activityImage = alertIcon,
                
                facts = new [] {

                    new { name = "Job Number"   , value = jobNum                     },
                    new { name = "Techinician"  , value = techName                   },
                    new { name = "Operation"    , value = opDesc                     },
                    new { name = "Prod Quantity", value = $"{prodQty:#.00}"          },
                    new { name = "Total Scanned", value = $"{scanQty:#.00}"          },
                    new { name = "Repulls"      , value = (repullQty>0? "Yes": "No") }
                }
            }
        }
    };
        
    var content = JsonConvert.SerializeObject(data);
    

    /*==============================================================
        Create REST Request
    ==============================================================*/

    var request = new RestRequest(Method.POST);
        request.AddHeader("Accept", "application/json");
        request.AddJsonBody(content);
     
    var response = client.Execute(request);
        responseStatusCodeTeams = (int)response.StatusCode;






    /*__ Added Usings ____________________________________________________

        using RestSharp;
        using Newtonsoft.Json;
    ____________________________________________________________________*/