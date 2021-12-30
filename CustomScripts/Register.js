function AddUser() {

    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify({ Email: $('#email').val(), Password: $('#password').val(), ConfirmPassword: $('#passwordretype').val()}),
        url: '/api/Account/Register',
        async: false,
        success: function (result) {

            if (result!==null) {

                alert("Successful");
            }
            else {
                alert("unSuccessful");
            }


        }

    });


}