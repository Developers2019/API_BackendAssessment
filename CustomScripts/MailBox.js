$(function () {

    $.ajaxSetup({
        'beforeSend': function (xhr) {
            if (sessionStorage.getItem("userToken")) {
                xhr.setRequestHeader("Authorization", "Bearer " + sessionStorage.getItem("userToken"));
            }
        }
    });

    //Add text editor
    //$('#compose-textarea').summernote()
    $('.select2').select2()
})
LoadEmails();


//Loads emails that are sent to the signed-in user
function LoadEmails() {

    Clear();
    $.ajax({
        type: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        data: {},
        url: '/api/mail/getinboxemail',
        success: function (result) {

            console.log(result);
            $('#title').append('Inbox');

            $.each(result, function (key) {

                $('#bd').append('<tr><td><div class="icheck-primary"><input type="checkbox" value="" id="check10"><label for="check10"></label></label></div></td><td class="mailbox-name"><a href="#" data-toggle="modal" data-target="#modal-read" onclick=GetEmailById(' + result[key].Email_Id + ');>' + result[key].SentFrom + '</td><td class="mailbox-subject">' + result[key].Description + '</td><td class="mailbox-date">' + result[key].HowLongAgo + '</td></tr>');


            });
                         

        }
    })
}

function Clear() {

    $('#bd').html('');
    $('#title').html('');
}

//Loads emails that are sent by the signed-in user
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

                $('#bd').append('<tr><td><div class="icheck-primary"><input type="checkbox" value="" id="check10"><label for="check10"></label></label></div></td><td class="mailbox-name"><a href="#" data-toggle="modal" data-target="#modal-read" onclick=GetEmailById(' + result[key].Email_Id + ');>' + result[key].SentTo + '</td><td class="mailbox-subject">' + result[key].Description + '</td><td class="mailbox-date">' + result[key].HowLongAgo + '</td></tr>');

                     
            });

           

        }
    })
}

//Loads deleted emails
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

                $('#bd').append('<tr><td><div class="icheck-primary"><input type="checkbox" value="" id="check10"><label for="check10"></label></label></div></td><td class="mailbox-name"><a href="#" data-toggle="modal" data-target="#modal-read" onclick=GetEmailById(' + result[key].Email_Id + ');>' + result[key].SentFrom + '</td><td class="mailbox-subject">' + result[key].Description + '</td><td class="mailbox-date">' + result[key].HowLongAgo + '</td></tr>');

                     
            });

           

        }
    })
}

//Sends email to a user
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

//Get email details
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
            $('#btnRecover').append('<button type="button" class="btn btn-default btn-sm" onclick="RecoverTrashedMail(' + result.Email_Id + ')" Title="Recover Email"><i class="fas fa-sync-alt"></i></button>');
            $('#btnTag').append('<a href="#" class="btn btn-default btn-sm" data-toggle="modal" data-target="#modal-addlabel" onclick="GetLabelList();" Title="Add/Remove Label"><i class="fas fa-tag"></i></a>');
            $('#btnLabel').append('<button type="submit" class="btn btn-primary" onclick="AddEmailLabel(' + result.Email_Id + ');"><i class="fas fa-plus"></i> Add</button>');
            $('#btnRemoveLabel').append('<button type="submit" class="btn btn-danger" onclick="RemoveEmailLabel(' + result.Email_Id + ');"><i class="fas fa-plus"></i> Remove</button>');

        }
    })

}

//Delete email from inbox
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

//Create a new Label
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


//Delete a label
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

//Recover an email 
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

//Close Modals
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

//Get emails in a specific label
function LoadEmailLabel(id) {

    Clear();
    $.ajax({
        type: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        data: {},
        url: '/api/mail/getemailsbylabel/' + id,
        success: function (result) {

            console.log();

            $('#title').append(result[0].LabelName);

            $.each(result, function (key) {

                $('#bd').append('<tr><td><div class="icheck-primary"><input type="checkbox" value="" id="check10"><label for="check10"></label></label></div></td><td class="mailbox-name"><a href="#" data-toggle="modal" data-target="#modal-read" onclick=GetEmailById(' + result[key].Email_Id + ');>' + result[key].EmailAddressFrom + '</td><td class="mailbox-subject">' + result[key].Description + '</td><td class="mailbox-date">' + result[key].HowLongAgo + '</td></tr>');


            });



        }
    })


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

                $('#labels').append('<ul class="nav nav-pills flex-column"><li class="nav-item"><a href="#" class="nav-link" onclick="LoadEmailLabel(' + result[key].Label_Id + ');"><i class="far fa-circle text-info"></i> ' + result[key].LabelName + '<span class="badge bg-danger float-right" onclick="DeleteLabel(' + result[key].Label_Id + ')" Title="Delete Label"><i class="fas fa-times"></i></span></a><li ></ul>');

            });



        }
    })

}

//Populate dropdown with a list of labels
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
//Get the number of emails in an inbox
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

//Add label to email
function AddEmailLabel(id) {

    var labelId = ddListChange();

    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: {},
        url: '/api/mail/addlabel/' + id + '/' + labelId,
        success: function (result) {

            console.log(result);
            swal({
                title: "Success!",
                text: "Label added to email.",
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

//Remove a label from an email
function RemoveEmailLabel(id) {

    var labelId = ddListChange();

    

    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: {},
        url: '/api/mail/removelabel/' + id + '/' + labelId,
        success: function (result) {

            console.log(result);
            swal({
                title: "Success!",
                text: "Label removed from email.",
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


