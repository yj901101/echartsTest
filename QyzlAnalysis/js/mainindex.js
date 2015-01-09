$(document).ready(function () {
    getxaxisParam();
    getyaxisParam();
    getSeriesData();
})
require([
    'echarts',
    'echarts/chart/bar',
    'echarts/chart/line'
],
function (ec) {
    var myChart_Category = ec.init(document.getElementById('div_pieCategory'));
    myChart_Category.setOption(mainoption);
})
var xaxisParam = new Array();
var yaxisParam = new Array();
var seriesData = new Array();
var legendData = new Array();
var mainoption = {
    title: {
        text: titleData
    },
    tooltip : {
        trigger: 'axis'
    },
    toolbox: {
        show : true,
        feature : {
            mark : {show: true},
            dataView : {show: true, readOnly: false},
            magicType: {show: true, type: ['line', 'bar']},
            restore : {show: true},
            saveAsImage : {show: true}
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
function getxaxisParam() {
    var xaxisParamdatalist = new Array();
    var xaxisParamdataobj = new Object();
    $.ajax({type: "post",url: "../Xaxis/GetXaxis",data: {"dataTypeid": 1},dataType: "json",async: false,success: function (result) {
        $(result).each(function () {
                xaxisParamdatalist.push(this.year);
            })
            titleData = result[0].title
            xaxisParamdataobj.data = xaxisParamdatalist;
            xaxisParamdataobj.type = "category";
            xaxisParam.push(xaxisParamdataobj);
        }
    })
}
function getyaxisParam() {
    $.ajax({type: "post",url: "../Xaxis/GetYaxis",data: { "dataTypeid": 1 },dataType: "json",async: false,success: function (result) {
            $(result).each(function () {
                var yaxisParamdataobj = new Object();
                yaxisParamdataobj.type = "value";
                yaxisParamdataobj.name = this.unit;
                yaxisParam.push(yaxisParamdataobj);
            })
        }
    })
}
function getSeriesData() {
    $.ajax({ type: "post", url: "../Xaxis/GetSeries", data: { "dataTypeid": 1 }, dataType: "json", async: false, success: function (result) {
        $(result).each(function () {
            var seriesParamdatalist = new Array();
            var seriesParamdataobj = new Object();
            var snum = this.num.split("_");
            for (var i = 0; i < snum.length; i++) {
                seriesParamdatalist.push(snum[i]);
            }
            seriesParamdataobj.data = seriesParamdatalist
            seriesParamdataobj.yAxisIndex = this.unit
            if (this.unit == 1) {
                seriesParamdataobj.type = "line";
            } else {
                seriesParamdataobj.type = "bar";
            }
            seriesParamdataobj.name = this.name;
            legendData.push(this.name)
            seriesData.push(seriesParamdataobj);
        })
    }
    })
}