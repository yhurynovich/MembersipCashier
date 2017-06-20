function renderLogOff(x, $class) {
    this.$template = $([
      //'<fieldset class="logoffPart ', $class, '">'
       '   <a href="#" onclick="logOff()">Log Off</a>'
      //, '</fieldset>'
    ].join(""));

    return this.$template;
}