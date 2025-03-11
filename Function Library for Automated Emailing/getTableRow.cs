/*== Build DataRow for Alert Data  ==========================================


    EmailUtility Library | getTableRow | Build DataRow for Email Alert

    Project:   2307_EmailAutomation
    Author:    Kevin Veldman
    Created:   2023/07/08
    Version:   1.0.1


        Request Parameters: 
        	rowStyle - String, HTML Style for this row ('style="prop: value;"')
			dataVals - String, Row Values separated by '~'

        Response Parameters: 
        	dataRow - String, HTML Formatted Data Row


    Purpose:
        Set up HTML DataRow from Data Values and HTML Style Attribute
    

    NOTE:
        Some Code has been changed to remove Protected Information

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com
===========================================================================*/


	string[] dataVals = dataVal.Split('~');

	var sbData = new System.Text.StringBuilder($"<tr class=''{rowStyle}>");

	foreach ( string val in dataVals ) 
		sbData.Append($"<td class='text-11px'>{val}&nbsp;</td>");


	// Output
	dataRow = sbData.Append($"</tr>").ToString();




/*== CHANGE LOG ==============================================================


============================================================================*/


 	/*__ Reference _______________________________________________________

	____________________________________________________________________*/