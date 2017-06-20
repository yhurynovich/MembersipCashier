function renderProfileCredit(x, $class) {
    var tagId = x.UserId + '_' + x.OwnerId + '_' + x.ProductId;
    this.$template = $([
      '<fieldset id="ProfileCredit' + tagId + '" class="profileCreditPart ', $class, '">'
      , '<input type="hidden" name="UserId" id="UserId', tagId, '" />'
      , '<input type="hidden" name="OwnerId" id="UserId', tagId, '" />'
      , '<input type="hidden" name="ProductId" id="UserId', tagId, '" />'
      , '   <div><label>Ballance</label><input type="number" class="profileCreditBallance" name="Ballance" id="Ballance', tagId, '"/></div>'
      , '</fieldset>'
    ].join(""));

    this.$template.find('input[name=UserId]').val(x.UserId);
    this.$template.find('input[name=OwnerId]').val(x.OwnerId);
    this.$template.find('input[name=ProductId]').val(x.ProductId);
    this.$template.find('input[name=Ballance]').val(x.Ballance);

    return this.$template;
}