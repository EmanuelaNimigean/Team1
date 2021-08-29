$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/userMessageHub").build();

    connection.start().then(function () {
        console.log("signalr connected");
    });

    connection.on("UserRoleChanged", roleChanged);
});

function roleChanged(id, oldRole, newRole){
    $(`td[user-role=${id}]`).text(newRole);

    var menu = $(`div[user-dropdown-menu=${id}]`);
    menu.find(`a[role-name=${newRole}]`).remove();
    menu.append(`<a class="dropdown-item" role-name=${oldRole} href="/Users/AssignRole/${id}?currentRole=${newRole}&amp;newRole=${oldRole}">${oldRole}</a>`);
}