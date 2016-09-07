/*
 * Script responsible for functionality related to hiding the password in url fields.
 * 
 * Concept:
 * 
 * For every url field there are 3 elements:
 *  - a hidden field always containing the full url entered by the user (referred to as hidden field). Text in this
 *      field is submitted when saving.
 *  - a field visible to the user, postfixed with "_Visible" (referred to as visible field). If the text is a (sort of)
 *      valid url and contains a password (i.e. has a [protocol]://[username]:[password]@[host] format), that password 
 *      is replaced by four dots. This replacement happens when the field loses focus or the preset amount of time 
 *      elapsed after the user started editing the field. Must have css class "url-field".
 *  - a div img postfixed with "_SwitchButton" acting as a switch to hide or display the password (referred to as 
 *      switch button). Must have css class "password-switch" and contain a span with the text and an img with either 
 *      "password-hidden" or "password-visible" css class.
 * 
 * When the user edits the visible field, its content is copied to the hidden field, keeping it up-to-date so it is 
 * always ready to be submitted. Once the visible field is no longer being edited (it loses focus, it 
 * has been in editing mode for five minutes or the user explicitly clicks the switch button), its text is examined. 
 * If the field contains a password, that password is replaced by four dots and the field is set to readonly
 * to prevent the user from accidentally editing the text containing the masked password.
 * 
 */
$.extend(true, {
    GitHgMirror: {
        UrlField: {
            _hidingTimers: {},

            _urlRegex: /\s*([\+\w]+):\/\/(\S+):(\S+)@(\S+)\s*/i, // [protocol]://[username]:[password]@[host]

            getVisibleFieldId: function (switchButtonId) {
                // 13 = "_SwitchButton".length
                var result = switchButtonId.substring(0, switchButtonId.length - 13) + "_Visible";
                return result;
            },

            getVisibleField: function (switchButton) {
                return $("#" + this.getVisibleFieldId(switchButton.attr("id")));
            },

            getHiddenFieldId: function (visibleFieldId) {
                return visibleFieldId.substring(0, visibleFieldId.length - 8); // 8 = "_Visible".length
            },

            getHiddenField: function (visibleField) {
                return $("#" + this.getHiddenFieldId(visibleField.attr("id")));
            },

            getSwitchButton: function (visibleField) {
                return $("#" + this.getHiddenFieldId(visibleField.attr("id")) + "_SwitchButton");
            },

            copyContentToHiddenField: function (visibleField) {
                this.getHiddenField(visibleField).val(visibleField.val().trim());
            },

            urlContainsPassword: function (text) {
                return this._urlRegex.test(text);
            },

            hidePasswordInUrl: function (text) {
                var match = this._urlRegex.exec(text);
                if (match === null) {
                    return text;
                } else {
                    return match[1] + "://" + match[2] + ":••••@" + match[4]; // [protocol]://[username]:••••@[host]
                }
            },

            setSwitchButtonVisibility: function (visibleField) {
                var switchButton = this.getSwitchButton(visibleField);
                var hiddenField = this.getHiddenField(visibleField);
                var urlIsHidden = visibleField.is('[readonly]');

                // Show the button only if the url is already hidden or if it contains a password
                if (urlIsHidden || this.urlContainsPassword(hiddenField.val())) {
                    switchButton.show();
                } else {
                    switchButton.hide();
                }
            },

            setSwitchButtonState: function (switchButton, setToReadOnly) {
                var image = switchButton.find("img");
                var text = switchButton.find("a");
                if (setToReadOnly) {
                    text.text("Show / Edit");
                    image.addClass("password-hidden");
                    image.removeClass("password-visible");
                } else {
                    text.text("Hide");
                    image.addClass("password-visible");
                    image.removeClass("password-hidden");
                }
            },

            setVisibleFieldState: function(visibleField, setToReadOnly) {
                if (setToReadOnly) {
                    visibleField.attr("tabIndex", -1);
                    visibleField.attr("readonly", "readonly");
                } else {
                    visibleField.removeAttr("tabIndex");
                    visibleField.removeAttr("readonly");
                }
            },

            clearPreviousTimer: function (visibleField) {
                var visibleFieldId = visibleField.attr("id");
                if (this._hidingTimers[visibleFieldId] !== undefined) {
                    clearTimeout(this._hidingTimers[visibleFieldId]);
                    this._hidingTimers[visibleFieldId] = undefined;
                }
            },

            updateVisibleField: function (visibleField, hidePassword) {
                var originalUrl = this.getHiddenField(visibleField).val();
                var urlToDisplay = hidePassword ? this.hidePasswordInUrl(originalUrl) : originalUrl;
                visibleField.val(urlToDisplay);

                var wasReadOnly = visibleField.attr("readonly") === "readonly";
                var setToReadOnly = hidePassword && (urlToDisplay !== originalUrl);
                this.setVisibleFieldState(visibleField, setToReadOnly);
                this.setSwitchButtonState(this.getSwitchButton(visibleField), setToReadOnly);

                var isReenabled = !setToReadOnly && wasReadOnly;
                if (isReenabled) {  // Start a timer to hide the password five minutes after going to editing mode.
                    // If the user does a show-hide-show, the old timer from the previous show does not 
                    // hide the password prematurely.
                    this.clearPreviousTimer(visibleField);
                    this._hidingTimers[visibleField.attr("id")] = setTimeout(
                        function () { $.GitHgMirror.UrlField.updateVisibleField(visibleField, true); },
                        300000);
                }
            }
        }
    }
});
