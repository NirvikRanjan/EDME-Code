function clearFilter() {
    $('#ddldealercity').multiselect("deselectAll", false).multiselect("refresh");
    $('#ddldealerstate').multiselect("deselectAll", false).multiselect("refresh");
    $('#ddldealerzone').multiselect("deselectAll", false).multiselect("refresh");
    $('#ddldealername').multiselect("deselectAll", false).multiselect("refresh");
    $('#ddlCampaignid').multiselect("deselectAll", false).multiselect("refresh");
    $('#ddlrenewalType').multiselect("deselectAll", false).multiselect("refresh");
}
function ReloadInsuranceData() {
    if ($.fn.DataTable.isDataTable("#tblnationalwise")) {
        var table = $("#tblnationalwise").DataTable();
        table.clear();
        table.destroy();
        $("#tblnationalwise thead th").remove();
        calllivereportProcedure(1, 'tblnationalwise');
    }


    if ($.fn.DataTable.isDataTable("#tblzonewise")) {
        var table = $("#tblzonewise").DataTable();
        table.clear();
        table.destroy();
        $("#tblzonewise thead th").remove();
        calllivereportProcedure(2, 'tblzonewise');
    }

    if ($.fn.DataTable.isDataTable("#tblstatewise")) {
        var table = $("#tblstatewise").DataTable();
        table.clear();
        table.destroy();
        $("#tblstatewise thead th").remove();
        calllivereportProcedure(3, 'tblstatewise');
    }

    if ($.fn.DataTable.isDataTable("#tblcitywise")) {
        var table = $("#tblcitywise").DataTable();
        table.clear();
        table.destroy();
        $("#tblcitywise thead th").remove();
        calllivereportProcedure(4, 'tblcitywise');
    }
    if ($.fn.DataTable.isDataTable("#tbldealerwise")) {
        var table = $("#tbldealerwise").DataTable();
        table.clear();
        table.destroy();
        $("#tbldealerwise thead th").remove();
        calllivereportProcedure(5, 'tbldealerwise');
    }
    if ($.fn.DataTable.isDataTable("#tbcampaignwise")) {
        var table = $("#tbcampaignwise").DataTable();
        table.clear();
        table.destroy();
        $("#tbcampaignwise thead th").remove();
        calllivereportProcedure(6, 'tbcampaignwise');
    }
    if ($.fn.DataTable.isDataTable("#tblpolicytypewise")) {
        var table = $("#tblpolicytypewise").DataTable();
        table.clear();
        table.destroy();
        $("#tblpolicytypewise thead th").remove();
        calllivereportProcedure(7, 'tblpolicytypewise');
    }
    if ($.fn.DataTable.isDataTable("#tblcrewise")) {
        var table = $("#tblcrewise").DataTable();
        table.clear();
        table.destroy();
        $("#tblcrewise thead th").remove();
        calllivereportProcedure(8, 'tblcrewise');
    }
}


function calllivereportProcedure(Id, tblId) {
    var DealerCity = $('#ddldealercity option:selected').toArray().map(item => item.value).join();
    var DealerState = $('#ddldealerstate option:selected').toArray().map(item => item.value).join();
    var DealerZone = $('#ddldealerzone option:selected').toArray().map(item => item.value).join();
    var DealerName = $('#ddldealername option:selected').toArray().map(item => item.value).join();
    var campaignName = $('#ddlCampaignid option:selected').toArray().map(item => item.value).join();
    var renewalTypeName = $('#ddlrenewalType option:selected').toArray().map(item => item.value).join();
    var reportId = Id;
    var parameters = { DealerCity: DealerCity, DealerState: DealerState, reportId: reportId, DealerZone: DealerZone, DealerName: DealerName, campaignName: campaignName, renewalTypeName: renewalTypeName };
    if (!$.fn.DataTable.isDataTable("#" + tblId)) {
        {
            $("#" + tblId + "_button").hide();
            callLiveTable(parameters, tblId);
        }
    }
}
function getableData(parameters) {
    return $.ajax({
        "url": siteRoot + "/liveReports/getLiveReports",
        "type": "POST",
        "data": { reportParameter: JSON.stringify(parameters) },
        "datatype": "json"
    });
}

function callLiveTable(parameters, tblId) {
    $('#' + tblId + '_Spinner').show();
    getableData(parameters).done(function (records) {
        var jsonrecords = JSON.parse(records.data);
        var my_columns = [];
        if (records.exception === "" && jsonrecords.length > 0) {
            $.each(jsonrecords[0], function (key, value) {
                var my_item = {};
                my_item.data = key;
                my_item.title = key;
                my_columns.push(my_item);
            });

            $('#' + tblId).DataTable({
                data: jsonrecords,
                "columns": my_columns,
                "paging": false,
                "searching": false,
                "order": [[0, "asc"]],
                "pageLength": 50,
                "responsive":  true,
                columnDefs: [
                    { targets: [1,2, 5, 7,9], className: 'dt-body-right' }
                ],
                "initComplete": function (settings, exception) {
                    $('#' + tblId + '_Spinner').hide();
                    $("#" + tblId + "_button").show();
                }

            });
            var buttons = new $.fn.dataTable.Buttons('#' + tblId, {
                buttons: [
                    {
                        extend: 'csvHtml5',
                        text: '<i class="fa fa-download">&nbsp;&nbsp;Excel</i>',
                        titleAttr: 'Excel',
                        filename: tblId

                    }
                ]
            }).container().appendTo($('#' + tblId + '_xl'));
        }
        else {
            var dataset = [{ "Result": "No Data Available" }];

            var my_columns = [];
            $.each(dataset[0], function (key, value) {
                var my_item = {};
                my_item.data = key;
                my_item.title = key;
                my_columns.push(my_item);
            });

            $('#' + tblId).DataTable({
                data: dataset,
                "columns": my_columns,
                "paging": false,
                "searching": false,
                "bLengthChange": false,
                "bInfo": false,
                "order": [[0, "asc"]],
                "pageLength": 50,
                "initComplete": function (settings, exception) {
                    $("#" + tblId+" thead").remove()
                    $('#' + tblId + '_Spinner').hide();
                    $("#" + tblId + "_button").show();
                }
            });


            $('#tblException').text(records.exception);
        }

    });
}
