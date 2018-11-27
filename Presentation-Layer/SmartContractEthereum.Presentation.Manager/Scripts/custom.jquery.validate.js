$(document).ready(function () {
    $('.input-validation-error').parents('.form-group').addClass('has-error');
    $('.field-validation-error').addClass('text-danger');

    // Store existing event handlers in local variables
    var form = $('#userForm'), formData = $.data(form[0]), settings = formData.validator.settings, oldErrorPlacement = settings.errorPlacement, oldSuccess = settings.success;

    settings.errorPlacement = function (label, element) {

        // Call old handler so it can update the HTML
        oldErrorPlacement(label, element);

        // Add Bootstrap classes to newly added elements
        label.parents('.form-group').addClass('has-error');
        label.addClass('text-danger');
    };

    settings.success = function (label) {
        // Remove error class from <div class="form-group">, but don't worry about
        // validation error messages as the plugin is going to remove it anyway
        label.parents('.form-group').removeClass('has-error');

        // Call old handler to do rest of the work
        oldSuccess(label);
    };

    /*
        $('#editFormId').validate({
            errorClass: 'help-block animation-slideDown', // You can change the animation class for a different entrance animation - check animations page  
            errorElement: 'div',
            errorPlacement: function (error, e) {
                e.parents('.form-group > div').append(error);
            },
            highlight: function (e) {

                $(e).closest('.form-group').removeClass('has-success has-error').addClass('has-error');
                $(e).closest('.help-block').remove();
            },
            success: function (e) {
                e.closest('.form-group').removeClass('has-success has-error');
                e.closest('.help-block').remove();
            },
            rules: {
                'Email': {
                    required: true,
                    email: true
                },

                'Password': {
                    required: true,
                    minlength: 6
                },

                'ConfirmPassword': {
                    required: true,
                    equalTo: '#Password'
                }
            },
            messages: {
                'Email': 'Please enter valid email address',

                'Password': {
                    required: 'Please provide a password',
                    minlength: 'Your password must be at least 6 characters long'
                },

                'ConfirmPassword': {
                    required: 'Please provide a password',
                    minlength: 'Your password must be at least 6 characters long',
                    equalTo: 'Please enter the same password as above'
                }
            }
        });
    */

});

$.validator.addMethod("anyDate",
    function (value, element) {
        return value.match(/^(0?[1-9]|[12][0-9]|3[0-1])[/., -](0?[1-9]|1[0-2])[/., -](19|20)?\d{2}$/);
    },
    "Please enter a date in the format!"
);