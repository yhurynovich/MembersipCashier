var requestVerificationToken;

function restoreBlob() {
    if (document.blob === undefined || document.blob == null)
        document.blob = new Object();

    if (document.blob.UserProfile === undefined || document.blob.UserProfile == null)
        document.blob.UserProfile = [];

    if (document.blob.UserSnapshot === undefined || document.blob.UserSnapshot == null)
        document.blob.UserSnapshot = [];
}

function handleJqueryAjaxError(xhr, ajaxOptions, thrownError)
{
    try {
        var container = null;
        if (xhr !== undefined && xhr != null && xhr.fieldSet !== undefined && xhr.fieldSet != null)
            container = xhr.fieldSet;
        if (container == null)
        {
            var pageContainers = $("fieldset");
            if (pageContainers.length == 1)
                container = pageContainers[0];
        }
        if (container == null) {
            container = $("<fieldset></fieldset>");
            $("body").append(container);
            container = container[0];
        }

        if (xhr.responseText !== undefined && xhr.responseText != null) {
            try {
                var ex = JSON.parse(xhr.responseText);
                replaceMessages(container, [{ message: ex.ExceptionMessage, type: "error", thrownError: thrownError }]);
            } catch (e) {
                replaceMessages(container, [{ message: thrownError, type: "error", thrownError: thrownError }]);
            }
        } else {
            replaceMessages(container, [{ message: thrownError, type: "error", thrownError: thrownError }]);
        }
    }catch(z){}
}

function replaceMessages(container, messages)
{
    if (container === undefined || container == null || messages === undefined || messages == null)
        return;

    $.each(container.getElementsByTagName('messages'), function (index, value) {
        value.remove();
    });

    var mm =  document.createElement("messages");
    container.insertAdjacentElement("afterBegin", mm);

    $.each(messages, function (index, value) {
        var txt = "";
        if (value.thrownError !== undefined && value.thrownError.length > 0)
            txt += value.thrownError;
        if (value.thrownError != value.message)
        {
            if (txt.length > 0) txt += ": ";
            txt += value.message;
        }

        mm.insertAdjacentHTML("beforeEnd", "<message class='" + value.type + "'>" + txt + "</message>");
    });
}

function logOff()
{
    $.ajax({
        url: "/api/Authentication/logoff",
        type: "GET",
        success: logOffRedirect(),
        error: function (xhr, ajaxOptions, thrownError) {
            try {
                var ex = JSON.parse(xhr.responseText);
                replaceMessages(data.fieldSet, [{ message: ex.ExceptionMessage, type: "error", thrownError: thrownError }]);
            } catch (e) {
                replaceMessages(data.fieldSet, [{ message: thrownError, type: "error", thrownError: thrownError }]);
            }
        },
        timeout: 1000,
        headers: {
            'RequestVerificationToken': requestVerificationToken
        }
    });
}

function logOffRedirect()
{
    window.location.href = "/";
}

function htmlDecode(input) {
    while (String(input).indexOf('&gt;') > -1)
        input = _htmlDecodeRecursiveHelper(input);
    while (String(input).indexOf('&lt;') > -1)
        input = _htmlDecodeRecursiveHelper(input);

    return input;
}

function _htmlDecodeRecursiveHelper(input) {
    return String(input)
        .replace('&gt;', '>')
        .replace('&lt;', '<');
}