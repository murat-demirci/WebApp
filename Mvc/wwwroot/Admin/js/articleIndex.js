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
   const dataTable = $('#articlesTable').DataTable({
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
                    let url = window.location.href;
                    url = url.replace("/Index", "");
                    window.open(`${url}/Add`, "_self");
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
                                        (user.EmailConfirmed==true ? "Dogrulandi" : "Dogrulanmadi"),
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

    //Delete
    $(document).on('click', '.btn-delete', function (e) {
        e.preventDefault();
        const id = $(this).attr('data-id');
        const tblRow = $(`[name=${id}]`);
        const articleTitle = tblRow.find('td:eq(1)').text();//(0'dan başlayarak') 1.siradaki table data name old. için
        Swal.fire({
            title: `${articleTitle}\nbaşlıklı makaleyi silmek istediğinizden emin misiniz?`,
            text: "Bu işlem geri alınamaz!",
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
                    url: '/Admin/Article/Delete/',
                    data: { articleId: id },
                    success: function (data) {
                        const articleResult = jQuery.parseJSON(data);
                        if (articleResult.resultStatus === 0) {
                            Swal.fire(
                                "Başarılı İşlem",
                                `${articleResult.Message}`,
                                'success'
                            );
                            dataTable.row(tblRow).remove().draw();
                        } else {
                            console.log(articleResult.resultStatus);
                            Swal.fire({
                                //silme işlemini gerçekleştirip hata mesajı veridği için
                                //her iki durumda da aynı mesaj verilmesi geçici olarak sağlanmıştır
                                icon: 'success',
                                title: 'Başarılı İşlem',
                                text: `${articleResult.Message}`,
                            });
                            tblRow.fadeOut(1500);
                        }
                    },
                    error: function (err) {
                        //Swal.fire({
                        //    icon: 'error',
                        //    title: 'Beklenmedik bir hata olustu',
                        //    text: articleResult.Message + " " + err.responseText,
                        //})
                        console.log(err);
                        toastr.error(`${err.responseText}`);
                    }
                })
            }
        })
    });
    //Delete

});