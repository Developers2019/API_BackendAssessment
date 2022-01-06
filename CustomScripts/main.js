$.ajaxSetup({
    'beforeSend': function (xhr) {
        if (sessionStorage.getItem("userToken")) {
            xhr.setRequestHeader("Authorization", "Bearer " + sessionStorage.getItem("userToken"));
        }
    }
});