$(document).ready(function () {
    getAge();
    getNumberOfRepos();
})

function getAge() {
    var id = $("#getAge").children().attr("id");
    $.ajax({
        method: "GET",
        url: "/Interns/GetAge",
        data: {
            "id": id
        },

        success: (resultGet) => {
            $("#getAge").children().text(resultGet)

        }
    });
}


function getNumberOfRepos() {
    var id = $("#getNumberOfRepos").children().attr("id");
    $.ajax({
        method: "GET",
        url: "/Interns/GetNumberOfRepos",
        data: {
            "id": id
        },

        success: (resultGet) => {
            $("#getNumberOfRepos").children().text(resultGet)

        }
    });
}

