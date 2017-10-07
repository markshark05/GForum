$('body').on('click', '.vote-container', function(e) {
    $.ajax({
        method: 'POST',
        url: $(e.target.parentElement).data('vote-url'),
        data: {
            voteType: $(e.target).data('vote-type')
        }
    }).done(function(data) {
        $(e.target).siblings('.vote-count').html(data);
        var targetIsActive = $(e.target).hasClass('active');
        $('.vote-btn').removeClass('active');
        if (!targetIsActive) $(e.target).addClass('active');
    });
});