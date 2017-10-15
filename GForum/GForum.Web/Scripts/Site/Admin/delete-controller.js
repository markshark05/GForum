$('body').on('click', '.category-delete-btn', function(e) {
    e.preventDefault();
    bootbox
        .confirm('This will also delete all posts in the category. Are you sure?', function(res) {
            if (res) {
                $(e.target).parent('form').submit();
            }
        })
});