
$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/teamMessageHub").build();

    connection.start().then(function () {
        console.log("signalr connected");
    });

    connection.on("NewTeamMemberAdded", createNewTeam);
    connection.on("DeleteTeamMember", deleteTeam);
    connection.on("UpdatedTeamMember", updateTeam);
});

function createNewTeam(id, jiraBoardUrl, git, emblem, motto) {
    $("#teamTableBodyId").append(`<tr team-id="${id}">
                <td>
                    ${jiraBoardUrl}
                </td>
                <td>
                    ${git}
                </td>
                <td>
                    ${emblem}
                </td>
                <td>
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
    $(`tbody[id=${id}]`).remove();
}
function updateTeam() {

}