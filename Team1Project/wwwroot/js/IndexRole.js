$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/userMessageHub").build();

    connection.start().then(function () {
        console.log("signalr connected");
    });

    connection.on("RoleCreated", roleCreated);
});

function roleCreated(id, name) {
    $("#roles").append(`<tr>
                <td>${id}</td>
                <td>${name}</td>
            </tr>`);
}