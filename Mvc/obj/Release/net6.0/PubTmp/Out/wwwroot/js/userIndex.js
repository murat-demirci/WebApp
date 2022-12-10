toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "3000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};
$(document).ready(function () {
    $('#usersTable').DataTable({
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                text: 'Ekle',
                attr: {
                    id: "btnAdd",
                },
                className: 'btn btn-success',
                action: function (e, dt, node, config) {

                }
            },
            {
                text: 'Yenile',
                className: 'btn btn-warning',
                action: function (e, dt, node, config) {
                    $.ajax({
                        type: 'GET',
                        url: '/Admin/User/GetAllCategories/',
                        contentType: 'application/json',
                        beforeSend: function () {
                            $('#usersTable').attr('style', 'opacity:0.1;user-select:none;');
                            $('iframe').removeClass('d-none');

                        },
                        success: function (data) {
                            const categoryListDto = jQuery.parseJSON(data);
                            if (categoryListDto.resultStatus === 0) {
                                let tableBody = '';
                                $.each(categoryListDto.Categories.$values, function (index, category) {
                                    if (category.Note == null || category.Note == "") {
                                        category.Note = "Note yok";
                                    }
                                    if (category.Articles.$values.length == null || category.Articles.$values.length == undefined) {
                                        category.Articles.$values.length = 0;
                                    }
                                    tableBody +=
                                        `
                                                <tr name="${category.ID}">
                                <td>${category.ID}</td>
                                <td>${category.Name}</td>
                                <td>${convertFirstToUpper(category.IsActive.toString())}</td>
                                <td>${category.Note}</td>
                                <td>${convertToShortDate(category.CreatedDate)}</td>
                                <td>${category.CreatedByName}</td>
                                <td>${convertToShortDate(category.ModifiedDate)}</td>
                                <td>${category.ModifiedByName}</td>
                                <td>${category.Articles.$values.length}</td>
                                <td style="max-width:200px;min-width:100px;text-align:center;">
                                    <button data-id="${category.ID}" style="width:90px;font-size:12px;" class="btn btn-primary btn-edit"><span class="fas fa-edit"></span> Duzenle</button>
                                    <button style="width:90px;font-size:12px;" data-id="${category.ID}" class="btn-delete btn btn-danger"><span class="fas fa-minus-circle"></span> Kaldir</button>
                                </td>
                            </tr>
                                                `;
                                });
                                $('#usersTable tbody').html(tableBody);
                                setTimeout(function () {
                                    $('#usersTable').removeAttr('style');
                                    $('iframe').addClass('d-none');
                                }, 1000);
                            }
                            else {
                                toastr.error(`${categoryListDto.Message}`, 'İşlem Başarısız!');
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            $('#usersTable').removeAttr('style');
                            
                            $('iframe').addClass('d-none');
                            toastr.error(`${err.responseText}`, 'Hata!');
                        }
                    });
                }
            }
        ],
        language: {
            "sDecimal": ",",
            "sEmptyTable": "Tabloda herhangi bir veri mevcut değil",
            "sInfo": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            "sInfoEmpty": "Kayıt yok",
            "sInfoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "Sayfada _MENU_ kayıt göster",
            "sLoadingRecords": "Yükleniyor...",
            "sProcessing": "İşleniyor...",
            "sSearch": "Ara:",
            "sZeroRecords": "Eşleşen kayıt bulunamadı",
            "oPaginate": {
                "sFirst": "İlk",
                "sLast": "Son",
                "sNext": "Sonraki",
                "sPrevious": "Önceki"
            },
            "oAria": {
                "sSortAscending": ": artan sütun sıralamasını aktifleştir",
                "sSortDescending": ": azalan sütun sıralamasını aktifleştir"
            },
            "select": {
                "rows": {
                    "_": "%d kayıt seçildi",
                    "0": "",
                    "1": "1 kayıt seçildi"
                }
            }
        }
    });
    // DataTables ends here 



    //Add
    $(function () {
        const url = '/Admin/Category/Add/';
        const placeHolderDiv = $('#modalPlaceHolder');
        $('#btnAdd').click(function () {
            $.get(url).done(function (data) {
                placeHolderDiv.html(data);
                placeHolderDiv.find(".modal").modal('show');
            });
        });
        //Ajax post islemi
        placeHolderDiv.on('click', '#btnSave', function (e) {
            e.preventDefault();
            const form = $('#form-category-add');
            const actionUrl = form.attr('action');//asp-action urlsine gider
            const dataToSend = form.serialize(); //gonderilecek veiriyi categoryadddto olarak donusturduk
            $.post(actionUrl, dataToSend).done((data) => {
                const ajaxAddModel = jQuery.parseJSON(data);//data model
                const newFormBody = $('.modal-body', ajaxAddModel.CategoryAddPartial);//formun oldugu kismi alicaz
                placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                if (isValid) {
                    placeHolderDiv.find('.modal').modal('hide');
                    if (ajaxAddModel.CategoryDto.Category.Note == null || ajaxAddModel.CategoryDto.Category.Note == "") {
                        ajaxAddModel.CategoryDto.Category.Note = "Note yok";
                    }
                    let tableRow = '';
                    if (ajaxAddModel.CategoryDto.Category.Articles == null) {
                        tableRow = `
                        <tr name="${ajaxAddModel.CategoryDto.Category.ID}">
                                                    <td>${ajaxAddModel.CategoryDto.Category.ID}</td>
                                                    <td>${ajaxAddModel.CategoryDto.Category.Name}</td>
                                                    <td>${convertFirstToUpper(ajaxAddModel.CategoryDto.Category.IsActive.toString())}</td>
                                                    <td>${ajaxAddModel.CategoryDto.Category.Note}</td>
                                                    <td>${convertToShortDate(ajaxAddModel.CategoryDto.Category.CreatedDate)}</td>
                                                    <td>${ajaxAddModel.CategoryDto.Category.CreatedByName}</td>
                                                    <td>${convertToShortDate(ajaxAddModel.CategoryDto.Category.ModifiedDate)}</td>
                                                    <td>${ajaxAddModel.CategoryDto.Category.ModifiedByName}</td>
                                                    <td>${0}</td>
                                                    <td style="max-width:200px;min-width:100px;text-align:center;">
                                    <button data-id="${ajaxAddModel.CategoryDto.Category.ID}" style="width:90px;font-size:12px;" class="btn btn-primary btn-edit"><span class="fas fa-edit"></span> Duzenle</button>
                                    <button style="width:90px;font-size:12px;" data-id="${ajaxAddModel.CategoryDto.Category.ID}" class="btn-delete btn btn-danger"><span class="fas fa-minus-circle"></span> Kaldir</button>
                                </td>
                                                </tr>
                                `;
                    } else {
                        tableRow = `
                        <tr name="${ajaxAddModel.CategoryDto.Category.ID}">
                                                    <td>${ajaxAddModel.CategoryDto.Category.ID}</td>
                                                    <td>${ajaxAddModel.CategoryDto.Category.Name}</td>
                                                    <td>${convertFirstToUpper(ajaxAddModel.CategoryDto.Category.IsActive.toString())}</td>
                                                    <td>${ajaxAddModel.CategoryDto.Category.Note}</td>
                                                    <td>${convertToShortDate(ajaxAddModel.CategoryDto.Category.CreatedDate)}</td>
                                                    <td>${ajaxAddModel.CategoryDto.Category.CreatedByName}</td>
                                                    <td>${convertToShortDate(ajaxAddModel.CategoryDto.Category.ModifiedDate)}</td>
                                                    <td>${ajaxAddModel.CategoryDto.Category.ModifiedByName}</td>
                                                    <td>${ajaxAddModel.CategoryDto.Category.Articles.$values.length}</td>
                                                    <td style="max-width:200px;min-width:100px;text-align:center;">
                                    <button data-id="${ajaxAddModel.CategoryDto.Category.ID}" style="width:90px;font-size:12px;" class="btn btn-primary btn-edit"><span class="fas fa-edit"></span> Duzenle</button>
                                    <button style="width:90px;font-size:12px;" data-id="${ajaxAddModel.CategoryDto.Category.ID}" class="btn-delete btn btn-danger"><span class="fas fa-minus-circle"></span> Kaldir</button>
                                </td>
</tr>
                                `;
                    }

                    const tableRowObject = $(tableRow);
                    tableRowObject.hide();
                    $('#usersTable').append(tableRowObject);
                    tableRowObject.fadeIn(1500);
                    toastr["success"]('Basatili Islem', `${ajaxAddModel.CategoryDto.Message}`);

                }
                else {
                    let summaryText = '';
                    $('#validation-summary  ul  li').each(function () {
                        let text = $(this).text();
                        summaryText += `*${text}\n`;
                    });
                    toastr["warning"](summaryText);
                }
            });
        });
        //Add
    });



    //Delete
    $(document).on('click', '.btn-delete', function (e) {
        e.preventDefault();
        const id = $(this).attr('data-id');
        const tblRow = $(`[name=${id}]`);
        const categoryName = tblRow.find('td:eq(1)').text();//2.siradaki table data
        Swal.fire({
            title: `${categoryName}\nKaldirmak istediginize emin misniz?`,
            text: "Kaldirilan kategoriyi cop kutusundan geri getirebilirsiniz",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Evet, silmek istiyorum',
            cancelButtonText: 'Hayir, silmek istemiyorum'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: '/Admin/Category/Remove/',
                    data: { categoryId: id },
                    success: function (data) {
                        const categoryDto = jQuery.parseJSON(data);
                        if (categoryDto.resultStatus === 0) {
                            Swal.fire(
                                categoryDto.Message,
                                `${categoryDto.Category.Name} Cop kutusuna tasindi`,
                                'success'
                            );
                            tblRow.fadeOut(1500);
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Basarisiz islem!',
                                text: categoryDto.Message
                            });
                        }
                    },
                    error: function (err) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Beklenmedik bir hata olustu',
                            text: categoryDto.Message + " " + err.responseText,
                        })
                    }
                })
            }
        })
    });
    //Delete

    $(function () {
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click', '.btn-edit', function (e) {
            e.preventDefault();
            const id = $(this).attr('data-id');
            $.ajax({
                type: 'GET',
                url: "/Admin/Category/Update",
                data: { categoryId: id },
                success: function (data) {
                    console.log(data);
                    placeHolderDiv.html(data);
                    placeHolderDiv.find('.modal').modal('show');
                },
                error: function (err) {
                    toastr.error("Hata");
                }

            });
        });
        //post edit
        placeHolderDiv.on('click', '#btnUpdate', function (e) {
            e.preventDefault();
            const form = $('#form-category-update');
            const actionUrl = form.attr('action');
            const dataToSend = form.serialize();//veri categoryupdatedto hali ile alinir 
            $.post(actionUrl, dataToSend).done(function (data) {
                const categoryUpdateAjax = jQuery.parseJSON(data);
                const newForm = $('.modal-body', categoryUpdateAjax.CategoryUpdatePartial);
                placeHolderDiv.find('.modal-body').html(newForm);
                const isValid = newForm.find('[name="IsValid"]').val() === 'True';
                if (isValid) {
                    placeHolderDiv.find('.modal').modal('hide');
                    if (categoryUpdateAjax.CategoryDto.Category.Note == null || categoryUpdateAjax.CategoryDto.Category.Note == "") {
                        categoryUpdateAjax.CategoryDto.Category.Note = "Note yok";
                    }
                    let tableRowN = '';
                    if (categoryUpdateAjax.CategoryDto.Category.Articles == null) {
                        tableRowN = `
                        <tr name="${categoryUpdateAjax.CategoryDto.Category.ID}">
                                                    <td>${categoryUpdateAjax.CategoryDto.Category.ID}</td>
                                                    <td>${categoryUpdateAjax.CategoryDto.Category.Name}</td>
                                                    <td>${convertFirstToUpper(categoryUpdateAjax.CategoryDto.Category.IsActive.toString())}</td>
                                                    <td>${categoryUpdateAjax.CategoryDto.Category.Note}</td>
                                                    <td>${convertToShortDate(categoryUpdateAjax.CategoryDto.Category.CreatedDate)}</td>
                                                    <td>${categoryUpdateAjax.CategoryDto.Category.CreatedByName}</td>
                                                    <td>${convertToShortDate(categoryUpdateAjax.CategoryDto.Category.ModifiedDate)}</td>
                                                    <td>${categoryUpdateAjax.CategoryDto.Category.ModifiedByName}</td>
                                                    <td>${0}</td>
                                                    <td style="max-width:200px;min-width:100px;text-align:center;">
                                    <button data-id="${categoryUpdateAjax.CategoryDto.Category.ID}" style="width:90px;font-size:12px;" class="btn btn-primary btn-edit"><span class="fas fa-edit"></span> Duzenle</button>
                                    <button style="width:90px;font-size:12px;" data-id="${categoryUpdateAjax.CategoryDto.Category.ID}" class="btn-delete btn btn-danger"><span class="fas fa-minus-circle"></span> Kaldir</button>
                                </td>
                                                </tr>
                                `;
                    } else {
                        tableRowN = `
                        <tr name="${categoryUpdateAjax.CategoryDto.Category.ID}">
                                                    <td>${categoryUpdateAjax.CategoryDto.Category.ID}</td>
                                                    <td>${categoryUpdateAjax.CategoryDto.Category.Name}</td>
                                                    <td>${convertFirstToUpper(categoryUpdateAjax.CategoryDto.Category.IsActive.toString())}</td>
                                                    <td>${categoryUpdateAjax.CategoryDto.Category.Note}</td>
                                                    <td>${convertToShortDate(categoryUpdateAjax.CategoryDto.Category.CreatedDate)}</td>
                                                    <td>${categoryUpdateAjax.CategoryDto.Category.CreatedByName}</td>
                                                    <td>${convertToShortDate(categoryUpdateAjax.CategoryDto.Category.ModifiedDate)}</td>
                                                    <td>${categoryUpdateAjax.CategoryDto.Category.ModifiedByName}</td>
                                                    <td>${categoryUpdateAjax.CategoryDto.Category.Articles.$values.length}</td>
                                                    <td style="max-width:200px;min-width:100px;text-align:center;">
                                    <button data-id="${categoryUpdateAjax.CategoryDto.Category.ID}" style="width:90px;font-size:12px;" class="btn btn-primary btn-edit"><span class="fas fa-edit"></span> Duzenle</button>
                                    <button style="width:90px;font-size:12px;" data-id="${categoryUpdateAjax.CategoryDto.Category.ID}" class="btn-delete btn btn-danger"><span class="fas fa-minus-circle"></span> Kaldir</button>
                                </td>
</tr>
                                `;
                    }
                    const newTable = $(tableRowN);
                    const categoryTableRow = $(`[ name = "${categoryUpdateAjax.CategoryDto.Category.ID}"`);
                    newTable.hide();
                    categoryTableRow.replaceWith(newTable);
                    newTable.fadeIn(1500);
                    toastr.success(`${categoryUpdateAjax.CategoryDto.Message}`, "Basarili islem");
                } else {
                    let summaryText = '';
                    $('#validation-summary  ul  li').each(function () {
                        let text = $(this).text();
                        summaryText += `*${text}\n`;
                    });
                    toastr["warning"](summaryText);
                }
            }).fail(function (err) {
                toastr.error('Hata');
            });
        });
    });
});