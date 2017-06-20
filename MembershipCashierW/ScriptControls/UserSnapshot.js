$(document).on("userSnapShotChanged", function (e) {
    $.each(e.UserSnapshot, function (index, value) {
        var userId = value.UserProfile.UserId;
        $("#UserSnapshot" + userId).empty().append(renderUserSnapshot(value));
        if (value.UserProfile !== undefined) {
            $.event.trigger({
                type: "userProfileChanged",
                UserProfile: value.UserProfile
            });
        }
        if (value.ProfileCredits !== undefined) {
            $.event.trigger({
                type: "profileCreditsChanged",
                ProfileCredits: value.ProfileCredits
            });
        }
    });
});

$(document).on("blobChanged", function (e) {
    if (e.blob.UserSnapshot !== undefined) {
        $.event.trigger({
            type: "userSnapShotChanged",
            UserSnapshot: e.blob.UserSnapshot
        });
    }
});

//$(".UserSnapshot").change($(".userSnapshot"), function (e) {
//    e.data.find("input").addClass("edit");
//    if (e.data.find("input[type=button]").length<1)
//        $("<div style='text-align:right;'><span style='display:  table-cell;' /><input type='button' value='Update' onclick='triggerUserSnapshotUpdated(this)' /></div>").appendTo(e.data.find("fieldset"));
//});

//function triggerUserSnapshotUpdated(c)
//{
//    var fs = c.parentElement.parentElement;
//    var data = new Object();
//    $.each(fs.getElementsByTagName('input'), function (index, value) {
//        if (value.name !== undefined && value.name.length>0)
//            eval('data.' + value.name + '="' + value.value + '"');
//    });
//    $.event.trigger({ type: "userSnapshotUpdated", button: c, fieldSet: fs, UserSnapshot: data });
//}

//$(document).on("userSnapshotUpdated", function (data) {
//});
