var FollowingController = function (followService) {

    var button;

    var init = function (container) {
        //$(container).on("click", ".js-toggle-follow", toggleFollow);
        $(".js-toggle-follow").click(toggleFollow);
    }

    var toggleFollow = function (e) {
        button = $(e.target)

        var artistId = button.attr("data-artist-id");

        if (button.hasClass("btn-default")) {
            followService.createFollow(artistId, done, fail);
        } else {
            followService.deleteFollow(artistId, done, fail);
        }
    }


    var done = function () {
        var text = (button.text() == "Following") ? "Follow" : "Following";

        button.toggleClass("btn-info").toggleClass("btn-default").text(text);
    }

    var fail = function () {
        alert("Something has failed");
    }

    return {
        init
    }
}(FollowService);