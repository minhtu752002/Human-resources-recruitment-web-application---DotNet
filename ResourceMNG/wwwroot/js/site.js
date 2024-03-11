// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//$(function () {
//    var PlaceHolderEl = $('#PlaceHoderHere')
//    $('button[data-toggle="ajax-modal"]').click(function (event) {
//        var url = $(this).data('url');
//        $.get(url).done(function (data) {
//            PlaceHolderEl.html(data);
//            PlaceHolderEl.find('.modal').modal('show')
//        })
//    })
//})
//$(function () {
//    var del = $('#DeleteModal')
//    $('a[data-toggle="Delete-modal"]').click(function (event) {
//        var url = $(this).data('url');
//        $.get(url).done(function (data) {
//            del.html(data);
//            del.find('.modal').modal('show')
//        })
//    })
//})
$(function () {
    function showModalContent(url, placeholderEl) {
        $.get(url).done(function (data) {
            placeholderEl.html(data);
            placeholderEl.find('.modal').modal('show');
        });
    }

    var placeHolderEl = $('#PlaceHoderHere');
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        showModalContent(url, placeHolderEl);
    });

    var del = $('#DeleteModal');
    $('a[data-toggle="Delete-modal"]').click(function (event) {
        var url = $(this).data('url');
        showModalContent(url, del);
    });

    var delEmp = $('#DeleteModalEmployee')
    $('a[data-toggle="Delete-modal-emp"]').click(function (event) {
        var url = $(this).data('url');
        showModalContent(url, delEmp);

    })
    var delVacacy = $('#DeleteModalVacacy')
    $('a[data-toggle="Delete-modal-vacacy"]').click(function (event) {
        var url = $(this).data('url');
        showModalContent(url, delVacacy);

    })
    var delApplicant = $('#DeleteModalApplicant')
    $('a[data-toggle="Delete-modal-applicant"]').click(function (event) {
        var url = $(this).data('url');
        showModalContent(url, delApplicant);

    })
    

});

