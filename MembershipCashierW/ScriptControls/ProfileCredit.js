$(document).on("profileCreditsChanged", function (e) {
    $.each(e.ProfileCredits, function (index, value) {
        var tagId = value.UserId + '_' + value.OwnerId + '_' + value.ProductId;
        $("#ProfileCredit" + tagId).find("#Ballance" + tagId).val(value.Ballance);
    });
});

$(document).on("blobChanged", function (e) {
    if (e.blob.ProfileCredits !== undefined) {
        $.event.trigger({
            type: "profileCreditsChanged",
            ProfileCredits: e.blob.ProfileCredits
        });
    }
});

$(document).on("change", ".profileCreditPart", function (e) {
    var fs = $(event.target).closest("fieldset");
    fs.find("input").addClass("edit");
    if (fs.find("input[type=button]").length < 1)
        $("<span style='display:  table-cell;' /><input type='button' value='Update' onclick='triggerProfileCreditUpdated(this)' style='display:table-cell; vertical-align:middle; margin-bottom:1px;'/>").appendTo(fs.find("div"));
});

function triggerProfileCreditUpdated(c) {
    var fs = c.parentElement.parentElement;
    var data = new Object();
    $.each(fs.getElementsByTagName('input'), function (index, value) {
        if (value.name !== undefined && value.name.length > 0)
            eval('data.' + value.name + '="' + value.value + '"');
    });
    $.event.trigger({ type: "profileCreditUpdated", button: c, fieldSet: fs, profileCredit: data });
}

//$(document).on("profileCreditUpdated", function (data) {
//});
