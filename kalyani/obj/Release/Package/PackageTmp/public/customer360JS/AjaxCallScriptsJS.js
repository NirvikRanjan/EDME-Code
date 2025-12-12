
var UserName = $('#PkUsername').val();
var location_cityId = $('#location_Id').val();
$('.numberOnly').keypress(function (e) {

    if (isNaN(this.value + "" + "." + String.fromCharCode(e.charCode))) {
        return false;
    }

});


$('.normaldatepicker').datepicker({
    duration: "slow",
    showOptions: { direction: "up" },
    dateFormat: 'yy-mm-dd',
    changeMonth: true,//this option for allowing user to select month
    changeYear: true //this option for allowing user to select from year range

});
$('.blockfuturedate').datepicker({
    duration: "slow",
    showOptions: { direction: "down" },
    dateFormat: 'yy-mm-dd',
    maxDate: 0,
    changeMonth: true,//this option for allowing user to select month
    changeYear: true //this option for allowing user to select from year range

});
function OpenWhatsApp(ele) {
    debugger;
    var phNum = "91" + $('#ddl_phone_no option:selected').text();

    var msg = $('#smstemplate').val();
   // msg = encodeURIComponent.msg;

    var docName = $(ele).attr("data-file");
    var href1 = "";
    var doclink = $(ele).attr("data-dwnload");

    if (docName != undefined && docName !== "INS QUOTATION") {
        href1 = "https://api.whatsapp.com/send?phone=" + phNum + "&text=Please Refer Attached File Name: "+ docName + " " + encodeURI(doclink)
        $('#whatsAppBtn').attr("data-from", "doc");
        $('#whatsAppBtn').attr('data-file', "Refer attached file:" + docName);
        window.open(href1, '_blank');
        $('#whatsAppModal').modal({
            backdrop: 'static',
            keyboard: false
        });
        $('#smsPopuId').modal('hide');
    }
    else if (docName == "INS QUOTATION")
    {
        var quationID =$(ele).attr("data-quatationID");
        $.when(getwhatsappTemplate(58, location_cityId, 'sms', quationID)).done(function () {
            msg = $('#smstemplate').val();
            //$('#smstemplate').val('');
            if (msg != "") {
                href1 = "https://api.whatsapp.com/send?phone=" + phNum + "&text=" + encodeURIComponent(msg);
                window.open(href1, '_blank');
                $('#whatsAppModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
                $('#smsPopuId').modal('hide');
            }
        });
      
    }
    else {
        href1 = "https://api.whatsapp.com/send?phone=" + phNum + "&text=" + encodeURIComponent(msg);
      
        window.open(href1, '_blank');
      
        
        $('#whatsAppModal').modal({

            backdrop: 'static',
            keyboard: false
        });

        //fixedEncodeURIComponent(str);
        $('#smsPopuId').modal('hide');
    }
   
}





function getwhatsappTemplate(selectedIdSMSType, selectedLocId, msgType, quationID) {
    return $.ajax({
        type: 'POST',
        url: siteRoot + "/customer360/getQutationTemplate/",
        datatype: 'json',
        //async: false,
        cache: false,
        data: { smsId: selectedIdSMSType, locId: selectedLocId, msgType: msgType, quationID: quationID },
        success: function (res) {
            if (res.success == true) {

                if (msgType == "sms") {
                    $('#smstemplate').val(res.sms);
                }
                else if (msgType == "email") {
                    $('#emailSubject').val(res.emailSub);
                    $('#emailtemplate').val(res.emailTemplate);
                }
            }
            else {
                Lobibox.notify('warning', {
                    continueDelayOnInactiveTab: true,
                    msg: res.error
                });
            }
            $('#mainLoader').fadeOut('slow');
        },
        error: function (ex) {
            alert(ex);
            $('#mainLoader').fadeOut('slow');
        }
    });

}

function putWhatsappResponse(ele) {
    //debugger;
    var value = $("#whatsAppStatus option:selected").val();
    var phNum = "91" + $('#ddl_phone_no option:selected').text();
    var msg = "";
    var responseFrom = "";
    var datafrom = $(ele).attr('data-from');
    if (datafrom == "doc") {
        msg = $(ele).attr('data-file');
        responseFrom = "doc";
    }
    else {
        msg = $('#smstemplate').val();
        msg = msg.replace(/[&\/\\#,+()$~%.'":*?<>{}]/g, '');
        responseFrom = "message";
    }
    try {
        if (value != "2") {
            $.ajax({
                type: 'POST',
                url: siteRoot + '/customer360/sendWhatsappMsg/',
                datatype: 'json',
                cache: false,
                data: { phNum: phNum, msg: msg, reason: value, responseFrom: responseFrom },
                success: function (res) {
                    if (res.success == true) {
                        Lobibox.notify('info', {
                            continueDelayOnInactiveTab: true,
                            msg: 'Response Recorded Successfully'
                        });
                        $('#smstemplate').val('');
                    }
                    else {
                        Lobibox.notify('warning', {
                            continueDelayOnInactiveTab: true,
                            msg: res.error
                        });
                        //alert(res.error);
                    }
                },
                error: function (ex) {
                   // alert("Server Error");
                }
            });
        }
        else {
            Lobibox.notify('warning', {
                continueDelayOnInactiveTab: true,
                msg: 'Please select sent status.'
            });
            $('#whatsAppModal').modal('show');
            $('#whatsAppModal').css({ 'display': 'block' });

        }
    }
    catch (err) {
        alert(err);
    }
}

function smsTypeChanged(ele, msgType) {
    selectedLocId = 0;

    if (msgType == "sms") {
        var selectedIdSMSType = $('#ddl_SMSType option:selected').val();
        //var selectedLocId = $('#ddl_workshop option:selected').val();


        if (selectedIdSMSType == "") {
            Lobibox.notify('warning', {
                continueDelayOnInactiveTab: true,
                msg: 'Please select SMS Type'
            });
            $('#smstemplate').val('');
            return false;
        }

        getTemplate(selectedIdSMSType, selectedLocId, 'sms');
    }
}

function getTemplate(selectedIdSMSType, selectedLocId, msgType) {
    $('#smstemplate').empty();


    $('#mainLoader').fadeIn('slow');
    $.ajax({
        type: 'POST',
        url: siteRoot + "/customer360/getTemplateMessage/",
        datatype: 'json',
        //async: false,
        cache: false,
        data: { smsId: selectedIdSMSType, locId: selectedLocId, msgType: msgType },
        success: function (res) {
            if (res.success == true) {

                if (msgType == "sms") {
                    $('#smstemplate').val(res.sms);
                }
                else if (msgType == "email") {
                    $('#emailSubject').val(res.emailSub);
                    $('#emailtemplate').val(res.emailTemplate);
                }
            }
            else {
                Lobibox.notify('warning', {
                    continueDelayOnInactiveTab: true,
                    msg: res.error
                });
            }
            $('#mainLoader').fadeOut('slow');
        },
        error: function (ex) {
            alert(ex);
            $('#mainLoader').fadeOut('slow');
        }
    });
}

function doCall() {

    Lobibox.confirm({
        msg: "Do you want to make call",
        callback: function ($this, type) {
            if (type === 'yes') {
                var phNum = $('#ddl_phone_no option:selected').text();
                if (phNum === "-" || phNum === "") {
                    Lobibox.notify('warning', {
                        continueDelayOnInactiveTab: true,
                        msg: 'Please select phone number'
                    });
                    return false;
                }
                $.ajax({
                    type: 'POST',
                    url: siteRoot + "/customer360/getUniqueID/",
                    datatype: 'json',
                    async: false,
                    cache: false,
                    data: {},
                    success: function (res) {
                        if (res.success == true) {

                            $.ajax({
                                type: 'POST',
                                url: siteRoot + "/customer360/initializeCall/",
                                datatype: 'json',
                                cache: false,
                                data: { phNum: phNum, uniqueid: res.unqId },
                                success: function (res) {
                                    if (res.success == true) {
                                        Lobibox.notify('info', {
                                            continueDelayOnInactiveTab: true,
                                            msg: 'Call made successfully'
                                        });
                                    }
                                    else {
                                        Lobibox.notify('warning', {
                                            continueDelayOnInactiveTab: true,
                                            msg: res.error
                                        });
                                    }
                                },
                                error: function (ex) {
                                    alert(ex);
                                }
                            });
                        }
                        else {
                            alert('something went wrong');
                        }
                    },
                    error: function (ex) {
                        alert(ex);
                    }
                });
            }
            else if (type === 'no') {
                return false;
            }
        }
    });


}


function callHistoryByVehicle(typeIs) {
   
    var dataTableCallDispo = $('#dispositionHistory').DataTable({
        "destroy": true,
        "ajax": {
            "url": siteRoot + "/customer360/getCallHistory/",
            "type": "POST",
            "datatype": "json",
            "data": { typeIs: typeIs },
            "error": function (xhr, error, thrown) { console.log(xhr, error, thrown); }
        },
        "columns": [
            { "data": "AssignId", "name": "AssignId" },

            { "data": "CallDate", "name": "CallDate" },
            { "data": "Time", "name": "Time" },
            { "data": "CreId", "name": "CreId" },

            { "data": "Campaign", "name": "Campaign" },
            { "data": "SecondaryDispo", "name": "SecondaryDispo" },
            { "data": "Details", "name": "Details" },
            { "data": "CreRemarks", "name": "CreRemarks" },
            { "data": "Feedback", "name": "Feedback" },
            { "data": "CallMadeType", "name": "CallMadeType" },
            { "data": "IsCallInitiated", "name": "IsCallInitiated" }

        ],
        responsive: true,
        "initComplete": function (data) {
            $('#tblException').text(data.json.exception);
            $('.dataTables_length').hide();
            $('.dataTables_filter').hide();

            //$('.dataTable').wrap('<div class="dataTables_scroll" />');
        },
        "serverSide": "true",
        "processing": "true",
        "serverSide": "true",
        "sorting": "false",
        //"scrollX": "true",
        //"scrollY": "300",
        "paging": "true",
        //"searching": "true",
        "language": {
            "processing": "Loading Please Wait.....!"
        },
        order: [],
        pageLength: 5
    });
}

function smsHistoryOfUser(typeIs) {

    var dataTableCallDispo = $('#sms_history').DataTable({
        "destroy": true,
        "ajax": {
            url: siteRoot + "/customer360/getSMSHistoryOfCustomer/",
            "type": "POST",
            "datatype": "json",
            data: {},
            "error": function (xhr, error, thrown) { console.log(xhr, error, thrown); }
        },
        "columns": [
            { "data": "interactionDate", "name": "interactionDate" },
            { "data": "interactionTime", "name": "interactionTime" },
            { "data": "WyzuserName", "name": "WyzuserName" },
            {
                "data": "mobileNumber", render: function (data, type, row) {
                    return singlephoneMasking(data);
                }
            },
            { "data": "smsType", "name": "smsType" },
            { "data": "smsMessage", "name": "smsMessage" },
            { "data": "smsStatus", "name": "smsStatus" }
        ],
        columnDefs: [{
            targets: "_all",
            orderable: false
        }],
        responsive: true,
        "initComplete": function (data) {
            $('#tblsmsHstry').text(data.json.exception);
        },
        "processing": "true",
        "bFilter": false,
        "bLengthChange": false,
        "sorting": "false",
        "paging": "true",
        "language": {
            "processing": "Loading Please Wait.....!"
        },
        order: [],
        pageLength: 5
    });

}

function renewalHistoryOfVehicle() {
    var dataTableCallDispo = $('#renewal_history').DataTable({
        "destroy": true,
        "ajax": {
            url: siteRoot + "/customer360/getrenewalHistoryOfVehicle/",
            "type": "POST",
            "datatype": "json",
            data: {},
            "error": function (xhr, error, thrown) { console.log(xhr, error, thrown); }
        },
        "columns": [

            {
                "data": "Action", render: function (data, type, row) {
                    return `<a id='whatsapp' onclick='OpenWhatsApp(this)' data-file='INS QUOTATION' data-quatationID=${row.id} data-dismiss='modal'><i class="fa fa-whatsapp"  style="color:green;font-size:20px;"></i><a>`
                }
            },
            {
                "data": "disposeddate", render: function (data, type, row) {
                    return reverDate(data) + " " + row.disposedtime;
                }
            },
            {
                "data": "", render: function (data, type, row)
                {
                    if (row.ResponseCode == "200") {
                        return "Yes";
                    }
                    else
                    {
                        return "No";
                    }
                }
            },
            { "data": "ResponseMessage", "name": "ResponseMessage" },
            { "data": "ResponseCode", "name": "ResponseCode" },
            { "data": "campaignName", "name": "campaignName" },
            { "data": "creName", "name": "creName" },
            { "data": "insurancecompany", "name": "insurancecompany" },
            { "data": "renewwaltype", "name": "renewwaltype" },
            { "data": "Basic_IDV_Tenure", "name": "Basic_IDV_Tenure" },
            { "data": "Basic_OD_Tenure", "name": "Basic_OD_Tenure" },
            { "data": "NCB_Value_Tenure", "name": "NCB_Value_Tenure" },
            { "data": "Zero_Dep_Tenure", "name": "Zero_Dep_Tenure" },
            { "data": "RTI_Tenure", "name": "RTI_Tenure" },
            { "data": "Basic_TP_Tenure", "name": "Basic_TP_Tenure" },
            { "data": "grosspremium", "name": "grosspremium" },
            { "data": "totalgstamt", "name": "totalgstamt" },
            { "data": "totalpremium", "name": "totalpremium" }
        ],
        columnDefs: [{
            targets: "_all",
            orderable: false
        }],
        responsive: true,
        "initComplete": function (data) {
            $('#tblrenewalhstry').text(data.json.exception);
        },
        "processing": "true",
        "bFilter": false,
        "bLengthChange": false,
        "sorting": "false",
        "paging": "true",
        "language": {
            "processing": "Loading Please Wait.....!"
        },
        order: [],
        pageLength: 5
    });

}

function AssignmentHistoryOfCustomer(typeIs) {

    var dataTableAssgnmntHstry = $('#assignment_smr_history').DataTable({
        "destroy": true,
        "ajax": {
            url: siteRoot + "/customer360/getAssignmentHistoryOfVehicleId/",
            "type": "POST",
            "datatype": "json",
            data: { moduleType: typeIs },
            "error": function (xhr, error, thrown) { console.log(xhr, error, thrown); }
        },
        "columns": [
            { "data": "assignedId", "name": "assignedId" },
            {
                "data": "assignDate", render: function (data, type, row) {
                    return parseJsonDateTime(data)
                }
            },
            { "data": "reason", "name": "reason" },
            { "data": "WyzuserName", "name": "WyzuserName" },
            { "data": "smsType", "name": "smsType" },
            { "data": "smsMessage", "name": "smsMessage" },
            { "data": "reassign", "name": "reassign" },
            { "data": "reassignhistory", "name": "reassignhistory" },
            { "data": "resonforDrop", "name": "resonforDrop" },
            { "data": "convertedstatus", "name": "convertedstatus" }
        ],
        columnDefs: [{
            targets: "_all",
            orderable: false
        }],
        responsive: true,
        "initComplete": function (data) {
            $('#tbsmrhstryException').text(data.json.exception);
        },
        "processing": "true",
        "bFilter": false,
        "bLengthChange": false,
        "sorting": "false",
        "paging": "true",
        "language": {
            "processing": "Loading Please Wait.....!"
        },
        order: [],
        pageLength: 5
    });
}




function GetWinbackFinalProposal() {
   
    var dataTableFinalProposal = $('#Final_Proposal').DataTable({
        "destroy": true,
        "ajax": {
            url: siteRoot + "/customer360/getfinalproposal/",
            "type": "POST",
            "datatype": "json",
            data: {},
            "error": function (xhr, error, thrown) { console.log(xhr, error, thrown); }
        },
        "columns": [

            {
                "data": "Action", render: function (data, type, row) {
                    return `<input name="rdoselectproposal" type="radio" id='rdowinback'  onclick="appendFinalproposalId(${row.id})" ></button>`
                }
            },
            { "data": "productName", "name": "productName" },
           
            //{
            //    "data": "IDV", render: function (data, type, row) {
            //        return data
                   
            //    }
            //},
           
            { "data": "Basic_IDV", "name": "Basic_IDV" },
            

            { "data": "Basic_Prem_Vehicle", "name": "Basic_Prem_Vehicle" },
            { "data": "NCB_Value", "name": "NCB_Value" },
           /* { "data": "Total_Deductions", "name": "Total_Deductions" },*/
            { "data": "PA_Owner_Driver", "name": "PA_Owner_Driver" },
            { "data": "Total_ODP_A", "name": "Total_ODP_A" },
          /*  { "data": "Total_Liability_Prem_B", "name": "Total_Liability_Prem_B" },*/

            { "data": "TPPD_Vehicle_Prem", "name": "TPPD_Vehicle_Prem" },
            
            { "data": "RTI_Premium", "name": "RTI_Premium" },
            { "data": "NilDepreciation_Premium", "name": "NilDepreciation_Premium" },
            {
                "data": "Action", render: function (data, type, row) {
                    var NETPREMIUM = row.Gross_Prem - (row.IGST_Amt + row.SGST_Amt + row.CGST_Amt);
                    //var NETPREMIUM = row.Basic_Prem_Vehicle + row.NCB_Value + row.Total_Deductions + row.Total_ODP_A + row.Total_Liability_Prem_B + row.RTI_Premium + row.NilDepreciation_Premium;
                    return NETPREMIUM;
                }
            },
            {
                "data": "Action", render: function (data, type, row) {
                    var GSTTotal = row.IGST_Amt + row.SGST_Amt + row.CGST_Amt;
                    return GSTTotal;
                }
            },
            { "data": "Gross_Prem", "name": "Gross_Prem" },


            {
                "data": "AssignDate", render: function (data, type, row) {
                    return parseJsonDateTime(data)
                    //return Date.now.toString(data)
                    
                }
            },




            //{
            //    "data": "Action", render: function (data, type, row) {
            //        var date = Date.Now.ToString("dd/MM/yyyy");
            //        return date;
            //    }
            //},



            //{
            //    "data": "Action", render: function (data, type, row) {
            //        var discount = row.Discount_AntiTheft + row.Discount_AA_Mem + row.DisCount_HandiCaped + row.Discount_Others;
            //        return discount;
            //    }
            //},
            //{ "data": "Total_Prem_A_B", "name": "Total_Prem_A_B" },
            //{ "data": "Total_Prem_A_B", "name": "Total_Prem_A_B" },


         
            //{ "data": "Total_Prem_A_B", "name": "Total_Prem_A_B" },

            //{
            //    "data": "Action", render: function (data, type, row) {
            //        var NetPremium = row.Basic_Prem_Access + row.NCB_Value + row.discount + row.Total_Prem_A_B + row.Total_Prem_A_B + row.RTI_Premium + row.Total_Prem_A_B;
            //        return NetPremium;
            //    }
            //},

        ],
        columnDefs: [{
            targets: "_all",
            orderable: false
        }],
        responsive: true,
        "initComplete": function (data) {
            $('#tblProposalResult').text(data.json.exception);
        },
        "processing": "true",
        "bFilter": false,
        "bLengthChange": false,
        "sorting": "false",
        "paging": false,
        "language": {
            "processing": "Loading Please Wait.....!"
        },
        order: [],
        pageLength: 5
       

    });
  
}




function appendFinalproposalId(finalproposalId) {
    $('#btnfinalproposalsubmit').show();
    $('#btnfinalproposalsubmit').removeAttr('onclick');
    $('#btnfinalproposalsubmit').attr('onClick', 'WinbackFinalProposal(' + finalproposalId + ');');
}

function WinbackFinalProposal(renewalresponseId) {
    $('#mainLoader').fadeIn('slow');

    var campaign = $("#assignCampaignId").val();

    $.ajax({
        type: 'POST',
        url: siteRoot +"/customer360/sendFinalProposal",
        datatype: 'json',
        data: { renewalresponseId: renewalresponseId, campaignId : campaign },
        
        cache: false,
        success: function (res) {
            if (res.success == true) {
                $('#divFinalProposal').show();
                Lobibox.notify('success', {
                    continueDelayOnInactiveTab: true,
                    msg: 'Final Proposal Sent.'
                });
                $('#mainLoader').fadeOut('slow');

            }
            else {
                $('#mainLoader').fadeOut('slow');
            }
        }, error(error) {
            $('#mainLoader').fadeOut('slow');

        }
    });

}


function getFinalQuoteResponse() {
    var dataTableFinalProposal = $('#tblfinalQuote').DataTable({
        "destroy": true,
        "ajax": {
            url: siteRoot + "/customer360/getfinalproposalResponse/",
            "type": "POST",
            "datatype": "json",
            data: {},
            "error": function (xhr, error, thrown) { console.log(xhr, error, thrown); }
        },
        "columns": [

            { "data": "ResponseCode", "name": "ResponseCode" },
            { "data": "ResponseMessage", "name": "ResponseMessage" },
            { "data": "PolicyNo", "name": "PolicyNo" },
            { "data": "HeroTransaction", "name": "HeroTransaction" },
            { "data": "PremAmount", "name": "PremAmount" }


        ],
        columnDefs: [{
            targets: "_all",
            orderable: false
        }],
        responsive: true,
        "initComplete": function (data) {
            $('#tblexceptionfinalQuote').text(data.json.exception);
        },
        "processing": "true",
        "bFilter": false,
        "bLengthChange": false,
        "sorting": "false",
        "paging": false,
        "language": {
            "processing": "Loading Please Wait.....!"
        },
        order: [],
        pageLength: 10
    });

}









function getCityByStateSelection(sel, selCity) {

    var stateSel = document.getElementById(sel).value;
    // alert(stateSel);

    var urlLink = siteRoot + "/customer360/getCityNames/";

    $.ajax({
        type: 'POST',
        url: urlLink,
        datatype: 'json',
        data: { state: stateSel },
        cache: false,
        success: function (res) {
            if (res.success == true) {
                if (res.cities != null) {
                    $('#' + selCity).empty();
                    var dropdown = document.getElementById(selCity);
                    dropdown[0] = new Option('--SELECT--', '--SELECT--');
                    for (var i = 0; i < res.cities.length; i++) {
                        dropdown[dropdown.length] = new Option(res.cities[i], res.cities[i]);
                    }
                }
            }
            else {
                alert(res.error);
            }
        }, error(error) {

        }
    });
}





function savecustomerDetails(saveType) {

    var phonenumber = $('#myPhoneNum').val();
    var reasons = $('#myPhoneRemark').val();
    var email = $('#myEmailNum').val();

    var dnd = $("input[name='cust.doNotDisturb']:checked").val();

    var dob = $('#dob').val();
    var doa = $('#anniversary_date').val();
    var customerId = $('#txtcustId').val();
    var phoneId = $('#preffered_contact_num').val()
    var Type = saveType;

    if (Type == "SavePhone") {
        if (phonenumber == "") {
            Lobibox.notify('warning', {
                msg: 'Please Enter Phone Number!'
            });
            return false;
        }
        if (phonenumber.length < 10) {
            Lobibox.notify('warning', {
                continueDelayOnInactiveTab: true,
                msg: 'Invalid Phone No.'
            });

            return false;


        } 
        if (reasons == "") {
            Lobibox.notify('warning', {
                msg: 'Please Enter Reason for Adding New Number!'
            });
            return false;
        }
    }

    if (Type == "DeletePhone") {
               if (reasons == "") {
            Lobibox.notify('warning', {
                msg: 'Please Enter Reason for Deleting Number!'
            });
            return false;
        }
    }
    if (Type == "SaveEmail") {
        if (email == "") {
            Lobibox.notify('warning', {
                msg: 'Please Enter  Email!'
            });
            return false;
        }
    }

    customerData = { phonenumber: phonenumber, reasons: reasons, email: email, dnd: dnd, dob: dob, doa: doa, Type: Type, phoneId: phoneId }
    $('#mainLoader').css({ 'display': 'block' });

    $.ajax({
        url: siteRoot + "/customerUpdate/Saves/",
        dataType: 'json',
        type: 'post',
        data: { customerData: JSON.stringify(customerData) },
        success: function (res) {
            if (res.successPhone == true) {
                $('#myPhoneNum').val('');
                $('#myPhoneRemark').val('');
                $('#ddl_phone_no').empty();
                $("#preffered_contact_num").empty();
                var appenddata1 = "";
                var appenddata2 = "";
                var Phone = "";

                for (var i = 0; i < res.phnum.length; i++) {
                    Phone = Phone + res.phnum[i].phoneNo + ",";
                    appenddata1 += "<option value ='" + res.phnum[i].id + "'>" + res.phnum[i].phoneNo + "</option>";
                    appenddata2 += "<option value ='" + res.phnum[i].id + "'>" + res.phnum[i].phoneNo + "</option>";

                }
                var phoneCount = Number(res.phnum.length);
                if (phoneCount > 1) {
                    $('#deletephonebtn').show();
                }
                else {
                    $('#deletephonebtn').hide();
                }
                $("#preffered_contact_num").append(appenddata1);
                $("#ddl_phone_no").append(appenddata2);
                $("#ddl_phone_noMask").append(appenddata2);
                $("#prefPhone").text(Phone);
                EditphoneMasking(res.phnum);

                $('#adddeletephonenumberDiv').hide();
                $('#myPhoneRemark').val('');
                Lobibox.notify('info', {
                    msg: 'Phone Number Saved successfully!'
                });

            }
            else if (res.successEmail == true) {
                $('#myEmailNum').val('');
                $('#emailId').val('');
                $("#email").empty();
                var appenddata1 = "";
                var EMAILS = "";

                for (var i = 0; i < res.emailAddress.length; i++) {
                    if (res.emailAddress[i].pref == true) {
                        EMAILS = res.emailAddress[i].email;

                    }
                    appenddata1 += "<option value = '" + res.emailAddress[i].id + " '>" + res.emailAddress[i].email + " </option>";


                }
                $("#email").append(appenddata1);
                $("#ddl_email").text(EMAILS);
                $('#addemailDiv').hide();
                $('#myEmailNum').val('');
                Lobibox.notify('info', {
                    msg: 'Email Saved successfully!'
                });
            }
            else if (res.successDelete == true) {

                $('#ddl_phone_no').empty();
                $("#preffered_contact_num").empty();
                var appenddata1 = "";
                var appenddata2 = "";
                var Phone = "";

                for (var i = 0; i < res.phnum.length; i++) {
                    Phone = Phone + res.phnum[i].phoneNo + ",";
                    appenddata1 += "<option value = '" + res.phnum[i].id + "'>" + res.phnum[i].phoneNo + " </option>";
                    appenddata2 += "<option value = '" + res.phnum[i].id + "'>" + res.phnum[i].phoneNo + " </option>";
                }

                var phoneCount = Number(res.phnum.length);
                if (phoneCount > 1) {
                    $('#deletephonebtn').show();
                }
                else {
                    $('#deletephonebtn').hide();
                }

                $("#preffered_contact_num").append(appenddata1);
                $("#ddl_phone_no").append(appenddata2);
                $("#prefPhone").text(Phone);

                EditphoneMasking(res.phnum);

                $('#adddeletephonenumberDiv').hide();
                $('#myPhoneRemark').val('');
                Lobibox.notify('success', {
                    msg: 'Mobile number deleted successfully'
                });
            }
            else if (res.successCustomerInfo == true) {
                var dnd = Boolean(res.dnd);
                if (dnd == true) {
                    $("#btnDND").css("display", "block")

                }
                else {
                    $("#btnDND").css("display", "none")

                } Lobibox.notify('success', {
                    msg: 'Customer Updated successfully'
                });
            }
            else {
                Lobibox.notify('warning', {
                    msg: 'something went Wrong'
                });
            }
            $('#mainLoader').fadeOut('slow');
            $('#phErro').text('');
        }, error: function (error) {
            alert(error);
            console.log(error);
            //return false;
        }
    });
}

/**Address Code Starts Here */
function getAddressForPopUp() {
    var userRole = $('#userrole').val();
    var dataTableCallDispo = $('#allAddress').DataTable({
        "destroy": true,
        "ajax": {
            url: siteRoot + '/customerUpdate/getDataForAddressTable/',
            "type": "POST",
            "datatype": "json",
            data: {},
        },
        "columns": [
            {
                "data": "Action", render: function (data, type, row) {
                    if (row.isPreferred == true) {
                        return "-";
                    }
                    else {
                        return `<a class="fa fa-address-book-o" title="Make Default Address" onclick="setAsDefaultAddress(${row.id})" style="font-size:18pt;color: green;"> </a>`
                    }
                }
            },
            { "data": "concatenatedAdress", "name": "concatenatedAdress" },
            { "data": "state", "name": "state" },
            { "data": "city", "name": "city" },
            { "data": "pincode", "name": "pincode" },
            {
                "data": "Action", render: function (data, type, row) {
                    if (row.isPreferred == true)
                    {
                        return "-";
                    }
                    else {
                        return `<a class="fa fa-trash-o" onclick="deleteAddress(${row.id})" style="font-size: 18pt;color: red;"> </a>`
                    }
                }
            }
        ],
        columnDefs: [{
            targets: "_all",
            orderable: false
        }],
        responsive: true,
        "initComplete": function (data) {
            if (userRole == "CRE") {
                dataTableCallDispo.columns(5).visible(false)

            }
            else {
                dataTableCallDispo.columns(5).visible(true)

            }
        },
        "processing": "true",
        "bFilter": false,
        "bLengthChange": false,
        "sorting": "false",
        "paging": "true",
        "language": {
            "processing": "Loading Please Wait.....!"
        },
        order: [],
        pageLength: 5
    });
}


function deleteAddress(AddressId) {
 

    $.ajax({
        url: siteRoot + '/customerUpdate/deleteRowFromPopUp/',
        type: 'post',
        dataType: 'json',
        async: false,
        data: { AddressId: AddressId },
        success: function (res) {
            if (res.success == true) {
                Lobibox.notify('warning', {
                    msg: 'Deleted Successfuly'
                });
                getAddressForPopUp();

            }
        }
    });
}

function updateNewAddress() {
    var validated = false;
    var checkboxVal = $('#IsPrimary').is(":checked");
    var addrVal = $('#newAddress').val();
    var stateVal = $('#pstateId').val();
    var cityVal = $('#paddrrCity').val();
    var pinVal = $('#ppincode').val();

    if (addrVal == "" && stateVal == "" && cityVal == "" && pinVal == "") {
        Lobibox.notify('warning', {
            continueDelayOnInactiveTab: true,
            msg: 'Please enter all required fields'
        });
        return false;
    }
    if (addrVal == "") {
        Lobibox.notify('warning', {
            size: 'mini',
            title: 'Warning',
            msg: 'Please Enter Address.'
        });
        return false;
    }
    if (stateVal == "") {
        Lobibox.notify('warning', {
            size: 'mini',
            title: 'Warning',
            msg: 'Please Enter State.'
        });
        return false;
    }
    if (cityVal == "--SELECT--") {
        Lobibox.notify('warning', {
            size: 'mini',
            title: 'Warning',
            msg: 'Please Enter City.'
        });
        return false;
    }
    if (pinVal == "") {
        Lobibox.notify('warning', {
            size: 'mini',
            title: 'Warning',
            msg: 'Please Enter Pincode.'
        });
        return false;
    }
    var addressObject = { address: addrVal, state: stateVal, city: cityVal, pincode: pinVal, isPrimary: checkboxVal }
    $.ajax({
        url: siteRoot + '/customerUpdate/AddNewAddress/',
        dataType: 'json',
        type: 'post',

        data: { values: JSON.stringify(addressObject) },
        success: function (result) {
            if (result.success) {
                if (result.isPrimary == true) {
                    $('#prefAdressUpdate').text(result.address);
                    $('#city').text(result.city);
                    $('#pin').text(result.pincode);
                }
                Lobibox.notify('success', {
                    msg: 'Address added successfully'
                });
                getAddressForPopUp();
                reset();
            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function setAsDefaultAddress(AddressId) {
    $.ajax({
        url: siteRoot + "/customerUpdate/setasDefaultAddress/",
        type: 'post',
        dataType: 'json',
        data: { AddressId: AddressId },
        success: function (res) {
            if (res.success) {
                Lobibox.notify('success', {
                    msg: 'Address Changed successfully.'
                });
                $('#prefAdressUpdate').text(res.prefAddress);
                getAddressForPopUp();

            }
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function NumOfFile(ele) {
    if (ele.files.length > 3) {
        Lobibox.notify('warning', {
            continueDelayOnInactiveTab: true,
            msg: 'you cant upload files more than 3'
        });
        $(ele).val("");
    }
}

function getDocuments() {
    $.ajax({
        type: 'POST',
        url: siteRoot + "/customer360/getDocuments/",
        datatype: 'json',
        cache: false,
        data: {},
        success: function (res) {
            // if (res.success == true) {
            if (res.success == true && res.files != "") {
                if (res.files.includes('%')) {
                    var files = res.files.split('%');
                    var docName = res.fileName.split(',');
                    var deptname = res.deptName.split(',');
                    var uploadedDatetime = res.uploadedDateTime.split(',');
                    var docFileName = res.docFileName.split('%');
                    var docIds = res.docId.split(',');
                    var creNames = res.usernames.split(',');

                    $('#tblDocument tbody tr').remove();
                    if ($.fn.DataTable.isDataTable("#tblDocument")) {
                        var table = $("#tblDocument").DataTable();
                        table.clear();
                        table.destroy();
                    }
                    for (var i = 0; i < files.length; i++) {
                        bindFiles(files[i], deptname[i], docName[i], uploadedDatetime[i], creNames[i], docIds[i], docFileName[i]);
                    }
                }
                else {
                    $('#tblDocument tbody tr').remove();
                    if ($.fn.DataTable.isDataTable("#tblDocument")) {
                        var table = $("#tblDocument").DataTable();
                        table.clear();
                        table.destroy();
                    }
                    bindFiles(res.files, res.deptName, res.fileName, res.uploadedDateTime, res.usernames.split(',')[0], res.docId, res.docFileName);
                }
                $("#tblDocument").dataTable({
                    //destroy: true
                    responsive: true,
                    
                });

            }
            else if (res.success == true && res.files == "")
            {
                $('#tblDocument').DataTable({
                   

                    "info": true,
                    "searching": false,
                    "bLengthChange": false,
                    "paging": false,
                    "bDestroy": true
                });
            }
            else {
                if (res.success == false) {
                    Lobibox.notify('warning', {
                        continueDelayOnInactiveTab: true,
                        msg: res.error
                    });

                }

            }
            //}
            //else {
            //    alert('something went wrong');
            //}
        },
        error: function (ex) {
            alert(ex);
        }
    });
}

function bindFiles(files, deptname, docName, Datetime, creNames, docId, docFileName) {
    //debugger;
    //alert("File Uploaded successfully");
    var DataTable = $('#tblDocument tbody');
    //$(DataTable).empty();

    var DataRow = "";
    var codePath = window.location.href;
    var callIndex = codePath.indexOf('customer360');
    var callPath = codePath.substr(callIndex, (codePath.length - 1));
    codePath = codePath.replace(callPath, '');
    var docUploadedName = docFileName
    var allPaths = "";

    var file1 = "", file2 = "", file3 = "", file1Name = "", file2Name = "", file3Name = "";
    if (files.includes(';')) {
        var fileBase = files.split('#')[1];
        files = files.split('#')[0];

        if (files.split(';')[0] != null || files.split(';')[0] != undefined) {
            file1 = '<a href="' + codePath + fileBase + files.split(';')[0] + '" download><i class="fa fa-download">' + docUploadedName.split(';')[0] + '</a>';
            file1Name = docUploadedName.split(';')[0];
            allPaths = (codePath + fileBase + files.split(';')[0]).replace(/ /g, "%20");

        }

        if (files.split(';')[1] != null || files.split(';')[1] != undefined) {
            file2 = '<a href="' + codePath + fileBase + files.split(';')[1] + '" download><i class="fa fa-download">' + docUploadedName.split(';')[1] + '</a>';
            file2Name = docUploadedName.split(';')[1];
            allPaths = " , " + (codePath + fileBase + files.split(';')[1]).replace(/ /g, "%20");

        }

        if (files.split(';')[2] != null || files.split(';')[2] != undefined) {
            file2 = '<a href="' + codePath + fileBase + files.split(';')[2] + '" download><i class="fa fa-download">' + docUploadedName.split(';')[2] + '</a>';
            file3Name = docUploadedName.split(';')[2];
            allPaths = " , " + (codePath + fileBase + files.split(';')[2]).replace(/ /g, "%20");

        }
    }
    else {
        var fileBase = files.split('#')[1];
        files = files.split('#')[0];

        file1 = '<a href="' + codePath + fileBase + files + '" download><i class="fa fa-download">' + docFileName + '</i></a>';
        file1Name = files;
        allPaths = (codePath + fileBase + files.split(';')[0]).replace(/ /g, "%20");

    }

    var deptName = deptname;
    var documentName = docName;
    //$('#deptName').val('');
    $('#documentName').val('');
    $('#fileList').val();
    if (creNames == null && creNames == undefined) {
        creNames = UserName;
    }

    //for (var i = 0; i<res.doc.length; i++) {
    var row = `<tr>
                                <td>${deptName}</td>
                                <td>${creNames}</td>
                                <td>${Datetime}</td>
                                <td>${documentName}</td>
                                <td>${file1}${file2}${file3}</td>
                                <td><button type="button" id="whatsappDoc" onclick="OpenWhatsApp(this)" data-file="${documentName}" data-dwnload="${allPaths}" class="btn btn-default whatsapp"><img src="${codePath}/public/images/whatsapp_logo.png" width='30px'/></button></td>
                            </tr>`;
    DataRow = DataRow + row;
    //}
    $(DataTable).append(DataRow);
}
//file upload success function
function fileSuccess(res) {
    //debugger;
    if (res.success == true) {
        $('#mainLoader').fadeOut('slow');
        getDocuments();

    }
    else {
        Lobibox.notify('warning', {
            continueDelayOnInactiveTab: true,
            msg: res.error
        });

        //  alert("Could not upload the file.. try after sometime?");
        $('#mainLoader').fadeOut('slow');
    }
}

function fileFailure(error) {
    alert("Internal Server error couldn't upload file.");
    $('#mainLoader').fadeOut('slow');

}

//-------------------------------- Document Upload and Document Handling Related Support Function --------------------------- End

/********************/

function doDialpadCall() {
    var phNum = $('#ddl_phone_no option:selected').text();

    $.ajax({
        type: 'POST',
        url: siteRoot + '/customer360/dialpadclicktoCall/',
        datatype: 'json',
        data: { phNum: phNum },
        cache: false,
        success: function (res) {
            if (res.success == true) {
                window.location = 'tel:' + phNum;
            }
            else {
                if (res.success == false) {
                    Lobibox.notify('warning', {
                        continueDelayOnInactiveTab: true,
                        msg: res.error
                    });
                }
            }
        }
    });

}
