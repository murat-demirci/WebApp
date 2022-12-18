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
                            $('#loader').removeClass('d-none');

                        },
                        success: function (data) {
                            const userListDto = jQuery.parseJSON(data);
                            dataTable.clear();
                            if (userListDto.resultStatus === 0) {
                                $.each(userListDto.Users.$values,
                                    function (index, user) {
                                    const newTableRow = dataTable.row.add([
                                            `<img src="/img/${user.UserPicture}" class="my-image-table"/>`,
                                            user.Id,
                                            user.UserName,
                                        user.Email,
                                            `<td>${(user.EmailConfirmed==true ? "Dogrulandi" : "Dogrulanmadi")}</td>`,
                                            `
                               
                                    <button data-id="${user.Id}" class="btn btn-primary btn-edit" style="width: 90px; font-size: 12px;"><span class="fas fa-edit"></span> Duzenle</button>
                                    <button data-id="${user.Id}" style="width: 90px; font-size: 12px;" class=" btn btn-danger  btn-delete"><span class="fas fa-minus-circle"></span> Kaldir</button>
                                
                            `

                                    ]).node();
                                        const jqTableRow = $(newTableRow);
                                        jqTableRow.attr("name", `${user.Id}`);
                                    });
                                dataTable.draw();
                                setTimeout(function () {
                                    $('#usersTable').removeAttr('style');
                                    $('#loader').addClass('d-none');
                                }, 1000);
                            }
                            else {
                                toastr.error(`${userListDto.Message}`, 'İşlem Başarısız!');
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            $('#usersTable').removeAttr('style');
                            $('#loader').addClass('d-none');
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
                        const newTableRow=dataTable.row.add([
                            `<img src="/img/${ajaxAddModel.UserDto.User.UserPicture}" class="my-image-table"/>`,
                            ajaxAddModel.UserDto.User.Id,
                            ajaxAddModel.UserDto.User.UserName,
                            ajaxAddModel.UserDto.User.Email,
                            `<td>${(ajaxAddModel.userDto.User.EmailConfirmed == true ? "Dogrulandi" : "Dogrulanmadi")}</td>`,
                            `
                               
                                    <button data-id="${ajaxAddModel.UserDto.User.Id}" class="btn btn-primary btn-edit" style="width: 90px; font-size: 12px;"><span class="fas fa-edit"></span> Duzenle</button>
                                    <button data-id="${ajaxAddModel.UserDto.User.Id}" style="width: 90px; font-size: 12px;" class=" btn btn-danger  btn-delete"><span class="fas fa-minus-circle"></span> Kaldir</button>
                                
                            `

                        ]).node();
                        const jqTableRow = $(newTableRow);
                        jqTableRow.attr("name", `${ajaxAddModel.UserDto.User.Id}`);
                        dataTable.row(newTableRow).draw();
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
        const url = '/Admin/User/Update/';
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click',
            '.btn-edit',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                $.get(url, { userId: id }).done(function (data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find('.modal').modal('show');
                }).fail(function () {
                    toastr.error("Bir hata oluştu.");
                });
            });
        //post edit
        placeHolderDiv.on('click', '#btnUpdate', function (e) {
            e.preventDefault();
            const form = $('#form-user-update');
            const actionUrl = form.attr('action');
            const dataToSend = new FormData(form.get(0)); //gonderilecek veiriyi userupdateto olarak donusturduk
            $.ajax({
                url: actionUrl,
                type: 'POST',
                data: dataToSend,
                processData: false,
                contentType: false,
                success: function (data) {
                    const userUpdateAjax = jQuery.parseJSON(data);
                    console.log(userUpdateAjax);
                    const id = userUpdateAjax.UserDto.User.Id;
                    const tableRow = $(`[name="${id}"]`)
                    const newForm = $('.modal-body', userUpdateAjax.UserUpdatePartial);
                    placeHolderDiv.find('.modal-body').html(newForm);
                    const isValid = newForm.find('[name="IsValid"]').val() === 'True';
                    if (isValid) {
                        placeHolderDiv.find('.modal').modal('hide');
                        dataTable.row(tableRow).data([
                            `<img src="/img/${userUpdateAjax.UserDto.User.UserPicture}" class="my-image-table"/>`,
                            userUpdateAjax.UserDto.User.Id,
                            userUpdateAjax.UserDto.User.UserName,
                            userUpdateAjax.UserDto.User.Email,
                            `<td>${(userUpdateAjax.userDto.User.EmailConfirmed == true ? "Dogrulandi" : "Dogrulanmadi")}</td>`,
                            `
                               
                                    <button data-id="${userUpdateAjax.UserDto.User.Id}" class="btn btn-primary btn-edit" style="width: 90px; font-size: 12px;"><span class="fas fa-edit"></span> Duzenle</button>
                                    <button data-id="${userUpdateAjax.UserDto.User.Id}" style="width: 90px; font-size: 12px;" class=" btn btn-danger  btn-delete"><span class="fas fa-minus-circle"></span> Kaldir</button>
                                
                            `

                        ]);
                        tableRow.attr("name", `${id}`);
                        dataTable.row(tableRow).invalidate();//datatable verileri tekrar kontrtol eder
                        toastr.success(`${userUpdateAjax.UserDto.Message}`, "Basarili islem");
                    } else {
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
    });
});