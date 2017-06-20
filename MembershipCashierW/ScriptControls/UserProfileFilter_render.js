function renderUserProfileFilter(x, $class) {
    this.$template = $([
      '<fieldset class="userProfileFilter">'
      , '<div><label>First Name</label><input name="FirstNameFilter"></div>'
      , '<div><label>Last Name</label><input name="LastNameFilter"></div>'
      , '</fieldset>'
    ].join(""));

    this.$template.find("input[name=FirstNameFilter]").change(this.$template, function (e) { $.event.trigger({ type: "userProfileFilterChanged", ProfileFilter: e.data }); });
    this.$template.find("input[name=LastNameFilter]").change(this.$template, function (e) { $.event.trigger({ type: "userProfileFilterChanged", ProfileFilter: e.data }); });
    return this.$template;
}

$(document).on("userProfileFilterChanged", function (e) {
    restoreBlob();
    document.blob.UserProfileFilter = e.ProfileFilter[0];
    if (document.blob.UserProfileFilter !== undefined && document.blob.UserProfileFilter != null) {
        var lambda = buildUserProfileFilterLambda(document.blob.UserProfileFilter);
        try {
            renderUserProfileForExpression(lambda);
        } catch (ee) {
            replaceMessages(e.fieldSet, [{ message: "Unable to filter profiles", type: "error", thrownError: "" }]);
        }
    }
});

function buildUserProfileFilterLambda(fieldset)
{
    var ret = [];
    var input = $(fieldset).find("input[name=FirstNameFilter]");
    if (input.length > 0 && input[0].value.length > 0)
        ret.push('x.FirstName~="' + input[0].value + '"');
    input = $(fieldset).find("input[name=LastNameFilter]");
    if (input.length > 0 && input[0].value.length > 0)
        ret.push('x.LastName ~ "' + input[0].value + '"');
    return "x=>" + ret.join(" && ");
}