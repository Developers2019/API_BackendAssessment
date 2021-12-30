LoadEmails();

function LoadEmails() {
    $.ajax({
        type: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        data: {},
        url: '/api/mail/getemail',
        success: function (result) {

            console.log(result);
            $.each(result, function (key) {

                $('#bd').append('<tr><td><div class="icheck-primary><input type="checkbox" value="" id="check1"><label for="check1"></label></div></td><td class="mailbox-name"><a href="read-mail.html">' + result[key].EmailAddressFrom + '</td><td class="mailbox-subject">' + result[key].Subject +  '</td><td class="mailbox-date">' +   result[key].CapturedDate +  '</td><</tr>');
                
            });

           

        }
    })
}


$(function () {
    //Enable check and uncheck all functionality
    $('.checkbox-toggle').click(function () {
        var clicks = $(this).data('clicks')
        if (clicks) {
            //Uncheck all checkboxes
            $('.mailbox-messages input[type=\'checkbox\']').prop('checked', false)
            $('.checkbox-toggle .far.fa-check-square').removeClass('fa-check-square').addClass('fa-square')
        } else {
            //Check all checkboxes
            $('.mailbox-messages input[type=\'checkbox\']').prop('checked', true)
            $('.checkbox-toggle .far.fa-square').removeClass('fa-square').addClass('fa-check-square')
        }
        $(this).data('clicks', !clicks)
    })

    //////Handle starring for glyphicon and font awesome
    ////$('.mailbox-star').click(function (e) {
    ////    e.preventDefault()
    ////    //detect type
    ////    var $this = $(this).find('a > i')
    ////    var glyph = $this.hasClass('glyphicon')
    ////    var fa = $this.hasClass('fa')

    ////    //Switch states
    ////    if (glyph) {
    ////        $this.toggleClass('glyphicon-star')
    ////        $this.toggleClass('glyphicon-star-empty')
    ////    }

    ////    if (fa) {
    ////        $this.toggleClass('fa-star')
    ////        $this.toggleClass('fa-star-o')
    ////    }
    ////})
})