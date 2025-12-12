$(document).ready(function () {

    $('form').on('submit', function () {
        $('#mainLoader').css({ 'display': 'block' });
    });
});

$("input[name='insudisposition.typeOfDisposition']").click(function () {
    dispoType = $(this).val();
    if(dispoType == "Contact")
    {
        $('#WhatdidtheCustomersayDIV').show();
        $('#INSBookingDetailsDIV').hide();
        $('#INScallMeLattteDiv').hide();
        $('#INSBookingDiv').hide();
        $('#INSRenewalNotRequiredDiv').hide();
        $('#INSOnlineOfflineDiv').hide();
        $('#INSNoSpeechDiv').hide();
        $('#dispositionRemarksDiv').hide();
        $('#btnsubmitDispositionDIV').hide();
        $('#INSCancelInsuDiv').hide();

        clearnoncaontact();
    }
    else if (dispoType == "NonContact")
    {
        $('#WhatdidtheCustomersayDIV').hide();
        $('#INSRenewalNotRequiredDiv').hide();
        $('#INScallMeLattteDiv').hide();
        $('#INSOnlineOfflineDiv').hide();
        $('#INSBookingDetailsDIV').hide();
        $('#INSBookingDiv').hide();
        $('#INSNoSpeechDiv').show();
        $('#dispositionRemarksDiv').show();
        $('#btnsubmitDispositionDIV').show();
        $('#INSCancelInsuDiv').hide();

        clearBookAppointment();
        clearappointmnettype();
        clearModeofAppointmnet();

        clearwhatcustsays();
        clearrenewalNotRequired();
        clearcallmelater();
    }
});
$("input[name='insudisposition.dispoCustAns']").click(function () {
    dispoCustAns = $(this).val();
    if (dispoCustAns == "Book Appointment")
    {


        var appoinmntExist = $('#appointBookId').val();
        if (appoinmntExist != "") {
            Lobibox.confirm({
                msg: "This Insurance is already booked do you want to modify?",
                callback: function ($this, type) {
                    if (type === 'yes') {
                        $('#WhatdidtheCustomersayDIV').show();
                        $('#INSBookingDetailsDIV').hide();
                        $('#INScallMeLattteDiv').hide();
                        $('#INSBookingDiv').hide();
                        $('#INSRenewalNotRequiredDiv').hide();
                        $('#INSOnlineOfflineDiv').show();
                        $('#INSNoSpeechDiv').hide();
                        $('#dispositionRemarksDiv').hide();
                        $('#btnsubmitDispositionDIV').hide();
                        $('#INSCancelInsuDiv').hide();

                        clearBookAppointment();
                        clearappointmnettype();
                        clearModeofAppointmnet();

                        clearrenewalNotRequired();
                        clearcallmelater();
                    }
                    else {
                        $('#WhatdidtheCustomersayDIV').show();
                        $('#INSBookingDetailsDIV').hide();
                        $('#INScallMeLattteDiv').hide();
                        $('#INSBookingDiv').hide();
                        $('#INSRenewalNotRequiredDiv').hide();
                        $('#INSOnlineOfflineDiv').hide();
                        $('#INSNoSpeechDiv').hide();
                        $('#dispositionRemarksDiv').hide();
                        $('#btnsubmitDispositionDIV').hide();
                        $('#INSCancelInsuDiv').hide();

                        clearBookAppointment();
                        clearappointmnettype();
                        clearModeofAppointmnet();

                        clearrenewalNotRequired();
                        clearcallmelater();
                    }

                }
            });
        }
        else {
            $('#WhatdidtheCustomersayDIV').show();
            $('#INSBookingDetailsDIV').hide();
            $('#INScallMeLattteDiv').hide();
            $('#INSBookingDiv').hide();
            $('#INSRenewalNotRequiredDiv').hide();
            $('#INSOnlineOfflineDiv').show();
            $('#INSNoSpeechDiv').hide();
            $('#dispositionRemarksDiv').hide();
            $('#btnsubmitDispositionDIV').hide();
            $('#INSCancelInsuDiv').hide();

            clearBookAppointment();
            clearappointmnettype();
            clearModeofAppointmnet();

            clearrenewalNotRequired();
            clearcallmelater();
        }
    }
    if (dispoCustAns == "Renewal Not Required")
    {
        $('#WhatdidtheCustomersayDIV').show();
        $('#INSBookingDetailsDIV').hide();
        $('#INScallMeLattteDiv').hide();
        $('#INSBookingDiv').hide();
        $('#INSRenewalNotRequiredDiv').show();
        $('#INSOnlineOfflineDiv').hide();
        $('#INSNoSpeechDiv').hide();
        $('#dispositionRemarksDiv').show();
        $('#btnsubmitDispositionDIV').show();
        $('#INSCancelInsuDiv').hide();
        $('#btnsubmitDispositionDIV').hide();


        clearBookAppointment();
        clearappointmnettype();
        clearModeofAppointmnet();

        clearrenewalNotRequired();
        clearcallmelater();
    }
    if (dispoCustAns == "ConfirmedInsu")
    {
        $('#WhatdidtheCustomersayDIV').show();
        $('#INSBookingDetailsDIV').hide();
        $('#INScallMeLattteDiv').hide();
        $('#INSBookingDiv').hide();
        $('#INSRenewalNotRequiredDiv').hide();
        $('#INSOnlineOfflineDiv').hide();
        $('#INSNoSpeechDiv').hide();
        $('#dispositionRemarksDiv').show();
        $('#btnsubmitDispositionDIV').show();
        $('#INSCancelInsuDiv').hide();


        clearBookAppointment();
        clearappointmnettype();
        clearModeofAppointmnet();

        clearrenewalNotRequired();
        clearcallmelater();
    }
    if (dispoCustAns == "Cancel Appointment")
    {
        $('#WhatdidtheCustomersayDIV').show();
        $('#INSRenewalNotRequiredDiv').hide();
        $('#INScallMeLattteDiv').hide();
        $('#INSOnlineOfflineDiv').hide();
        $('#INSBookingDetailsDIV').hide();
        $('#INSBookingDiv').hide();
        $('#INSNoSpeechDiv').hide();
        $('#INSCancelInsuDiv').show();
        $('#dispositionRemarksDiv').show();
        $('#btnsubmitDispositionDIV').show();

        clearBookAppointment();
        clearappointmnettype();
        clearModeofAppointmnet();

        clearrenewalNotRequired();
        clearcallmelater();
    }
    if (dispoCustAns == "Call Me Later")
    {
        $('#WhatdidtheCustomersayDIV').show();
        $('#INSBookingDetailsDIV').hide();
        $('#INScallMeLattteDiv').show();
        $('#INSBookingDiv').hide();
        $('#INSRenewalNotRequiredDiv').hide();
        $('#INSOnlineOfflineDiv').hide();
        $('#INSNoSpeechDiv').hide();
        $('#dispositionRemarksDiv').show();
        $('#btnsubmitDispositionDIV').show();
        $('#INSCancelInsuDiv').hide();

        clearBookAppointment();
        clearappointmnettype();
        clearModeofAppointmnet();

        clearrenewalNotRequired();
        clearcallmelater();
    }
});

$("input[name='appointbooked.modeofappointment']").click(function () {
    modeofappointment = $(this).val();
    if (modeofappointment == "Online") {

        $('#fieldbookingDiv').hide();
        $('#walkinLocDivId').hide();

        $('#WhatdidtheCustomersayDIV').show();
        $('#INSBookingDetailsDIV').show();
        $('#INScallMeLattteDiv').hide();
        $('#INSBookingDiv').hide();
        $('#INSRenewalNotRequiredDiv').hide();
        $('#INSOnlineOfflineDiv').show();
        $('#INSNoSpeechDiv').hide();
        $('#dispositionRemarksDiv').show();
        $('#btnsubmitDispositionDIV').show();

        $('#fieldbookingDiv').hide();
        $('#walkinLocDivId').hide();
        clearappointmnettype();
    } else if (modeofappointment == "Offline")
    {
        $('#fieldbookingDiv').hide();
        $('#walkinLocDivId').hide();

        $('#WhatdidtheCustomersayDIV').show();
        $('#INSBookingDetailsDIV').hide();
        $('#INScallMeLattteDiv').hide();
        $('#INSBookingDiv').show();
        $('#INSRenewalNotRequiredDiv').hide();
        $('#INSOnlineOfflineDiv').show();
        $('#INSNoSpeechDiv').hide();
        $('#dispositionRemarksDiv').hide();
        $('#btnsubmitDispositionDIV').hide();

    }
    clearBookAppointment();
});

$("input[name='appointbooked.typeOfPickup']").click(function () {
    typeofPickup = $(this).val();
    if (typeofPickup == "Field") {
        $('#fieldbookingDiv').show();
        $('#walkinLocDivId').hide();

        $('#WhatdidtheCustomersayDIV').show();
        $('#INSBookingDetailsDIV').show();
        $('#INScallMeLattteDiv').hide();
        $('#INSBookingDiv').show();
        $('#INSRenewalNotRequiredDiv').hide();
        $('#INSOnlineOfflineDiv').show();
        $('#INSNoSpeechDiv').hide();
        $('#dispositionRemarksDiv').show();
        $('#btnsubmitDispositionDIV').show();

    }
    else if (typeofPickup == "Walk-in") {
        $('#fieldbookingDiv').hide();
        $('#walkinLocDivId').show();

        $('#WhatdidtheCustomersayDIV').show();
        $('#INSBookingDetailsDIV').show();
        $('#INScallMeLattteDiv').hide();
        $('#INSBookingDiv').show();
        $('#INSRenewalNotRequiredDiv').hide();
        $('#INSOnlineOfflineDiv').show();
        $('#INSNoSpeechDiv').hide();
        $('#dispositionRemarksDiv').show();
        $('#btnsubmitDispositionDIV').show();
    }
    clearBookAppointment();
});


function clearrenewalNotRequired()
{
    $('#chkrenewalNotRequired').val('');
    $('.NoService').each(function () { $(this).prop('checked', false) });
    hideallRenewalnotrequiredDiv();
    showallcheckbox();
    clearCheckboxes();
}


function clearcallmelater() {
    $('#txtfollowupdate').val('');
    $('#txtfollowuptime').val('');
    $('#textareaCRERemarks').val('');
    $('#textareaCustomerRemarks').val('');
    $('#txt-reasonforfollowup').val('');
}

function clearnoncaontact() {
    const chbx = document.getElementsByName("insudisposition.dispoNotTalk");

    for (let i = 0; i < chbx.length; i++) {
        chbx[i].checked = false;
    }
}

function clearModeofAppointmnet() {
    const chbx = document.getElementsByName("appointbooked.modeofappointment");

    for (let i = 0; i < chbx.length; i++) {
        chbx[i].checked = false;
    }
}
function clearappointmnettype() {
    const chbx = document.getElementsByName("appointbooked.typeOfPickup");

    for (let i = 0; i < chbx.length; i++) {
        chbx[i].checked = false;
    }
}

function clearwhatcustsays() {
    const chbx = document.getElementsByName("insudisposition.dispoCustAns");

    for (let i = 0; i < chbx.length; i++) {
        chbx[i].checked = false;
    }
}
function clearBookAppointment() {
    $('#walkinLocation').prop('selectedIndex', 0);
    //$('#appointbooked_pincodeValue').val('');
    //$('#appointbooked_pincodeValue').val('');
    $('#txtappointmentDate').val('');
    $('#txtappointmentTime').val('');
    $('#textareaCRERemarks').val('');
    $('#textareaCustomerRemarks').val('');
}

function hiderenewalnotrequireDiv(value) {
    if (value == "1") {
        $('#INSAlreadyServicedDiv').hide();
        $('#INSVehicleSoldDiv').hide();
        $('#INSDissatisfiedwithpreviousserviceSiv').hide();
        $('#INSDistancefromDiv').hide();
        $('#INSDissatisfiedwithSalesDiv').hide();
        $('#INSDissatisfiedwithInsuranceDiv').hide();
        $('#INSExcBillingIdDiv').hide();
        $('#INSStolenDiv').hide();
        $('#INSTotallossDiv').hide();
        $('#INSOtherLastDiv').hide();
    }
    else if (value == "2") {
        $('#INSAlreadyServicedDiv').show();
        $('#INSVehicleSoldDiv').show();
        $('#INSDissatisfiedwithpreviousserviceSiv').show();
        $('#INSDistancefromDiv').show();
        $('#INSDissatisfiedwithSalesDiv').show();
        $('#INSDissatisfiedwithInsuranceDiv').show();
        $('#INSExcBillingIdDiv').show();
        $('#INSStolenDiv').show();
        $('#INSTotallossDiv').show();
        $('#INSOtherLastDiv').show();
    }
}
/* Renewal Not Required */

$('#AlreadyServiced').click(function () {
    if ($(this).prop('checked'))
    {
        hideallcheckbox();
        hideallRenewalnotrequiredDiv
        $(".AlreadyServiced").show();
        $("#alreadyservicedDiv1").show();

    }
    else {
        showallcheckbox();
        hideallRenewalnotrequiredDiv();
        $('#btnsubmitDispositionDIV').hide();
    }
});


$('#VehicleSold').click(function () {

    if ($(this).prop('checked')) {
        hideallcheckbox();
        hideallRenewalnotrequiredDiv
        $(".VehicleSold").show();
        $("#VehicelSoldYesRNo").show();
    }
    else {
        showallcheckbox();
        hideallRenewalnotrequiredDiv();
        $('#btnsubmitDispositionDIV').hide();
    }
});
$('#ExcBillingId').click(function () {

    if ($(this).prop('checked')) {
        hideallcheckbox();
        hideallRenewalnotrequiredDiv
        $(".ExcBillingId").show();
    }
    else {
        showallcheckbox();
        hideallRenewalnotrequiredDiv();
        $('#btnsubmitDispositionDIV').hide();
    }
});
$('#Totalloss').click(function () {

    if ($(this).prop('checked')) {
        hideallcheckbox();
        hideallRenewalnotrequiredDiv
        $(".Totalloss").show();
    }
    else {
        showallcheckbox();
        hideallRenewalnotrequiredDiv();
        $('#btnsubmitDispositionDIV').hide();
    }
});
$('#Distancefrom').click(function () {
    if ($(this).prop('checked'))
    {

        $("#DistancefromDealerLocationDIV").show();
        hideallcheckbox();
        hideallRenewalnotrequiredDiv
        $(".Distancefrom").show();

    } else {
        showallcheckbox();
        hideallRenewalnotrequiredDiv();
        $('#btnsubmitDispositionDIV').hide();

    }

});
$('#DissatisfiedwithInsuranceId').click(function () {
    if ($(this).prop('checked'))
    {
        hideallcheckbox();
        hideallRenewalnotrequiredDiv
        $(".DissatisfiedwithInsuranceId").show();

    } else {
        showallcheckbox();
        hideallRenewalnotrequiredDiv();
        $('#btnsubmitDispositionDIV').hide();
    }

});
$('#DissatisfiedwithSalesID').click(function () {
    if ($(this).prop('checked'))
    {
        hideallcheckbox();
        hideallRenewalnotrequiredDiv
        $(".DissatisfiedwithSalesID").show();

    } else {
        showallcheckbox();
        hideallRenewalnotrequiredDiv();
        $('#btnsubmitDispositionDIV').hide();
    }

});
$('#Dissatisfiedwithpreviousservice').click(function () {
    if ($(this).prop('checked'))
    {
        hideallcheckbox();
        hideallRenewalnotrequiredDiv
        $(".Dissatisfiedwithpreviousservice").show();

    } else {
        showallcheckbox();
        hideallRenewalnotrequiredDiv();
        $('#btnsubmitDispositionDIV').hide();
    }

});
$('#renewalNotRequiredReason').click(function () {
    if ($(this).prop('checked'))
    {
        hideallcheckbox();
        hideallRenewalnotrequiredDiv
        $(".renewalNotRequiredReason").show();

    } else {
        showallcheckbox();
        hideallRenewalnotrequiredDiv();
        $('#btnsubmitDispositionDIV').hide();
    }

});
$('#Stolen').click(function () {
    if ($(this).prop('checked'))
    {
        hideallcheckbox();
        hideallRenewalnotrequiredDiv
        $(".Stolen").show();

    } else {
        showallcheckbox();
        hideallRenewalnotrequiredDiv();
        $('#btnsubmitDispositionDIV').hide();
    }

});
$('#Other').click(function () {
    if ($(this).prop('checked'))
    {
        hideallcheckbox();
        hideallRenewalnotrequiredDiv
        $(".OtherLast").show();
        $("#OtherSeriveRemarks").show();

    } else {
        showallcheckbox();
        hideallRenewalnotrequiredDiv();
        $('#btnsubmitDispositionDIV').hide();
    }

});


function showallcheckbox() {
    $(".VehicleSold").show();
    $(".AlreadyServiced").show();
    $(".ExcBillingId").show();
    $(".Totalloss").show();
    $(".Distancefrom").show();
    $(".DissatisfiedwithInsuranceId").show();
    $(".DissatisfiedwithSalesID").show();
    $(".Dissatisfiedwithpreviousservice").show();
    $(".Stolen").show();
    $(".OtherLast").show();

}
function hideallcheckbox() {
    $(".VehicleSold").hide();
    $(".AlreadyServiced").hide();
    $(".ExcBillingId").hide();
    $(".Totalloss").hide();
    $(".Distancefrom").hide();
    $(".DissatisfiedwithInsuranceId").hide();
    $(".DissatisfiedwithSalesID").hide();
    $(".Dissatisfiedwithpreviousservice").hide();
    $(".Stolen").hide();
    $(".OtherLast").hide();
    $('#btnsubmitDispositionDIV').show();

}

function hideallRenewalnotrequiredDiv() {

    $("#alreadyservicedDiv1").hide();
    $("#ServicedAtOtherDealerDiv").hide();
    $("#NonAutorizedworkshopDiv").hide();
    $("#AutorizedworkshopDIV").hide();

    $('#PurchaseClickYes').hide();

    $("#VehicelSoldYesRNo").hide();
    $("#VehicleSoldClickYes").hide();
    $("#VehicleSoldClickNo").hide();

    $("#DistancefromDealerLocationDIV").hide();

    $("#OtherSeriveRemarks").hide();
}

$("input[name$='insudisposition.renewalDoneBy']").click(function () {
    varAlreadyservicedadio = $(this).val();

    if (varAlreadyservicedadio == "Renewed At My Dealer") {
        $('#dateofrnwal').val('');
        $('#dateofrennonauth').val('');
        $('#insuranceProvidedByOEM').prop('selectedIndex', 0);
        $('#inBoundLeadSourceSelectVal').prop('selectedIndex', 0);
        $('#insuranceProvidedUnAuth').prop('selectedIndex', 0);




       // $('#ServicedMyDealerDiv').show();
        $('#ServicedAtOtherDealerDiv').hide();
    }
    else if (varAlreadyservicedadio == "Renewed At Other Dealer") {
        $('#insudisposition_premimum').val('');
        $('#insudisposition_coverNoteNo').val('');
        $('#insuranceProvidedBy').prop('selectedIndex', 0);
        $('#ServicedAtOtherDealerDiv').show();
       // $('#ServicedMyDealerDiv').hide();
    }
    else {
        $('#insudisposition_premimum').val('');
        $('#insudisposition_coverNoteNo').val('');
        $('#insuranceProvidedBy').prop('selectedIndex', 0);


        $('#dateofrnwal').val('');
        $('#dateofrennonauth').val('');
        $('#insuranceProvidedByOEM').prop('selectedIndex', 0);
        $('#inBoundLeadSourceSelectVal').prop('selectedIndex', 0);
        $('#insuranceProvidedUnAuth').prop('selectedIndex', 0);

        $('#InfoNotAvailable').hide();
       // $('#ServicedMyDealerDiv').hide();
        $('#ServicedAtOtherDealerDiv').hide();
    }
    clearrenewedatotherdealer();
    clearrenewedatmydealer();
});
$("input[name='insudisposition.typeOfAutherization']").click(function () {
    varServicedAtOtherDealer = $(this).val();

    if (varServicedAtOtherDealer == "OTHERS") {
         $('#NonAutorizedworkshopDiv').show();
         $('#AutorizedworkshopDIV').hide();
        $('.CheckedwithDMS').hide();
        clearrenewedatmydealer();
    }
    else if (varServicedAtOtherDealer == "Dealer") {
        $('#AutorizedworkshopDIV').show();
        $('#NonAutorizedworkshopDiv').hide();
        $('.CheckedwithDMS').hide();
        clearrenewedatotherdealer();
     }
    // else {
    //    $('#AutorizedworkshopDIV').hide();
    //    $('#NonAutorizedworkshopDiv').hide();
    //    $('.CheckedwithDMS').hide();
    //}
});

$('.timepicker_7').timepicker({
    showPeriod: false, // 24 hours formate
    onHourShow: timepicker7OnHourShowCallback,
    showPeriodLabels: false,

});
$('#appointmentTime1').timepicker({
    showPeriod: false, // 24 hours formate
    onHourShow: timepicker7OnHourShowCallback,
    showPeriodLabels: false,
    defaultTime: '12:00',
    dynamic: true,
});
function timepicker7OnHourShowCallback(hour) {
    if ((hour > 20) || (hour < 8)) {
        return false;
    }
    return true;
}

$('.timePickRange7to19').timepicker({
    showPeriod: false, // 24 hours formate
    onHourShow: timepick,
    showPeriodLabels: false,

});

function timepick(hour) {
    if ((hour > 19) || (hour < 7)) {
        return false;
    }
    return true;
}



$("#btnsubmitdisposition").click(function () {
    var fieldAddress = $('#txtfieldaddress').val();
    var walkinlocation = $('#walkinLocation').val();
    var pincode = $('#appointbooked_pincodeValue').val();
    var appointmentDate = $('#txtappointmentDate').val();
    var appointmentTime = $('#txtappointmentTime').val();
    var followupdate=$('#txtfollowupdate').val();
    var follwuptime=$('#txtfollowuptime').val();
    var creremarks = $('#textareaCRERemarks').val();
    var customerremarks = $('#textareaCustomerRemarks').val();
    var reasonForFollowUp = $('#txt-reasonforfollowup').val();

    var typeofdispo = $("input[name='insudisposition.typeOfDisposition']:checked").val();
    if (typeofdispo == "NonContact") {
        var chkincSubmit = 0;
        $('[name="insudisposition.dispoNotTalk"]').each(function () {
            if ($(this).is(':checked')) chkincSubmit++;
        });
        if (chkincSubmit == 0) {
            Lobibox.notify('warning', {
                continueDelayOnInactiveTab: true,
                msg: 'Please Select one Reason.'
            });
            return false;


        } else
        {
            var noncontactreason = $("input[name='insudisposition.dispoNotTalk']:checked").val();

            if (noncontactreason == "Other")
            {
                var textNoOthers = $('#nonContactOther').val();
                if (textNoOthers == "") {
                    Lobibox.notify('warning', {
                        continueDelayOnInactiveTab: true,
                        msg: 'Please Specify the Remarks.'
                    });
                    return false;
                }
            }

        }
    }
    else {
        var custAns = $("input[name='insudisposition.dispoCustAns']:checked").val();
        if (custAns == "Book Appointment") {
            var modeofappointment = $("input[name='appointbooked.modeofappointment']:checked").val();
            if (modeofappointment == "Online") {
                if (appointmentDate == "") {
                    Lobibox.notify('warning', {
                        continueDelayOnInactiveTab: true,
                        msg: 'Please Provide Appointment Date.'
                    });

                    return false
                }
                if (appointmentTime == "") {
                    Lobibox.notify('warning', {
                        continueDelayOnInactiveTab: true,
                        msg: 'Please Provide Appointment Time.'
                    });

                    return false
                }

            }
            else if (modeofappointment == "Offline") {
                var typeOfPickup = $("input[name='appointbooked.typeOfPickup']:checked").val();
                if (typeOfPickup == "Field") {
                    if (fieldAddress == "") {
                        Lobibox.notify('warning', {
                            continueDelayOnInactiveTab: true,
                            msg: 'Please Provide Address.'
                        });

                        return false
                    }
                    if (pincode == "") {
                        Lobibox.notify('warning', {
                            continueDelayOnInactiveTab: true,
                            msg: 'Please Provide Pincode.'
                        });

                        return false
                    }
                    if (appointmentDate == "") {
                        Lobibox.notify('warning', {
                            continueDelayOnInactiveTab: true,
                            msg: 'Please Provide Appointment Date.'
                        });

                        return false
                    }
                    if (appointmentTime == "") {
                        Lobibox.notify('warning', {
                            continueDelayOnInactiveTab: true,
                            msg: 'Please Provide Appointment Time.'
                        });

                        return false
                    }

                }
                else if (typeOfPickup == "Walk-in") {
                    //if (walkinlocation == "") {
                    //    Lobibox.notify('warning', {
                    //        continueDelayOnInactiveTab: true,
                    //        msg: 'Please Provide Walk-In location.'
                    //    });
                    //    return false;
                    //}
                    if (appointmentDate == "") {
                        Lobibox.notify('warning', {
                            continueDelayOnInactiveTab: true,
                            msg: 'Please Provide Appointment Date.'
                        });

                        return false
                    }
                    if (appointmentTime == "") {
                        Lobibox.notify('warning', {
                            continueDelayOnInactiveTab: true,
                            msg: 'Please Provide Appointment Time.'
                        });

                        return false
                    }
                }
            }
        }
        else if (custAns == "Renewal Not Required") {
            var renewalnotRequiredreason = $('#chkrenewalNotRequired').val();


            if (renewalnotRequiredreason == "") {
                Lobibox.notify('warning', {
                    continueDelayOnInactiveTab: true,
                    msg: 'Please Select Any One .'
                });
                return false;
            }
            if (renewalnotRequiredreason == "Other Service") {
                var otherreason = $('#commentsOtherRemarks').val();
                if (otherreason == "") {
                    Lobibox.notify('warning', {
                        continueDelayOnInactiveTab: true,
                        msg: 'Please Select Reason .'
                    });
                    return false;
                }
            }
        }
        else if (custAns == "ConfirmedInsu") {

        }
        else if (custAns == "Cancel Appointment") {
            if ($('#rdoinscancelAppointment').is(':checked')) {
            }
            else {
                Lobibox.notify('warning', {
                    continueDelayOnInactiveTab: true,
                    msg: 'Please Confirm to Cancel Appointment.'
                });
                return false;
            }


        }
        else if (custAns == "Call Me Later") {
            if (followupdate == "") {
                Lobibox.notify('warning', {
                    continueDelayOnInactiveTab: true,
                    msg: 'Please Provide Followup Date.'
                });

                return false
            }
            if (follwuptime == "") {
                Lobibox.notify('warning', {
                    continueDelayOnInactiveTab: true,
                    msg: 'Please Provide Followup Time.'
                });

                return false
            }

            if (reasonForFollowUp == "" || reasonForFollowUp == null) {
                Lobibox.notify('warning', {
                    continueDelayOnInactiveTab: true,
                    msg: 'Please Select Reason for Follow up.'
                });

                return false
            }
        }
    }
});

$("input[name='insudisposition.dispoNotTalk']").click(function () {
    var mNoOthre = $(this).val();
    if (mNoOthre == "Other") {
        $("#NoOthers").show();
    } else {

        $("#NoOthers").hide();
    }
});


$("input[name='insudisposition.renewalDoneBy']").click(function () {


    const chbx = document.getElementsByName("insudisposition.typeOfAutherization");

    for (let i = 0; i < chbx.length; i++) {
        chbx[i].checked = false;
    }
    $('#AutorizedworkshopDIV').hide();
    $('#NonAutorizedworkshopDiv').hide();
    $('.CheckedwithDMS').hide();
});



$("input[name='listingForm.VehicleSoldYes']").click(function () {
    vehisold = $(this).val();

    const chbx = document.getElementsByName("listingForm.PurchaseYes");

    for (let i = 0; i < chbx.length; i++) {
        chbx[i].checked = false;
    }
    if (vehisold == "VehicleSold Yes")
    {
        $('#VehicleSoldClickYes').show();
        $('#VehicleSoldClickNo').show();
        $('#PurchaseClickYes').hide();
        
    }
    else if (vehisold == "VehicleSoldYes No")
    {
        $('#VehicleSoldClickYes').hide();
        $('#VehicleSoldClickNo').show();
        $('#PurchaseClickYes').hide();

    }
    clearcontactDetails();
    clearpurchaseyes();
});
$("input[name='listingForm.PurchaseYes']").click(function () {
    purchase = $(this).val();
    if (purchase == "Purchase Yes")
    {
        $('#PurchaseClickYes').show();
    }
    else if (purchase == "Purchase No")
    {
        $('#PurchaseClickYes').hide();
        clearpurchaseyes();
    }
});


function clearCheckboxes() {

    const chbx = document.getElementsByName("insudisposition.renewalDoneBy");

    for (let i = 0; i < chbx.length; i++) {
        chbx[i].checked = false;
    }
    const chbx1 = document.getElementsByName("insudisposition.typeOfAutherization");

    for (let i = 0; i < chbx1.length; i++) {
        chbx1[i].checked = false;
    }
    const chbx2 = document.getElementsByName("listingForm.VehicleSoldYes");

    for (let i = 0; i < chbx2.length; i++) {
        chbx2[i].checked = false;
    }
    
    $('#textareaCRERemarks').val('');
    $('#textareaCustomerRemarks').val('');
    $('#commentsOtherRemarks').val('');
    clearpurchaseyes();
    clearrenewedatmydealer();
    clearrenewedatotherdealer();
    clearcitydistance();
    clearcontactDetails();
}


function clearpurchaseyes() {
    $('#vehicleRegNo').val('');
    $('#chassisNo').val('');
    $('#variant').val('');
    $('#model').val('');
    $('#saleDate').val('');
    $('#dealershipName').val('');
}
function clearrenewedatmydealer() {
    $('#dealerNameId').val('');
    $('#dateofrnwal').val('');
    $('#insuranceProvidedByOEM').prop('selectedIndex', 0);
}
function clearrenewedatotherdealer() {
    $('#dateofrennonauth').val('');
    $('#insuranceProvidedUnAuth').val('');
}
function clearcitydistance() {
        $('#citydistance').val('');

}
function clearcontactDetails() {

    $('#customerFNameConfirm').val('');
    $('#customerLNameConfirm').val('');
    $('#Mobile1').val('');
    $('#Mobile2').val('');
    $('#STDCodeInput').val('');
    $('#LandlineInput').val('');
    $('#addreline1').val('');
    $('#addreline2').val('');
    $('#PinInput').val('');
    $('#stateInput').prop('selectedIndex', 0);
    $('#cityInput').prop('selectedIndex', 0);
}

function adddeletecontact(value) {
    if (value == "add") {
        $('#addphoneno').show();
        $("#savePhoneCust").prop("value", "Save");
        $('#lblremarks').text("Reason for adding");
        $('#ButtonName').removeAttr('onclick');
        $('#savePhoneCust').attr('onClick', 'savecustomerDetails("SavePhone");');

    }
    else if (value == "delete") {
        $('#addphoneno').hide();
        $("#savePhoneCust").prop("value", "Delete");

        $('#lblremarks').text("Reason for Deleting");
        $('#ButtonName').removeAttr('onclick');
        $('#savePhoneCust').attr('onClick', 'savecustomerDetails("DeletePhone");');

    }
    $('#adddeletephonenumberDiv').show();

}
function addemail(value) {
    if (value == "add") {
        $('#addemailDiv').show();

    }
}
$("#myPhoneNum").keypress(function (e) {
    var key = e.keyCode || e.which;
    console.log(key + "");
    if (/*(key >= 97 && key <= 122) || (key >= 65 && key <= 90) ||*/ (key >= 48 && key <= 57) || key == 32) {
        //No Action
    }
    else {
 

        return false;
    }
});

$('#myEmailNum').blur(function () {
    var customerEmail = $(this).val();
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,3}))$/igm;
    if (re.test(customerEmail)) {

    } else {
        $('#myEmailNum').val('');
        Lobibox.notify('warning', {
            continueDelayOnInactiveTab: true,
            msg: 'Invalid Email Address.'
        });
        return false;
    }
});

function reset() {
    $('#newAddress').val('');
    $('#ppincode').val('');
    //$('#paddrrCity').prop('selectedIndex', 0);
    $('#paddrrCity').empty();
    //$('#paddrrCity').attr('disabled', true);
    $('#pstateId option:first').prop('selected', true);
    $('#IsPrimary').prop('checked', false);
}


function hidepopupdiv() {
    $('#addemailDiv').hide();
    $('#adddeletephonenumberDiv').hide();
    
}

$(".onlyAlphabetOnly").keypress(function (a) {
    var b = a.charCode;
    b >= 65 && b <= 120 || 32 == b || 0 == b || a.preventDefault()
})