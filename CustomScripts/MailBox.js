LoadEmails();

function LoadEmails() {

    Clear();
    $.ajax({
        type: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        data: {},
        headers: {
            'Authorization': 'Bearer' + sessionStorage.getItem('userToken')

        },
        url: '/api/mail/getinboxemail',
        success: function (result) {

            console.log(result);
            $('#title').append('Inbox');

            $.each(result, function (key) {

                $('#bd').append('<tr><td class="mailbox-name"><a href="#" data-toggle="modal" data-target="#modal-read" onclick=GetEmailById(' + result[key].Email_Id + ');>' + result[key].EmailAddressFrom + '</td><td class="mailbox-subject">' + result[key].Description + '</td><td class="mailbox-date">' + result[key].HowLongAgo + '</td></tr>');


            });
                         

        }
    })
}

function Clear() {

    $('#bd').html('');
    $('#title').html('');
}

function LoadSentEmails() {

    Clear();
    $.ajax({
        type: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        data: {},
        url: '/api/mail/getsentemail',
        success: function (result) {

            
            $('#title').append('Sent');

            $.each(result, function (key) {

                $('#bd').append('<tr><td><div class="icheck-primary"><input type="checkbox" value="" id="check10"><label for="check10"></label></label></div></td><td id="emailId">' + result[key].Email_Id + '</td><td class="mailbox-name"><a href="#" data-toggle="modal" data-target="#modal-read" onclick=GetEmailById(' + result[key].Email_Id +');>' + result[key].EmailAddressFrom + '</td><td class="mailbox-subject">' + result[key].Description + '</td><td class="mailbox-date">' + result[key].HowLongAgo + '</td></tr>');
                               
                     
            });

           

        }
    })
}

function LoadTrashedEmails() {

    Clear();
    $.ajax({
        type: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        data: {},
        url: '/api/mail/gettrashedemail',
        success: function (result) {

            
            $('#title').append('Trash');

            $.each(result, function (key) {

                $('#bd').append('<tr><td><div class="icheck-primary"><input type="checkbox" value="" id="check10"><label for="check10"></label></label></div></td><td id="emailId">' + result[key].Email_Id + '</td><td class="mailbox-name"><a href="#" data-toggle="modal" data-target="#modal-read" onclick=GetEmailById(' + result[key].Email_Id +');>' + result[key].EmailAddressFrom + '</td><td class="mailbox-subject">' + result[key].Description + '</td><td class="mailbox-date">' + result[key].HowLongAgo + '</td></tr>');
                               
                     
            });

           

        }
    })
}

function SendEmail() {

  
    $.ajax({
        type: 'Post',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify({ EmailAddressTo: $("#to").val(), Subject: $("#subject").val(), Message: $("#compose-textarea").val() }),
        url: '/api/mail/sendemail',
        success: function (result) {

            console.log(result);

            //Reload page once done
            swal({
                title: "Success!",
                text: "Email sent successfully.",
                type: "success",
                html: true,
                showConfirmButton: false,
                timer: 1000
            }, function () {

                window.location.href = '/Home/MailBox';

            });


        }
    })
}

function GetEmailById(id) {


    $.ajax({
        type: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        data: {},
        url: '/api/mail/reademail/' + id,
        success: function (result) {

            $('#sub').append(result.Subject);
            $('#from').append('From:' + result.EmailAddressFrom);
            $('#date').append(result.DisplayDate);
            $('#message').append(result.HTMLBody);
            $('#btnDelete').append('<button type="button" class="btn btn-default btn-sm" data-toggle="tooltip" data-container="body" title="Delete" onclick="SendToTrash(' + result.Email_Id + ')"><i class= "far fa-trash-alt"></i ></button >');
            $('#btnRecover').append('<button type="button" class="btn btn-default btn-sm" onclick="RecoverTrashedMail(' + result.Email_Id + ')"><i class="fas fa-sync-alt"></i></button>');
            $('#btnTag').append('<a href="#" class="btn btn-default btn-sm" data-toggle="modal" data-target="#modal-addlabel" onclick="GetLabelList();"><i class="fas fa-tag"></i></a>');
            $('#btnLabel').append('<button type="submit" class="btn btn-primary" onclick="AddEmailLabel(' + result.Email_Id + ');"><i class="fas fa-plus"></i> Add</button>');

        }
    })

}

function SendToTrash(id) {


    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: {},
        url: '/api/mail/sendtotrash/' + id,
        success: function (result) {

            console.log(result);
            //sweet alert
            swal({
                title: "Success!",
                text: "Email sent to trash.",
                type: "success",
                html: true,
                showConfirmButton: false,
                timer: 1000
            }, function () {

                window.location.href = '/Home/MailBox';

            });
        }
    })

}

function CreateLabel() {


    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify({LabelName: $("#txtLabel").val()}),
        url: '/api/mail/createlabel',
        success: function (result) {

            console.log(result);
            //sweet alert

            swal({
                title: "Success!",
                text: "Label created successfully.",
                type: "success",
                html: true,
                showConfirmButton: false,
                timer: 1000
            }, function () {

                window.location.href = '/Home/MailBox';

            });
        }
    })

}

function RemoveLabel(id) {


    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: {},
        url: '/api/mail/removelabel/' + id,
        success: function (result) {

            console.log(result);
            //sweet alert
        }
    })

}

function DeleteLabel(id) {
    swal({ title: "Delete Label", text: "Are you sure you want to delete this label?", type: "warning", showCancelButton: true, cancelButtonText: "No", confirmButtonClass: "btn-primary", confirmButtonText: "Yes", closeOnConfirm: false }, function () {


        $.ajax({
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            data: {},
            url: '/api/mail/deletelabel/' + id,
            success: function (result) {

                console.log(result);
                //sweet alert
                swal({ title: "Success!", text: "Label Deleted Successfully", type: "success", html: true, timer: 1000, showConfirmButton: false }, function () {

                    window.location.href = '/Home/MailBox';
                });

            }
        })
    });

}

function RecoverTrashedMail(id) {


    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: {},
        url: '/api/mail/recoveremail/' + id,
        success: function (result) {

            console.log(result);
            swal({
                title: "Success!",
                text: "Email recovered successfully.",
                type: "success",
                html: true,
                showConfirmButton: false,
                timer: 1000
            }, function () {

                window.location.href = '/Home/MailBox';

            });
        }
    })

}

function ClosePreview() {

    setTimeout(function () {
        $('#modal-read').modal('hide');
        window.location.href = '/Home/MailBox';
    }, 500);
}

function CloseLabelModal() {

    setTimeout(function () {
        $('#modal-Label').modal('hide');
        window.location.href = '/Home/MailBox';
    }, 500);
}

function CloseComposeModal() {

    setTimeout(function () {
        $('#modal-compose').modal('hide');
        window.location.href = '/Home/MailBox';
    }, 500);
}


//Get list of labels
GetLabels();
function GetLabels() {

    $.ajax({
        type: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        data: {},
        url: '/api/mail/getlabels',
        success: function (result) {

            console.log(result);
            $.each(result, function (key) {

                $('#labels').append('<ul class="nav nav-pills flex-column"><li class="nav-item"><a href="#" class="nav-link"><i class="far fa-circle text-info"></i> ' + result[key].LabelName + '<span class="badge bg-danger float-right" onclick="DeleteLabel(' + result[key].Label_Id + ')" Title="Delete Label"><i class="fas fa-times"></i></span></a><li ></ul>');

            });



        }
    })

}
function GetLabelList() {
    $('#modal-read').modal('hide');
    $.ajax({
        type: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        data: {},
        url: '/api/mail/getlabels',
        success: function (result) {

            console.log(result);
            $.each(result, function (key) {

                $('#labelDisplay').append('<option value="' + result[key].Label_Id + '">' + result[key].LabelName + '</option>')

            });



        }
    })

}


GetCount();
function GetCount() {

    $.ajax({
        type: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        data: {},
        url: '/api/mail/getemailcount',
        success: function (result) {
                                                                                                                                                                                                                                                                                                                                                                                                             
            console.log(result);
            $('#inboxCount').append(result);


        }
    })

}

ddListChange();
function ddListChange() {

    var item = $("#labelDisplay option:selected").val();
    return item;
}

function AddEmailLabel(id) {

    var item = ddListChange();

    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify({ labelId: item }),
        url: '/api/mail/addlabel/' + id,
        success: function (result) {

            console.log(result);
            swal({
                title: "Success!",
                text: "Label add to email.",
                type: "success",
                html: true,
                showConfirmButton: false,
                timer: 1000
            }, function () {

                window.location.href = '/Home/MailBox';

            });



        }
    })

}


$(function () {
    //Add text editor
    //$('#compose-textarea').summernote()

    $('.select2').select2()
})