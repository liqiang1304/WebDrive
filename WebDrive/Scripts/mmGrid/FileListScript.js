$(document).ready(function () {

    var fixed2 = function (val) {
        if (typeof val != 'number') {
            return '';
        }
        return val.toFixed(2);
    }

    var cols = [
        {
            title: 'Icon', name: 'ICON', width: 30, align: 'center', lockWidth: true, renderer: function (val, item, rowIndex) {
                if (item.Directory) {
                    return '<div class="btnDir"></div>';
                } else {
                    return '<div class="btnFile"></div>';
                }
            }
        },
        {
            title: 'File Name', name:'FileName', width:230, sortable:true, align:'left'
        },
        {
            title: 'Size', name:'FileSize', width:60, sortable:true, type: 'number', align:'center'
        },
        {
            title: 'Create Date', name:'CreateDate', width:100, sortable:true, align:'center'
        },
        {
            title: 'Operation', name: '', width: 90, align: 'center', lockWidth: true, lockDisplay: true, renderer: function (val, item, rowIndex) {
                if (item.Directory) {
                    return '<button  class="btn btn-info">&nbsp&nbsp&nbsp Open &nbsp&nbsp&nbsp</button>';
                } else {
                    return '<button  class="btn btn-info">Download</button>'
                }
            }
        }
    ];

    var mmg = $('.mmg').mmGrid({
        height: 400
        , cols: cols
        , url: '/UserFile/GetUserDir'
        , method: 'get'
        , remoteSort: true
        //, items: items
        , params: {ParentID: 0}
        , sortName: 'FileName'
        , sortStatus: 'asc'
        , multiSelect: true
        , checkCol: true
        , fullWidthRows: true
        , autoLoad: false
        , plugins: [
            $('#pg').mmPaginator({})
        ]
        , params: function () {
            //如果这里有验证，在验证失败时返回false则不执行AJAX查询。
            return {
                secucode: $('#secucode').val()
            }
        }
    });


    mmg.on('cellSelected', function (e, item, rowIndex, colIndex) {
        console.log('cellSelected!');
        console.log(this);
        console.log(e);
        console.log(item);
        console.log(rowIndex);
        console.log(colIndex);
        //查看
        if ($(e.target).is('.btn-info, .btnPrice')) {
            e.stopPropagation();  //阻止事件冒泡
            //alert(JSON.stringify(item));
            if (item.Directory) {
                mmg.load({ ParentID: item.UserFileID });
            }
        } else if ($(e.target).is('.btn-danger') && confirm('你确定要删除该行记录吗?')) {
            e.stopPropagation(); //阻止事件冒泡
            mmg.removeRow(rowIndex);
        }
    }).on('loadSuccess', function (e, data) {
        //这个事件需要在数据加载之前注册才能触发
        console.log('loadSuccess!');
        console.log(data);
        console.log(mmg.rowsLength());
        window.currentDir = mmg.row(mmg.rowsLength() - 1);
        mmg.removeRow(mmg.rowsLength() - 1);
    }).on('rowInserted', function (e, item, index) {
        console.log('rowInserted!');
        console.log(item);
        console.log(index);
        console.log(mmg.rowsLength());
    }).on('rowUpdated', function (e, oldItem, newItem, index) {
        console.log('rowUpdated!');
        console.log(oldItem);
        console.log(newItem);
        console.log(index);
    }).on('rowRemoved', function (e, item, index) {
        console.log('rowRemoved!');
        console.log(item);
        console.log(index);
        console.log(mmg.rowsLength());
    }).load();



    var item1 = { AMPLITUDE: 2.069, PREVCLOSINGPRICE: 2.9, TURNOVERDEALS: 0, HIGHESTPRICE: 2.95, TURNOVERVOL: 2511165, TRADINGDAY: 1345478400000, TOTALSHARE: 2000000000, SECUCODE: "100000", EPS: 0.0266, LOWESTPRICE: 2.89, OPENINGPRICE: 2.9, SECUABBR: "新加股票", ALISTEDSHARE: 2000000000, ID: 3131903, WCOSTAVG: 3.5362, NETCASHFLOWOPERPS: -0.4041, SECUNAME: "东风汽车股份有限公司", CLOSINGPRICE: 2.9, DAYCHANGERATE: 0, TURNOVERVAL: 7316381, BVPS: 3.0533, DAYCHANGE: 0, PE: 12.4963, TURNOVERRATE: 0.1256, ADJUSTCLOSINGPRICE: 10.0741, PB: 0.9581 };
    var item2 = { AMPLITUDE: 0.7389, PREVCLOSINGPRICE: 12.18, TURNOVERDEALS: 0, HIGHESTPRICE: 12.22, TURNOVERVOL: 1332194, TRADINGDAY: 1345478400000, TOTALSHARE: 1926958448, SECUCODE: "100001", EPS: 0.1909, LOWESTPRICE: 12.13, OPENINGPRICE: 12.19, SECUABBR: "新加股票2", ALISTEDSHARE: 1093476397, ID: 3131171, WCOSTAVG: 12.8369, NETCASHFLOWOPERPS: -0.04, SECUNAME: "上海国际机场股份有限公司", CLOSINGPRICE: 12.14, DAYCHANGERATE: -0.3284, TURNOVERVAL: 16207539, BVPS: 8.16, DAYCHANGE: -0.04, PE: 15.5997, TURNOVERRATE: 0.1218, ADJUSTCLOSINGPRICE: 33.1878, PB: 1.523 };

    $('#btnUp').on('click', function () {
        var url = "/UserFile/GetParentDirID";
        var data = {
            currentParentID: window.currentDir.currentDirID
        };
        $.get(url, data, function (response) {
            mmg.load({ ParentID: response.ParentDirID });
        });
    });

    $('#btnNewDir').on('click', function () {
        var name = prompt("Please input new directory name:");
        if (name != null && name != "") {
            var url = "/UserFile/CreateNewDir";
            var data = {
                dirName : name,
                parentID: window.currentDir.currentDirID
            };
            $.get(url, data, function (response) {
                if (response.success) {
                    mmg.load({ ParentID: window.currentDir.currentDirID });
                } else {
                    alert("Create directory failed!");
                }
            });
        }
    });

    $('#btnSearch').on('click', function () {
        //点击查询时页码置为1
        mmg.load({ page: 1 });
    });


});