$(function () {
    let ddClient = $("#ClientId");
    ddClient.empty();
    ddClient.append($('<option></option>').val(0).html(' --- clerar --- '));
    ddClient.prop('disabled', true);
    $.ajax({
        url: "/Clients/getClientList/",
        type: "GET",
        success: (jsonReslut) => {
            $.each(jsonReslut, (key, item) => {
                ddClient.append($('<option></option>').val(item.clientId).html(item.name));
            });
            ddClient.val('');
            ddClient.data('placeholder', ' [ Select Client ] ');
            ddClient.prop('disabled', false);
        }
    });
    ddClient.change(clientSelect);
});

function clientSelect() {
    let ddClient = $("#ClientId");
    $('#invoiceTable').hide(500, () => {
        if (ddClient.val() === '0') {
            ddClient.val('');
            $('#clientName').val('').data('source', '');
            $('#clientAddress').val('').data('source', '');
            $('#clientCity').val('').data('source', '');
            $('#clientState').val('').data('source', '');
            $('#clientZip').val('').data('source', '');
            $('#clientPhone').val('').data('source', '');
            $('#clientEmail').val('').data('source', '');
        } else {
            getClientRecord();
        }
    });
    
}

function getClientRecord() {
    let ddClient=$('#ClientId');
    let clientId = ddClient.val();
    $.ajax({
        url: "/Clients/getClientById/",
        type: "GET",
        data: { id: clientId },
        contentType: 'application/json; charset=utf-8',
        success: (jsonReslut) => {
            if (jsonReslut) {
                $('#clientName').val(jsonReslut[0].name).data('source', jsonReslut[0].name);
                $('#clientAddress').val(jsonReslut[0].address).data('source', jsonReslut[0].address);
                $('#clientCity').val(jsonReslut[0].city).data('source', jsonReslut[0].city);
                $('#clientState').val(jsonReslut[0].state).data('source', jsonReslut[0].state);
                $('#clientZip').val(jsonReslut[0].zip).data('source', jsonReslut[0].zip);
                $('#clientPhone').val(jsonReslut[0].phone).data('source', jsonReslut[0].phone);
                $('#clientEmail').val(jsonReslut[0].email).data('source', jsonReslut[0].email);
                loadInvoices();
            } else {
                $('#clientName').val('').data('source', '');
                $('#clientAddress').val('').data('source', '');
                $('#clientCity').val('').data('source', '');
                $('#clientState').val('').data('source', '');
                $('#clientZip').val('').data('source', '');
                $('#clientPhone').val('').data('source', '');
                $('#clientEmail').val('').data('source', '');
            }
        }
    });
}

function loadInvoices() {
    let ddClient = $('#ClientId');
    let clientId = ddClient.val();
    if ($('#invoiceTableBody') != null) { $('#invoiceTableBody').empty(); }
    let dTBody = $('#invoiceTableBody');
    $.ajax({
        url: "/Clients/getInvoicesByClient/",
        type: "GET",
        data: { id: clientId },
        success: (jsonReslut) => {
            if (jsonReslut !== null && jsonReslut.length > 0) {
                dTBody.append('<tr><th class="col-md-1"> </th> <th class="col-md-2">Invoice #</th> <th class="col-md-3">Date</th> <th class="col-md-2">Invoice Total</th> <th class="col-md-2">Status</th><th class="col-md-2"></th> </tr>');
                $.each(jsonReslut, (key, item) => {
                    dTBody.append('<tr data-invoiceId="' + item.invoiceId + '"><td><i class="btn glyphicon glyphicon-plus text-success" data-command="expand" id="bInvoiceDetail' + item.invoiceId + '"> </i></td>' +
                        '<td class="col-md-2">' + item.invoiceNumber +
                        '</td> <td class="col-md-3">'+ item.invoiceDate + 
                        '</td> <td class="col-md-2">' + item.invoiceTotal +
                        '</td> <td class="col-md-2">' + item.invoiceStatus +
                        '</td> <td class="col-md-2"><button class="btn btn-sm btn-success" id="bInvoiceEdit" onclick="window.location.href=\'/Invoices/Edit/' + item.invoiceId + '\'"><i class="glyphicon glyphicon-pencil"> </i> Edit</button> <button class="btn btn-sm btn-danger" id="bInvoiceDetete" onclick="window.location.href=\'/Invoices/Delete/' + item.invoiceId + '\'"><i class="glyphicon glyphicon-trash"> </i> Delete</button></td> </tr>');
                });
            }
            $('[id^=bInvoiceDetail]').click(loadLineItems);
            $('#invoiceTable').show(500);
        }
    });
}

function loadLineItems(e) {
    let vPlus = $(this);

    let vRow = vPlus.parent().parent();
    const iInvoiceId = vRow.data('invoiceid');
    let sCommand = vPlus.data('command');
    let lineRowId = "liId" + iInvoiceId.toString();
    if (sCommand === 'expand') {
        if ($("#detail" + iInvoiceId.toString()).length === 0) {
        $.ajax({
            url: "/Clients/getLineItemsByInvoice/",
            type: "GET",
            data: { id: iInvoiceId },
            success: (jsonReslut) => {
                let sInsert = '<tr id="detail' + iInvoiceId + '" style="display:none" ><td colspan="6"><div class="panel panel-default"> <table class="table table-condensed table-bordered col-md-12"> <tbody> <tr><th class="col-md-1">Line #</th><th class="col-md-4">Desctiption</th><th class="col-md-1">Qty</th><th class="col-md-2">Unit Price</th><th class="col-md-2">Line Total</th><th class="col-md-1"></th></tr>';
                if (jsonReslut != null && jsonReslut.length > 0) {
                    $.each(jsonReslut, (key, item) => {
                        sInsert += '<tr> ';
                        sInsert += '<td>' + item.lineNumber + '</td>';
                        sInsert += '<td>' + item.descriptin + '</td>';
                        sInsert += '<td>' + item.qty + '</td>';
                        sInsert += '<td>' + item.unitPrice + '</td>';
                        sInsert += '<td>' + item.lineTotal + '</td>';
                        sInsert += '<td><button class="btn btn-sm btn-success" id="bInvoiceEdit" onclick="window.location.href=\'/LineItems/Edit/' + item.lineId + '\'"><i class="glyphicon glyphicon-pencil"> </i></button> <button class="btn btn-sm btn-danger" id="bInvoiceDetete" onclick="window.location.href=\'/LineItems/Delete/' + item.lineId + '\'"><i class="glyphicon glyphicon-trash"> </i></button></td>';
                        sInsert += '</tr>';
                    });
                }
                sInsert += '<tr> <td colspan=6><div class="panel"><button class="btn btn-sm btn-info" onclick="window.location.href=\'/LineItems/Create/' + iInvoiceId + '\'" >ADD LINE ITEM</button></div></td></tr>';
                sInsert += '</tbody> </table></div></td> </tr> ';
                $(sInsert).insertAfter(vRow).show(600, () => {
                    vPlus.removeClass('glyphicon-plus').removeClass('text-success').addClass('text-danger').addClass('glyphicon-minus').data('command', 'collapse');
                });

            }
        });
        }
    }
    if (sCommand === 'collapse') {
        $('#detail' + iInvoiceId).hide(600).remove();
        vPlus.removeClass('glyphicon-minus').removeClass('text-danger').addClass('text-success').addClass('glyphicon-plus').data('command', 'expand');
    }
    
    //$('<tr><td></td> <td colspan="4"> <div class="panel panel-default"><h5>&nbsp;&nbsp; Line Items</h5> <table class="table table-condensed table-bordered col-md-12"> <tbody> <tr> <th class="col-md-1">Line #</th> <th class="col-md-6">Desctiption</th> <th class="col-md-1">Qty</th> <th class="col-md-2">Unit Price</th> <th class="col-md-2">Line Total</th> </tr> <tr> <td>1</td> <td>2</td> <td>3</td> <td>4</td> <td>5</td> </tr> <tr> <td>1</td> <td>2</td> <td>3</td> <td>4</td> <td>5</td> </tr> <tr> <td>1</td> <td>2</td> <td>3</td> <td>4</td> <td>5</td> </tr> </tbody> </table><hr /><button class="btn btn-sm">Add Line Item</button> </div>  </td> </tr>').insertAfter(vRow);
}

//function getOnLineStat() {
//    var tImg = document.getElementsByClassName("Qgzj8 gqwaM");
//    var sStatSpan = document.getElementsByClassName("O90ur");
//    var nNameDiv = document.getElementsByClassName("_2zCDG");
//    var nNameSpan = nNameDiv.childNodes[0];
//    if (sStatSpan && nNameSpan.getAttribute("title") == "Majka") {
//        let today = new Date();
//        let dd = today.getDate();
//        let mm = today.getMonth() + 1;
//        let yy = today.getFullYear();
//        let HH = today.getHours();
//        let MM = today.getMinutes();
//        console.log(mm & '/' & dd & '' & HH & ':' & MM)
//        tImg[0].setAttribute('title', 'Majka ostatnio byłą tutaj: ' + mm + '/' + dd + ' at  ' + HH + ':' + MM);
//    }
//}