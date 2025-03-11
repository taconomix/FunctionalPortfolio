/*== Format AlertTable for HTML EMail ========================================

	EmailUtility Library | getAlertTable | Get HTML Format Table for Data

    Project:   2307_EmailAutomation
    Author:    Kevin Veldman
    Created:   2023/07/20
    Version:   1.0.4


		Request Parameters: See Reference Section
			Headers   - System.String
			ColNames  - System.String
			AlertRows - System.String 
			Colors    - System.String
			
		Response Parameters:
			EmailBody - System.String


    Purpose:
        Format alert data to HTML for Email Alerts content
    

    NOTE:
        Some Code has been changed to remove Protected Information

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com
===========================================================================*/


// Alert Information ________________________________________________
	
	string[] _headers = Headers.Split('~');
	string AlertTitle = _headers[0];
	string RtHeader = _headers[1];
	string TableName = _headers[2];
	string FootNote = _headers[3];



// Column Headers ___________________________________________________

	string Columns = string.Empty;
	foreach ( string col in ColNames.Split('~') )
		Columns += $"<th style='vertical-align: top;'>{col}</th>";



// Color Schemes _____________________________________________________

	// Brand Colors ("SS")
	string ssRed = "#C51F30"; 
	string ssYel = "#FFCF01"; 
	string ssGrn = "#9CB92D"; 
	string ssGry = "#4C5966"; 
	string ssBlu = "#66BECB"; 

	// Alt Colors ("ALT")
	string altRed = "#950095";
	string altYel = "#FFA624";
	string altGrn = "#00FF00";
	string altGry = "#808080";
	string altBlu = "#2680FF";

	// Set colors for Email (Default to Brand Colors)
	string TopColorMain = ssGry;
	string TopColorRight = ssYel;
	string TitleColor = ssRed;
	string FooterColor = ssBlu;
	string FooterFont = "#000000";

	// Set Company1 Colors
	if ( Colors.ToLower() == "co1" ) {
		TopColorMain = ssGry;
		TopColorRight = ssYel;
		TitleColor = ssRed;
		FooterColor = ssBlu;
		FooterFont = "#000000";
	}

	// Set Company2 Colors
	if ( Colors.ToLower() == "co2" ) {
		TopColorMain = "#090B3B";
		TopColorRight = "#FA9623";
		TitleColor = "#0B0F61";
		FooterColor = "#4C597D";
		FooterFont = "#FFFFFF";
	}



// Create HTML Email Content ________________________________________

	var sbEmail = new System.Text.StringBuilder();
	sbEmail.Append($@"
		<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional //EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
		<html>
			<head>
				<title></title>
				<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
			</head>
			<body style='background-color: #FFFFFF; padding: 0; font-family: Arial, sans-serif; width: 100%; !important;'>
				<table border='0' cellpadding='3' cellspacing='0' width='100%' style='max-width: 1240px;'>
					<tr style='background-color: {TopColorMain};'>
						<td style='text-align: left; color: #FFFFFF; font-size: 24px;'><b>{AlertTitle}</b></td>
						<td style='text-align: right; color: {TopColorRight}; font-size: 20px;'><b>{RtHeader}</b></td>
					</tr>
					<tr style='background-color: #E7E7E8; height: 16px;'><td></td><td></td></tr>
					<tr style='background-color: #E7E7E8;'>
						<td colspan='2'>
							<h2 style='color: {TitleColor}; font-size: 20px; padding-left: 16px;'><b>{TableName}</b></h2>
							<table cellpadding='10' cellspacing='0' border='1' style='margin-left: 16; margin-right: 16;'>
								<thead><tr style='cellpadding: 10; border-collapse: collapse;'>{Columns}</tr></thead>
								<tbody>{AlertRows}</tbody>
							</table>
						</td>
					</tr>
					<tr style='background-color: #E7E7E8; height: 12px;'><td></td><td></td></tr>
					<tr style='background-color: {FooterColor}; height: 24px;'>
						<td style='color: {FooterFont};'><b>{FootNote}</b></td><td></td>
					</tr>
				</table>
			</body>
		</html>");



// Set Response Value _______________________________________________
	EmailBody = sbEmail.ToString();




	/*__ Reference _______________________________________________________

		// Parameter Formatting 
					
			Headers:   Email Headers & Footnote, '~'-separated
			ColNames:  DataTable Column Headers, '~'-separated
			AlertRows: DataRows from getTableRow fucntion
			Colors:    String indicating color theme

	____________________________________________________________________*/