/*== Validate all requirements to remove Order Hold ==========================

    Method Directive | SalesOrder.MasterUpdate | Pre-Processing

    Project:   2407_SystemUplift
    Created:   2024/08/02
    Version:   2.1.1

    Info:
        -Validate all Conditions required to remove Order Hold
        -Most of these were migrated from Order Entry Customization
        -Several added to for Third Party CC Payment Validation


    NOTE:
        Some Code has been changed to remove Protected Information

    -----------------------------------------------------------------
    
    Created by Kevin Veldman for Review by Ducommun, Inc. and Associated Entities

                Phone: (574) 707-2782
                Email: kevin.veldman@rfptsolutions.com
============================================================================*/

//
// Quit when Order is unavailable, or User is an Importer
//
    string ErrorMessage = "";

    var oh = ds.OrderHed.FirstOrDefault( x => x.Added() || x.Updated() );

    if ( oh == null ) return;


    var Importers = new List<string>{ };
    
    bool ImporterUser = Importers.Contains(callContextClient.CurrentUserId.ToLower());

    if ( ImporterUser ) return;


//
// Set Brand Order boolean. Some conditions ignored for Other Brand Orders.
//
    var BrandIDs = new List<string>{  };
    var thisCustID = Db.Customer.Where( x => x.CustNum == oh.CustNum && BrandIDs.Contains(x.CustID) )
                                .Select( s => s.CustID )
                                .FirstOrDefault() ?? string.Empty;

    bool BrandOrder = !string.IsNullOrEmpty( thisCustID );



//
// Validate Email using regex Pattern Matching
//
    Func<string,bool> ValidEmail = EmailAddr => 
    {
        string regexPattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        var re = new Regex(regexPattern);
        return re.IsMatch(EmailAddr);
    };


    string ShipConEmail = (string)oh["Character02"];

    if ( !ValidEmail(ShipConEmail) )
    {
        ErrorMessage = "Invalid Ship Contact Email";
        throw new Ice.BLException(ErrorMessage);
    }


//
// ShipVia Code must be selected
//
    if ( oh.ShipViaCode == "" )
    {
        ErrorMessage = "No ShipVia Code Selected";
        throw new Ice.BLException(ErrorMessage);
    }


//
// Taxes must be calculated for PS Brand Orders
//
    if ( !oh.ReadyToCalc && !BrandOrder )
    {
        ErrorMessage = "Taxes must be calculated before Order Hold is removed.\nPlease verify that the Ready To Process flag is checked.";
        throw new Ice.BLException(ErrorMessage);
    }



//
// Confirm Shipping has been calculated and added, if required
//
    bool ShipCalcRequired = (decimal)oh["Number06"] == 2m && !BrandOrder;
    bool ShipCalculated   = (bool)oh["CheckBox04"];
    bool OverrideShipping = (bool)oh["CheckBox03"];

    if ( ShipCalcRequired && !ShipCalculated && !OverrideShipping ) 
    {
        ErrorMessage = "Shipping must be calculated before Order Hold is removed.\nPlease ensure that Shipping has been calculated.";
        throw new Ice.BLException(ErrorMessage);
    }



//
// Confirm that Cash Deposit or full authorization for PS Brand orders with CreditCard Terms
//
    bool PSCreditCardOrder = !BrandOrder && oh.TermsCode.ToUpper() == "CC";

    if ( PSCreditCardOrder )
    {
        bool HasFullDeposit = false;
        bool HasCashDeposit = Db.CashHead.Any( x => x.OrderNum == oh.OrderNum && x.DocTranAmt >= 0 );
        
        if ( HasCashDeposit )
        {
            decimal TotalDeposits = Db.CashHead.Where( x => x.OrderNum == oh.OrderNum ).Sum( x => x.DocTranAmt );
            HasFullDeposit = TotalDeposits >= oh.DocOrderAmt;
        }


        if ( !HasFullDeposit )
        {
            bool HasAuthorization = oh.CCApprovalNum.Length > 0;

            if ( !HasAuthorization ) /// && !HasFullDeposit )
            {
                ErrorMessage = "No Credit Card Authorization for open balance exists. Please Authorize customer card.";
                throw new Ice.BLException(ErrorMessage);
            }


            decimal RemainingBalance = oh.DocOrderAmt;

            var OpenLines = Db.OrderDtl.Where( x => x.OrderNum == oh.OrderNum && !x.OpenLine && !x.VoidLine && x.KitFlag != "C" );
            
            foreach ( var od in OpenLines )
            {
                RemainingBalance -= (od.DocOrdBasedPrice * od.OrderQty);
                RemainingBalance -= Db.OrderMsc.Where( x => x.OrderNum == od.OrderNum && x.OrderLine == od.OrderLine ).Sum( x => x.MiscAmt );
            }

            bool ValidAmount = oh.CCDocTotal >= RemainingBalance;

            if ( !ValidAmount )
            {
                ErrorMessage = "Credit Card Authorization is less than the Order Total. \nPlease void existing authorization and re-authorize for the correct amount.";
                throw new Ice.BLException(ErrorMessage);
            }
        }
    }






        /*__ Reference ___________________________________


        ________________________________________________*/