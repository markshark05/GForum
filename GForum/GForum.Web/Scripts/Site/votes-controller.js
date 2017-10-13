$('body').on('click', '.vote-btn', function(e) {
    var $voteBtn = $(e.target);
    var btnWasActiveBeforeClick = $voteBtn.hasClass('active');

    $.ajax({
        method: 'POST',
        url: $voteBtn.parent().data('vote-url'),
        data: {
            voteType: $voteBtn.data('vote-type')
        }
    })
        .done(function(data) {
            if (data.success) {
                $voteBtn.siblings('.vote-count').html(data.likes);

                $voteBtn.siblings('.vote-btn').addBack().removeClass('active');
                if (!btnWasActiveBeforeClick) {
                    $voteBtn.addClass('active');
                }
            } else {
                toastr.error(data.error);
            }
        });
});