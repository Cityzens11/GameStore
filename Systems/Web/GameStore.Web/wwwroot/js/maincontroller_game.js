//window.onload = function () {
//    var replyButtons = document.querySelectorAll("#reply");
//    replyButtons.forEach(function (button) {
//        button.addEventListener("click", function () {
//            var textArea = document.getElementById("comment");
//            if (textArea.style.display === "none") {
//                textArea.style.display = "block";
//            } else {
//                textArea.style.display = "none";
//            }
//        });
//    });
//};

$(document).ready(function () {
    $('a.reply').click(function (event) {
        event.preventDefault();
        var commentId = $(this).data('comment-id');
        $('#edit-' + commentId).hide();
        $('#comment-' + commentId).toggle();
    });
    $('a.edit-comment').click(function (event) {
        event.preventDefault();
        var commentId = $(this).data('comment-id');
        $('#comment-' + commentId).hide();
        $('#edit-' + commentId).toggle();
    });
});

function toggleForm(commentId) {
    var form = document.getElementById('comment-' + commentId);
    form.style.display = form.style.display === 'none' ? 'block' : 'none';
}
