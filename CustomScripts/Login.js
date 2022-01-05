

function Login() {

    $.ajax({
        type: 'POST',
        contentType: 'application/x-www-form-urlencoded',
        url: '/Token',
        data: { username: $("#email").val(), password: $("#pass").val(), grant_type: 'password' },
        success: function (data) {
            // Cache the access token in session storage.
            sessionStorage.setItem('userToken', data.access_token);
            sessionStorage.setItem('userName', data.userName);

            console.log(data)
            window.location.href = '/Home/MailBox';
        }
    })
        

}
