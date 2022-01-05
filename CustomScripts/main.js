$.ajaxSetup({
    'beforeSend': function (xhr) {
        if (localStorage.getItem("userToken")) {
            xhr.setRequestHeader("Authorization", "Bearer " + sessionStorage.getItem("userToken"));
        }
    }
});