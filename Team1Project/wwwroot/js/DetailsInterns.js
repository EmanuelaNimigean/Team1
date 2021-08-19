$(document).ready(function () {
    getAge();
    getNumberOfRepos();
    getNumberOfCommits();
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

function getNumberOfCommits() {
    var username = $("#githubUsername").text();
    $.ajax({
        method: "GET",
        url: "/api/GithubApi/GetUserCommits/"+username ,

        success: (resultGet) => {
            var month = new Date().getMonth() + 1;
            var avg = resultGet / month;
            $("#numberOfCommits").text(resultGet);
            $('#avgCommitsPerMonths').text(avg);

        }
    });
}

