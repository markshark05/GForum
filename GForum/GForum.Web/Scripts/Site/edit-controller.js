$('body').on('click', '.edit-post-btn', function(e) {
    e.preventDefault();

    var $content = $(e.target).parents('.card').find('.content');
    var content = $content.data('raw-content');
    var editUrl = $(e.target).data('edit-url');

    var $texarea = $('<textarea>').addClass('form-control').val(content);
    var $errors = $('<p>').addClass('text-danger');

    $content.html($texarea);
    $texarea.after($errors);
    $(e.target).replaceWith('Editing');

    var saveBtn = $('<button>').addClass('btn btn-default').html('Save')
        .on('click', function(e) {
            $.ajax({
                method: 'POST',
                url: editUrl,
                data: {
                    content: $texarea.val()
                }
            })
                .done(function(data) {
                    console.log(data);
                    if (data.success) {
                        window.location.reload(true);
                    } else {
                        $errors.html(data.error);
                    }
                })
                .fail(function() {
                    $errors.html('Error!');
                });
        });

    $content.append(saveBtn);
});