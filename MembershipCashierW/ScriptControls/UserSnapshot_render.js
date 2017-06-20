function renderUserSnapshot(x, $class)
{
    var parts = [];
    if (x.UserProfile !== undefined && x.UserProfile!=null)
        parts.push(renderUserProfile(x.UserProfile, $class));
    $.each(x.ProfileCredits, function (index, value) {
        if (value != null) {
            var tagId = "ProfileCredit" + value.UserId + '_' + value.OwnerId + '_' + value.ProductId;
            var tag = $("#" + tagId);
            if (tag.length === undefined || tag.length < 1) {
                parts.push($('<div id="' + tagId + '" name="profileCredit">'));
                parts.push(renderProfileCredit(value, $class));
                parts.push('</div>');
            }
        }
    });
    return parts;
}