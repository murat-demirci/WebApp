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
   const dataTable = $('#usersTable').DataTable({
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
                        url: '/Admin/User/GetAllUsers/',
                        contentType: 'application/json',
                        beforeSend: function () {
                            $('#usersTable').attr('style', 'opacity:0.1;user-select:none;');
                            $('iframe').removeClass('d-none');

                        },
                        success: function (data) {
                            const userListDto = jQuery.parseJSON(data);
                            dataTable.clear();
                            if (userListDto.resultStatus === 0) {
                                $.each(userListDto.Users.$values,
                                    function (index, user) {
                                        dataTable.row.add([
                                            `<img src="/img/${user.UserPicture}" style="max-height:50px;max-width:50px;"/>`,
                                            user.Id,
                                            user.UserName,
                                            user.Email,
                                            `
                               
                                    <button data-id="${user.Id}" class="btn btn-primary btn-edit" style="width: 90px; font-size: 12px;"><span class="fas fa-edit"></span> Duzenle</button>
                                    <button data-id="${user.Id}" style="width: 90px; font-size: 12px;" class=" btn btn-danger  btn-delete"><span class="fas fa-minus-circle"></span> Kaldir</button>
                                
                            `

                                        ]);
                                    });
                                dataTable.draw();
                                setTimeout(function () {
                                    $('#usersTable').removeAttr('style');
                                    $('iframe').addClass('d-none');
                                }, 1000);
                            }
                            else {
                                toastr.error(`${userListDto.Message}`, 'İşlem Başarısız!');
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
        const url = '/Admin/User/Add/';
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
            const form = $('#form-user-add');
            const actionUrl = form.attr('action');//asp-action urlsine gider
            const dataToSend = new FormData(form.get(0)); //gonderilecek veiriyi useradddto olarak donusturduk
            $.ajax({
                url: actionUrl,
                type: 'POST',
                data: dataToSend,
                processData: false,
                contentType:false,
                success: function(data) {
                    const ajaxAddModel = jQuery.parseJSON(data);//data model
                    const newFormBody = $('.modal-body', ajaxAddModel.UserAddPartial);//formun oldugu kismi alicaz
                    placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                    const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                    if (isValid) {
                        placeHolderDiv.find('.modal').modal('hide');
                        dataTable.row.add([
                            `<img src="/img/${ajaxAddModel.UserDto.User.UserPicture}" style="max-height:50px;max-width:50px;"/>`,
                            ajaxAddModel.UserDto.User.Id,
                            ajaxAddModel.UserDto.User.UserName,
                            ajaxAddModel.UserDto.User.Email,
                            `
                               
                                    <button data-id="${ajaxAddModel.UserDto.User.Id}" class="btn btn-primary btn-edit" style="width: 90px; font-size: 12px;"><span class="fas fa-edit"></span> Duzenle</button>
                                    <button data-id="${ajaxAddModel.UserDto.User.Id}" style="width: 90px; font-size: 12px;" class=" btn btn-danger  btn-delete"><span class="fas fa-minus-circle"></span> Kaldir</button>
                                
                            `

                        ]).draw();
                        toastr["success"]('Basarili Islem', `${ajaxAddModel.UserDto.Message}`);
                    }
                    else {
                        console.log(ajaxAddModel);
                        let summaryText = '';
                        $('#validation-summary  ul  li').each(function () {
                            let text = $(this).text();
                            summaryText += `*${text}\n`;
                        });
                        toastr["warning"](summaryText);
                    }
                },
                error: function (err) {
                    console.log(err);
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
        const userName = tblRow.find('td:eq(2)').text();//2.siradaki table data
        Swal.fire({
            title: `${userName}\nSilmek istediginize emin misniz?`,
            text: "Bu islem geri alinamaz",
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
                    url: '/Admin/User/Remove/',
                    data: { userId: id },
                    success: function (data) {
                        const userDto = jQuery.parseJSON(data);
                        console.log(userDto.User);
                        if (userDto.resultStatus === 0) {
                            Swal.fire(
                                "Basarili Islem",
                                userDto.Message,
                                'success'
                            );
                            dataTable.row(tblRow).remove().draw();
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Basarisiz islem!',
                                text: userDto.Message
                            });
                        }
                    },
                    error: function (err) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Beklenmedik bir hata olustu',
                            text: userDto.Message + " " + err.responseText,
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