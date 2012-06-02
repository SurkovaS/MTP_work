function DrawMTPGrid(gridId, pagerId, indexUrl, editUrl, deleteUrl, addUrl, viewUrl, colNames, colModel) {

    $("#" + gridId).jqGrid({
        colNames: colNames,
        colModel: colModel,
        pager: $("#" + pagerId),
        sortname: 'Title',
        sortorder: "",
        rowNum: 10,
        rowList: [10, 20, 50],
        viewrecords: true,
        rownumbers: true,
        shrinkToFit: true,
        width: 1100,
        altRows: true,
        altclass: 'myAltRowClass',
        height: 'auto',
        gridview: true,
        url: indexUrl,
        datatype: 'json',
        mtype: 'GET',
        caption: 'Программы тестирования',
        loadError: function (xhr, status, error) {
            alert(error);
        },
        ondblClickRow: function (rowid) {
            window.location = viewUrl + '/' + rowid;
        }
    });

    var grid = jQuery("#grid").jqGrid('navGrid', '#' + pagerId, {
        edit: true,
        add: true,
        del: true,
        search: false,
        refresh: true,
        addfunc: function () { window.location = addUrl; },
        editfunc: function (id) {
            window.location = editUrl + '/' + id;
        },
        delfunc: function (id) {
            $.post(deleteUrl + '/' + id, function () {
                grid.trigger("reloadGrid");
            }).error(function (readyState, responceText) { alert(responceText); });
        }
    });
    
}