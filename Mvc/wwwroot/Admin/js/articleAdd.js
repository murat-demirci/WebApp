$(document).ready(function () {

    //trumbowgy
    $('#text-editor').trumbowyg({
        btns: [
            ['viewHTML'],
            ['undo', 'redo'], // Only supported in Blink browsers
            ['formatting'],
            ['strong', 'em', 'del'],
            ['superscript', 'subscript'],
            ['link'],
            ['insertImage'],
            ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
            ['unorderedList', 'orderedList'],
            ['horizontalRule'],
            ['removeformat'],
            ['fullscreen'],
            ['foreColor', 'backColor'],
            ['emoji'],
            ['fontSize'],
            ['fontfamily']
            
        ],
        plugins: {
            colors: {
                foreColorList: [
                    'ff0000', '00ff00', '0000ff'
                ],
                backColorList: [
                    '000', '333', '555'
                ],
                displayAsList: false
            },
            fontsize: {
                sizeList: [
                    '12px',
                    '14px',
                    '16px',
                    '18px'
                ]
            },
            fontfamily: {
                fontList: [
                    { name: 'Arial', family: 'Arial, Helvetica, sans-serif' },
                    { name: 'Open Sans', family: '\'Open Sans\', sans-serif' }
                ]
            },
            
        }
        });
    //trumbowgy

    //select2
    $('#categoryList').select2({
        placeholder: "Bir kategori seçiniz",
        allowClear: true
        });
    //select2

    //jquery ui - datepicker
    $("#datepicker").datepicker({
        closeText: "kapat",
        prevText: "&#x3C;geri",
        nextText: "ileri&#x3e",
        currentText: "bugün",
        monthNames: ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran",
            "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"],
        monthNamesShort: ["Oca", "Şub", "Mar", "Nis", "May", "Haz",
            "Tem", "Ağu", "Eyl", "Eki", "Kas", "Ara"],
        dayNames: ["Pazar", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi"],
        dayNamesShort: ["Pz", "Pt", "Sa", "Ça", "Pe", "Cu", "Ct"],
        dayNamesMin: ["Pz", "Pt", "Sa", "Ça", "Pe", "Cu", "Ct"],
        weekHeader: "Hf",
        dateFormat: "dd.mm.yy",
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: "",
        duration: 1000,
        minDate: 0,
        maxDate: +3,
        });
    //jquery ui - datepicker
})