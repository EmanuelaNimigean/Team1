$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/internMessageHub").build();

    connection.start().then(function () {
        console.log("signalr connected");
    });

    connection.on("InternAdded", createNewIntern);
    connection.on("InternDeleted", deleteIntern);
    connection.on("InternUpdated", updateIntern);
});

function createNewIntern(id, name, birthDate, emailAddress, githubUsername, teamId) {
    $("#internTableBodyId").append(`<tr intern-id="${id}">
                <td intern-name="${id}">
                    ${name}
                </td>
                <td intern-birthdate="${id}">
                    ${birthDate}
                </td>
                <td intern-emailaddress="${id}">
                    ${emailAddress}
                </td>
                <td intern-teamid="${id}">
                    ${teamId}
                </td>
                <td intern-githubusername="${id}">
                    ${githubUsername}
                </td>
                <td>
                    <a asp-action="Edit" asp-route-id="@item.Id">Edit</a> |
                    <a asp-action="Details" asp-route-id="@item.Id">Details</a> |
                    <a asp-action="Delete" asp-route-id="@item.Id">Delete</a>
                </td>
            </tr>`);
}

function deleteIntern(id) {
    $(`tr[intern-id=${id}]`).remove();
}
function updateIntern(id, name, birthDate, emailAddress, githubUsername, teamId) {
    $(`td[intern-name=${id}]`).text(name);
    $(`td[intern-birthdate=${id}]`).text(birthDate);
    $(`td[intern-emailaddress=${id}]`).text(emailAddress);
    $(`td[intern-teamid=${id}]`).text(teamId);
    $(`td[intern-githubusername=${id}]`).text(githubUsername);
}