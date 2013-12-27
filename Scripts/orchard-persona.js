(function ($) {
    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    }
    function getUrlVar(name) {
        return getUrlVars()[name];
    }

    function gotVerifiedEmail(assertion) {
        if (assertion !== null) {
            var url = 'users/account/persona';
            // handle logins from parts other than widget that define the personaLogon var.
            // we use this to handle servers running on virtual paths
            if (typeof (personaLogonUrl) !== 'undefined' && personaLogonUrl.length > 0)
                url = personaLogonUrl;
            var returnurl = getUrlVar('ReturnUrl');
            url = url + '?ReturnUrl=' + returnurl;
            $.ajax({
                type: 'POST',
                url: url,
                data: { assertion: assertion },
                success: function (res, status, xhr) {
                    if (typeof (res.returnUrl) !== 'undefined') {
                        window.location.replace(unescape(res.returnUrl));
                    }
                    else {
                        window.location.reload();
                    }
                },
                error: function (res, status, xhr) {
                    alert("login failure" + res);
                }
            });
        }
    }
    $('.persona-login').bind('click', function () {
        navigator.id.get(gotVerifiedEmail);
    });
    $('.persona-logout').bind('click', function () {
        e.preventDefault();
        var url = 'users/account/logoff';
        // handle logins from parts other than widget that define the personaLogon var.
        // we use this to handle servers running on virtual paths
        if (typeof (personaLogoffUrl) !== 'undefined' && personaLogoffUrl.length > 0)
            url = personaLogoffUrl;
        $.ajax({
            type: 'POST',
            url: 'url',
            data: null
        });
    });
})(window.jQuery);