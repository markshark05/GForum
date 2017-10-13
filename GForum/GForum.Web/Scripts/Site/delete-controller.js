$('body').on('click', '.delete-post-btn', function(e) {
    e.preventDefault();

    var $deleteBtn = $(e.target);
    var $yesBtn = $('<a>').attr('href', '#').html('Yes');
    var $noBtn = $('<a>').attr('href', '#').html('No');

    $deleteBtn.hide();
    $deleteBtn.after($yesBtn).after($noBtn);

    $noBtn.on('click', function(e) {
        e.preventDefault();
        $noBtn.remove();
        $yesBtn.remove();
        $deleteBtn.show();
    });

    $yesBtn.on('click', function(e) {
        e.preventDefault();
        $.ajax({
            method: 'POST',
            url: $deleteBtn.attr('href'),
        })
            .done(function(data) {
                if (data.success) {
                    toastr.success('Click to go back.', 'Deleted.', {
                        timeOut: 0,
                        onclick: function() {
                            window.location.href = $deleteBtn.data('redirect-url');
                        }
                    });
                } else {
                    toastr.error(data.error);
                }
            });
    });
});