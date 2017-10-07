$('body').on('click', '.vote-btn', function(e) {
    $.ajax({
        method: 'POST',
        url: $(e.target.parentElement).data('vote-url'),
        data: {
            voteType: $(e.target).data('vote-type')
        }
    })
        .done(function(data) {
            $(e.target).siblings('.vote-count').html(data);
            var targetIsActive = $(e.target).hasClass('active');
            $(e.target).siblings('.vote-btn').addBack().removeClass('active');
            if (!targetIsActive) $(e.target).addClass('active');
        })
        .fail(function() {
            toastr.error('You must be logged in to do this.')
        });
});