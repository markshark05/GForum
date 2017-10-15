$('body').on('click', '.admin-btn', function(e) {
    e.preventDefault();
    var $target = $(e.target);
    bootbox
        .confirm($target.data('message'), function(res) {
            if (res) {
                $target.parent('form').submit();
            }
        })
});