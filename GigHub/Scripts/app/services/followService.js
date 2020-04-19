var FollowService = function () {

    var createFollow = function (artistId, done, fail) {
        $.post("/api/followings", { FolloweeId: artistId })
            .done(done)
            .fail(fail)
    }

    var deleteFollow = function (artistId, done, fail) {
        $.ajax({
            url: "/api/followings/" + artistId,
            method: "DELETE"
        })
            .done(done)
            .fail(fail)
    }

    return {
        createFollow,
        deleteFollow
    }
}();