function renderUserProfile(x, $class) {
    this.$template = $([
      '<table id="UserProfile' + x.UserId + '" cellpadding="0" cellspacing="0"><tr>'
      , '<td class="userNameBarcode" id="userNameBarcode', x.UserId, '" valign="top" style="width:40px; height:40px; vertical-align:top; border:1px solid #aaa; padding:0; margin:0;">'
      , '<script>$("#userNameBarcode', x.UserId, '").barcode("', x.UserName, '", "datamatrix", {output:"css", moduleSize:2, showHRI:false, posX:0, posY:0, fontSize:0});</script>'
      , '</td>'
      , '<td style="padding:0">'
      , '<fieldset class="userProfilePart table ', $class, '">'
      , '<input type="hidden" name="UserId" id="UserId', x.UserId, '" />'
      , '<input type="hidden" name="UserStatusId" id="UserStatusId', x.UserId, '" />'
      , '<input type="hidden" name="UserName" id="UserName', x.UserId, '" />'
      , '<div class="row"><label style="display: table-cell;">First Name</label><input class="cell" name="FirstName" id="FirstName', x.UserId, '" /></div>'
      , '<div class="row"><label>Last Name</label><input class="cell" name="LastName" id="LastName', x.UserId, '" /></div>'
      , '</fieldset>'
      , '</td>'
      ,'</tr></table>'
    ].join(""));

    this.$template.find("input[name=UserId]").val(x.UserId);
    this.$template.find("input[name=UserStatusId]").val(x.UserStatusId);
    this.$template.find("input[name=UserName]").val(x.UserName);
    this.$template.find("input[name=FirstName]").val(x.FirstName);
    this.$template.find("input[name=LastName]").val(x.LastName);

    return this.$template;
}