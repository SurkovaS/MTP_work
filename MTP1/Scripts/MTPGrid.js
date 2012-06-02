function DrawMTPGrid(gridId, pagerId,
                        indexUrl, editUrl, deleteUrl,
                        addUrl, viewUrl, colNames, colModel, caption,
                        hiddengrid) {

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
        altRows: true,
        altclass: 'myAltRowClass',
        height: 'auto',
        gridview: true,
        url: indexUrl,
        datatype: 'json',
        mtype: 'GET',
        caption: caption,
        hiddengrid: hiddengrid? hiddengrid : false,
        ondblClickRow: function (rowid) {
            window.location = viewUrl + '/' + rowid;
        },
        loadError: function (jqXHR, textStatus, errorThrown) {
        alert('HTTP status code: ' + jqXHR.status + '\n' +
              'textStatus: ' + textStatus + '\n' +
              'errorThrown: ' + errorThrown);
        alert('HTTP message body (jqXHR.responseText): ' + '\n' + jqXHR.responseText);
    }
    });

    var grid = jQuery("#" + gridId).jqGrid('navGrid', '#' + pagerId, {
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
            if (confirm('Вы действительно хотите удалить запись?')) {
                $.post(deleteUrl + '/' + id, function () {
                    grid.trigger("reloadGrid");
                });
            }
        }
    });
     
    ResizeGrid($("#" + gridId));
}

function ResizeGrid(grid) {
    // Get width of parent container
    var width = jQuery(grid).closest('div[id*="grid_container"]').width();
    if (width) {
        jQuery(grid).setGridWidth(width + 5);
    }
}