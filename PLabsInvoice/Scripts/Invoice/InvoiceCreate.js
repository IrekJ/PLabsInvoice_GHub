$(function () {
    let ddClient = $("#ClientId");
    //ddClient.empty();
    $.ajax({
        url: "/Clients/getClientList/",
        type: "GET",
        success: (jsonReslut) => {
            $.each(jsonReslut, (key, item) => {
                ddClient.append($('<option></option>').val(item.clientId).html(item.name));
            })
            ddClient.prop('selectedIndex', 0);
        }
    })
    
})