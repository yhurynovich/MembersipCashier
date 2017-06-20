function renderLogin(x, $class) {
    this.$template = $([
      '<fieldset class="loginPart ', $class, '">'
      , '   <div><label>User Name</label><input name="UserName" id="UserName" /></div>'
      , '   <div><label>Password</label><input type="password" name="Password" id="Password" /></div>'
      , '   <div><label>Remember Me</label><input type="checkbox" name="rememberMe" id="rememberMe" /></div>'
      , '</fieldset>'
    ].join(""));

    this.$template.find("input[name=UserName]").val(x.userName);
    return this.$template;
}