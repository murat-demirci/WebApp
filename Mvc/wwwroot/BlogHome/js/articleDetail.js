$(document).ready(function () {
    $(function () {
        $(document).on('click', '#btnSave', function (event) {
            event.preventDefault();
            const form = $('#form-comment-add');
            const actionUrl = form.attr('action');
            const dataToSend = form.serialize();
            $.post(actionUrl, dataToSend).done(function (data) {
                const commentAddAjaxModel = jQuery.parseJSON(data);
                console.log(commentAddAjaxModel);
                const newFormBody = $('.form-card', commentAddAjaxModel.CommentAddPartial);
                const cardBody = $('.form-card');
                cardBody.replaceWith(newFormBody);
                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                if (isValid) {
                    const newSingleComment = `
            <div class="media mb-4" style="display: flex">
                <img class="d-flex mr-3 ml-3 rounded-circle" style="max-height: 100px" src="https://randomuser.me/api/portraits/men/3.jpg" alt="">
                <div class="media-body" style="display: block; margin-left: 20px">
                    <h5 class="mt-0">${commentAddAjaxModel.CommentDto.Comment.CreatedByName}</h5>
                    ${commentAddAjaxModel.CommentDto.Comment.CommentContent}
                </div>
            </div>`;
                    const newSingleCommentObject = $(newSingleComment);
                    newSingleCommentObject.hide();
                    $('#comments').append(newSingleCommentObject);
                    newSingleCommentObject.fadeIn(2000);
                    toastr.success(`Yorum onaylandıktan sonra aktif olacaktır.`);
                    $('#btnSave').prop('disabled', true);
                    setTimeout(function () {
                        $('#btnSave').prop('disabled', false);
                    }, 15000);
                }
                else {
                    let summaryText = "";
                    $('#validation-summary > ul > li').each(function () {
                        let text = $(this).text();
                        summaryText += `*${text}\n`;
                    });
                    toastr.warning(summaryText);
                }
            }).fail(function (error) {
                console.error(error);
            });
        });
    });
});