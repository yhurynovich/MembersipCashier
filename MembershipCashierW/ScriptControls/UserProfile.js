$(document).on("userProfileChanged", function (e) {
    if (e.UserProfile.length === undefined)
        e.UserProfile = [e.UserProfile]; //pluralize

    $.each(e.UserProfile, function (index, value) {
        var tagId = "#UserProfile" + value.UserId;
        $(tagId).empty().append(renderUserProfile(value));
    });
});

$(document).on("blobChanged", function (e) {
    if (e.blob.UserProfile !== undefined) {
        $.event.trigger({
            type: "userProfileChanged",
            UserProfile: e.blob.UserProfile
        });
    }
});

$(document).on("change", ".userProfilePart", function (e) {
    var fs = $(event.target).closest("fieldset");
    fs.find("input").addClass("edit");
    if (fs.find("input[type=button]").length < 1)
        $("<div style='text-align:right;'><span style='display:  table-cell;' /><input type='button' value='Update' onclick='triggerUserProfileUpdated(this)' /></div>").appendTo(fs);
});

function triggerUserProfileUpdated(c) {
    var fs = c.parentElement.parentElement;
    var data = new Object();
    $.each(fs.getElementsByTagName('input'), function (index, value) {
        if (value.name !== undefined && value.name.length > 0)
            eval('data.' + value.name + '="' + value.value + '"');
    });
    $.event.trigger({ type: "userProfileUpdated", button: c, fieldSet: fs, UserProfile: data });
}

//$(document).on("userProfileUpdated", function (data) {
//});
