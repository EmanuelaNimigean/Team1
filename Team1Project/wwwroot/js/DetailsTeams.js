$(document).ready(function () {
    getAvgAge();
})

function getAvgAge() {
    var id = $("#getAvgAge").children().attr("id");
    $.ajax({
        method: "GET",
        url: "/Teams/GetAverageAge",
        data: {
            "teamId": id
        },

        success: (resultGet) => {
            $("#getAvgAge").children().text(resultGet)
        }
    });
}
