
$(function () {

    $('#savesubmit,#saveshoplistsubmit').click(function (e) {
        var isDescValid = true;

        $('.description').each(function () {

            if ($.trim($(this).val()) == '') {
                isDescValid = false;
                $(this).css({
                    "border": "1px solid red",
                    "background": "#FFCECE"
                });
            }
            else {
                $(this).css({
                    "border": "",
                    "background": ""
                });
            }
        });

        if (isDescValid == false) {
            e.preventDefault();
            alert('All Item Names must contain a value.');
        }
        else {

            if ($('.removeLink:checkbox:checked').length > 0) {
                if (confirm('You have selected items for removal - do you want to apply these changes?')) {
                    return true;
                } else {
                    removeAllCheckRemoveItems();
                }
            }
        }

    });

    function removeAllCheckRemoveItems() {
        $('.removeLink').each(function () {
            $(this).prop('checked', false);
        });
    }

});


