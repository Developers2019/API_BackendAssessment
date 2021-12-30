

function Login() {

    $.ajax({
        type: 'POST',
        contentType: 'application/x-www-form-urlencoded',
        url: '/Token',
        data: { username: $("#email").val(), password: $("#password").val(), grant_type: 'password' },
    }).done(function (data) {
        //self.user(data.userName);
        // Cache the access token in session storage.
        sessionStorage.setItem('userToken', data.access_token);

        console.log(data)
        window.location.href = '/Home/MailBox';
    });

}
