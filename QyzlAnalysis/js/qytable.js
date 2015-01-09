
$(document).ready(function () {
    getxaxisParam();
    getyaxisParam();
    getSeriesData();
    createTable();
})
var myChart_Category;
$(function () {
    $(".zlblr").click(function () {
        if (!$("#newdiv").hasClass("dis")) {
            $("#newdiv").addClass("dis");
        }
        if ($("#olddiv").hasClass("dis")) {
            $("#olddiv").removeClass("dis");
        }
        $("#subSection").html("");
        $("#dtid").val($(this).attr("deptid"))
        xaxisParam = new Array();
        yaxisParam = new Array();
        seriesData = new Array();
        legendData = new Array();
        datanum = new Array();
        dataname = new Array();
        getxaxisParam();
        getyaxisParam();
        if (dataType != 1) {
            getSeriesData();
        } else {
            getPieSeries();
        }
        $("#editName").html(titleData);
        mainoption = {
            tooltip: {
                trigger: 'axis'
            },
            toolbox: {
                show: true,
                feature: {
                    mark: { show: false },
                    dataView: { show: true, readOnly: false },
                    magicType: { show: true, type: ['line', 'bar'] },
                    restore: { show: true },
                    saveAsImage: { show: false }
                }
            },
            legend: {
                data: legendData
            },
            xAxis: xaxisParam,
            yAxis: yaxisParam,
            series: seriesData
        }
        require([
            'echarts',
            'echarts/chart/bar',
            'echarts/chart/line'
        ],
        function (ec) {
            var myChart_Category = ec.init(document.getElementById('div_pieCategory'));
            myChart_Category.setOption(mainoption);
        })
        createTable();
    })
})


var xaxisParam = new Array();
var yaxisParam = new Array();
var seriesData = new Array();
var legendData = new Array();
var titleData = "10年地区GDP"
var mainoption = {
    tooltip: {
        trigger: 'axis'
    },
    toolbox: {
        show: true,
        feature: {
            mark: { show: false },
            dataView: { show: true, readOnly: false },
            magicType: { show: true, type: ['line', 'bar'] },
            restore: { show: true },
            saveAsImage: { show: true }
        }
    },
    legend: {
        data: legendData
    },
    xAxis: xaxisParam,
    yAxis: yaxisParam,
    series: seriesData
}
var titleData = "";
var dataType = 0;
var xaxisParamdatalist = new Array();
function getxaxisParam() {
    xaxisParamdatalist = new Array()
    var xaxisParamdataobj = new Object();
    $.ajax({ type: "post", url: "../Xaxis/GetXaxis", data: { "dataTypeid": $("#dtid").val() }, dataType: "json", async: false, success: function (result) {
        $(result).each(function () {
            xaxisParamdatalist.push(this.year);
        })
        if (xaxisParamdatalist.length == 1) { dataType = 1 } else {
            dataType = 0;
            xaxisParamdataobj.data = xaxisParamdatalist;
            xaxisParamdataobj.type = "category";
            xaxisParam.push(xaxisParamdataobj);
        }
        titleData = result[0].title}
    })}
function getyaxisParam() {
    $.ajax({ type: "post", url: "../Xaxis/GetYaxis", data: { "dataTypeid": $("#dtid").val(), "datatyepe": dataType }, dataType: "json", async: false, success: function (result) {
        $(result).each(function () {
            var yaxisParamdataobj = new Object();
            yaxisParamdataobj.type = "value";
            yaxisParamdataobj.name = this.unit;
            yaxisParam.push(yaxisParamdataobj);})}})}
function getSeriesData() {
    $.ajax({ type: "post", url: "../Xaxis/GetSeries", data: { "dataTypeid": $("#dtid").val() }, dataType: "json", async: false, success: function (result) {
        $(result).each(function () {
            var seriesParamdatalist = new Array();
            var seriesParamdataobj = new Object();
            var snum = this.num.split("_");
            if (this.stack.indexOf("%") == -1) {
                for (var i = 0; i < snum.length; i++) {
                    var valobj = new Object();
                    var tooltipobj = new Object();
                    tooltipobj.formatter = this.stack + ":<br/>{a}<br/>{b}:{c}";
                    valobj.value = snum[i];
                    valobj.tooltip = tooltipobj;
                    seriesParamdatalist.push(valobj);
                }
            } else {
                for (var i = 0; i < snum.length; i++) {
                    seriesParamdatalist.push(snum[i]);
                }
            }
            seriesParamdataobj.data = seriesParamdatalist
            seriesParamdataobj.yAxisIndex = this.unit
            if (this.unit == 1) {
                seriesParamdataobj.type = "line";
            } else {
                seriesParamdataobj.type = "bar";
                if (this.stack.indexOf("%") == -1) {
                    seriesParamdataobj.stack = this.stack
                }
                seriesParamdataobj.barWidth = 20;
            }
            datanum.push(this.num)
            dataname.push(this.name)
            seriesParamdataobj.name = this.name;
            legendData.push(this.name)
            seriesData.push(seriesParamdataobj);
        })
    }
    })
}
function getPieSeries() {
    var xaxisParamdatalist1 = new Array();
    datanum = new Array();
    dataname = new Array();
    var seriesParamdatal = new Array();
    var seriesParamdatao = new Object();
    var xaxisParamdataobj = new Object();
    $.ajax({ type: "post", url: "../Xaxis/GetOtherSeries", data: { "dataTypeid": $("#dtid").val(), "dataType": dataType }, dataType: "json", async: false, success: function (result) {
        $(result).each(function () {
            //legendData.push(this.name)
            xaxisParamdatalist1.push(this.name)
            seriesParamdatal.push(this.num)
            if (this.unit == 1) {
                seriesParamdatao.type = "line";
            } else {
                seriesParamdatao.type = "bar";
                seriesParamdatao.barWidth = 20;
            }
        })
        datanum = seriesParamdatal;
        dataname = xaxisParamdatalist1;
        xaxisParamdataobj.data = xaxisParamdatalist1;
        xaxisParamdataobj.type = "category";
        seriesParamdatao.name = xaxisParamdatalist[0]
        seriesParamdatao.data = seriesParamdatal;
        xaxisParam.push(xaxisParamdataobj);
        seriesData.push(seriesParamdatao)
    }
    })
}
var datanum = new Array(); var dataname = new Array();
function createTable1() {
    var tablehtml = "<thead><tr>";
    tablehtml += "<th>年份/类型</th>";
    for (var fristrow = 0; fristrow < dataname.length; fristrow++) {
        tablehtml += "<td><label onclick=rowclick('cloumn','row');><input type='checkbox' name='cloumn' value='" + $("#dtid").val() + "_" + dataname[fristrow] + "'>" + dataname[fristrow] + "</label></td>";
    }
    tablehtml += "</tr></thead><tbody>";
    for (var tr1 = 0; tr1 < xaxisParamdatalist.length; tr1++) {
        tablehtml += "<tr><th><label onclick=rowclick('row','cloumn');><input type='checkbox' name='row' value='" + $("#dtid").val() + "_" + xaxisParamdatalist[tr1] + "'>" + xaxisParamdatalist[tr1] + "</label></th>"
        for (var tdnum = 0; tdnum < datanum.length; tdnum++) {
            var datanumber = datanum[tdnum]
            tablehtml += "<td><input name='dtnum' cloumnid='" + $("#dtid").val() + "_" + dataname[tdnum] + "' rowid='" + $("#dtid").val() + "_" + xaxisParamdatalist[tr1] + "' type='text' value=" + datanumber.split("_")[tr1] + " /></td>"
        }
        tablehtml += "</tr>";
    }
    tablehtml += "</tbody>";
    $("#tableNumber").html(tablehtml)
    datanum = new Array();
    dataname = new Array();
}
function createTable() {
    var tablehtml = "<thead><tr>";
    tablehtml += "<th>年份/类型</th>";
    for (var fristrow = 0; fristrow < dataname.length; fristrow++) {
        tablehtml += "<td><label>" + dataname[fristrow] + "</label></td>";
    }
    tablehtml += "</tr></thead><tbody>";
    for (var tr1 = 0; tr1 < xaxisParamdatalist.length; tr1++) {
        tablehtml += "<tr><th><label>" + xaxisParamdatalist[tr1] + "</label></th>"
        for (var tdnum = 0; tdnum < datanum.length; tdnum++) {
            var datanumber = datanum[tdnum]
            var numdata = datanumber.split("_")[tr1];
            if (numdata != "") { } else { numdata="0" }
            tablehtml += "<td>" + numdata + "</td>"
        }
        tablehtml += "</tr>";
    }
    tablehtml += "</tbody>";
    $("#tableNumber").html(tablehtml)
    
}
require([
    'echarts',
    'echarts/chart/bar',
    'echarts/chart/line'
],
function (ec) {
    var myChart_Category = ec.init(document.getElementById('div_pieCategory'));
    myChart_Category.setOption(mainoption);
})
function qytableimportexecl() {
    var tabhtmlval = "年份/类型;";
    var tabvalList = new Array();
    for (var fristrow = 0; fristrow < dataname.length; fristrow++) {
        tabhtmlval += dataname[fristrow] + ";";
    }
    tabhtmlval = tabhtmlval.substring(0, tabhtmlval.length - 1);
    tabvalList.push(tabhtmlval); tabhtmlval = "";
    for (var tr1 = 0; tr1 < xaxisParamdatalist.length; tr1++) {
        tabhtmlval += xaxisParamdatalist[tr1]+";"
        for (var tdnum = 0; tdnum < datanum.length; tdnum++) {
            var datanumber = datanum[tdnum]
            var numdata = datanumber.split("_")[tr1];
            if (numdata != "") { } else { numdata = "0" }
            tabhtmlval += numdata + ";"
        }
        tabhtmlval = tabhtmlval.substring(0, tabhtmlval.length - 1);
        tabvalList.push(tabhtmlval);tabhtmlval = "";
    }
    $.getJSON("../CustomData/ExcelOutput", "data=" + tabvalList + "", function (jsobj) {
        window.location = "http://" + window.location.host + jsobj;
    });
}