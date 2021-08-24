
$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/teamMessageHub").build();

    connection.start().then(function () {
        console.log("signalr connected");
    });

    connection.on("NewTeamAdded", createNewTeam);
    connection.on("TeamDeleted", deleteTeam);
    connection.on("TeamUpdated", updateTeam);
});

function createNewTeam(id, jiraBoardUrl, git, emblem, motto) {
    $("#teamTableBodyId").append(`<tr team-id="${id}">
                <td team-jira="${id}">
                    ${jiraBoardUrl}
                </td>
                <td team-git="${id}">
                    ${git}
                </td>
                <td team-emblem="${id}">
                    ${emblem}
                </td>
                <td team-motto="${id}">
                    ${motto}
                </td>
                <td>
                    <a asp-action="Edit" asp-route-id="@item.Id">Edit</a> |
                    <a asp-action="Details" asp-route-id="@item.Id">Details</a> |
                    <a asp-action="Delete" asp-route-id="@item.Id">Delete</a>
                </td>
            </tr>`);
}

function deleteTeam(id) {
    $(`tr[team-id=${id}]`).remove();
}
function updateTeam(id, jiraBoardUrl, git, emblem, motto) {
    $(`td[team-jira=${id}]`).text(jiraBoardUrl);
    $(`td[team-git=${id}]`).text(git);
    $(`td[team-emblem=${id}]`).text(emblem);
    $(`td[team-motto=${id}]`).text(motto);
}