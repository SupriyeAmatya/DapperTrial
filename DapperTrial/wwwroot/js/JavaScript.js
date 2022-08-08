$('body').on('click', '#ShowForgotPassword', function () {
    $.ajax({
        method: "GET",
        url: "/Login/_ForgotPassword",
        success: function (data) {
            $('body').find('#FormBox .modal-header .modal-title').html('');
            $('body').find('#FormBox .modal-header .modal-title').html('Forgot Password');

            $('body').find('#FormBox .modal-body').html('');
            $('body').find('#FormBox .modal-body').html(data);

            $('#FormBox').modal('show');
        }
    });


});

function OnSuccess(data) {
    console.log(data);
    if (data.code == 0) {
        toastr.success('Email Sent', 'Success')
        $('#FormBox').modal('hide');

    }
    if (data.code == 9) {
        toastr.error('Could not send email', 'Error')

    }
    if (data.code == 8) {
        toastr.error('Email does not exist', 'Error')

    }

}