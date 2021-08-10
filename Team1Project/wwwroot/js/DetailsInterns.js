$(document).ready(function () {
    getAge();
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
