$('body').on('click', '.edit-post-btn', function(e) {
    e.preventDefault();

    var $editBtn = $(e.target);
    var $contentBox = $editBtn.parents('.card').find('.content');

    var content = $contentBox.data('raw-content');
    var editUrl = $editBtn.attr('href');

    var $texarea = $('<textarea>').addClass('form-control').val(content);
    var $errors = $('<p>').addClass('text-danger');

    $contentBox.html($texarea);
    $texarea.after($errors);
    $editBtn.replaceWith('Editing');

    var $saveBtn = $('<button>').addClass('btn btn-default').html('Save')
        .on('click', function(e) {
            $.ajax({
                method: 'POST',
                url: editUrl,
                data: {
                    content: $texarea.val()
                }
            })
                .done(function(data) {
                    if (data.success) {
                        window.location.reload(true);
                    } else {
                        $errors.html(data.error);
                    }
                });
        });

    $contentBox.append($saveBtn);
});