(function () {
    function StartDataTable(table) {
        console.log(`StartDataTable called. ${table}`);

        $(`#${table}`).DataTable();
    }

    window.myNamespace = window.myNamespace || {};
    window.myNamespace.onLoad = {
        StartDataTable: StartDataTable
    };

})();
