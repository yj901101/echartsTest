$(function () {
    $("#jjsjlr").click(function () {
        if ($("#dtul").hasClass("dis")) {
            $("#dtul").removeClass("dis")
        } else {
            $("#dtul").addClass("dis")
        }
    })
    $("#zlsjlr").click(function () {
        if ($("#zlul").hasClass("dis")) {
            $("#zlul").removeClass("dis")
        } else {
            $("#zlul").addClass("dis")
        }
    })
    $(".zlsjblr").click(function () { //专利数据点击
        if (!$("#newdiv").hasClass("dis")) {
            $("#newdiv").addClass("dis");
        }
        if ($("#olddiv").hasClass("dis")) {
            $("#olddiv").removeClass("dis");
        }
        $.ajax({ type: "post", url: "../SubSection/Index", data: { "dtid": $(this).attr("deptid") }, dataType: "html", success: function (data) {
            $("#subSection").html(data);
        }
        })
        var y = 1;
        if ($(this).attr("deptid") == 2) {
            y = 7
        }
        if ($(this).attr("deptid") == 3) {
            y = 10
        }
        $.ajax({ type: "post", url: "../ZlProTab/GetZlXaxis", data: { "zldtid": y }, async: false, dataType: "json", success: function (data) {
            createPic(data);
        }
        })
        $("#editName").html(titleData);
        createTable();
    })
    $("#zdysjlr").click(function () {
        if ($("#newdiv").hasClass("dis")) {
            $("#newdiv").removeClass("dis");
        }
        if (!$("#olddiv").hasClass("dis")) {
            $("#olddiv").addClass("dis");
         }
        $.ajax({
            type: "post",
            url: "../CustomData/Index",
            dataType: "html",
            async: false,
            success: function (data) {
                $("#newdiv").html(data);
            }
        })
        $("#divContainer").animate({ scrollTop: document.body.clientHeight + 'px' }, 800);
    })
})
var SubSectionclick = function (obj) {
    $("#SubSectionUL").children().each(function () {
        $(this).removeClass("on")
    });
    $(obj).parent().addClass("on")
    $.ajax({ type: "post", url: "../ZlProTab/GetZlXaxis", data: { "zldtid": $(obj).attr("liid") }, async: false, dataType: "json", success: function (data) {
        createPic(data);
    }
    })
$("#editName").html(titleData);
createTable();
}
var sonSectionclick = function (obj) {
    
    var x = $(obj).attr("litype")
    $.ajax({ type: "post", url: "../ZlProTab/GsJsly", data: { "zldtid": $(obj).attr("liid"), "zllimit": x }, async: false, dataType: "json", success: function (data) {
        if (x == 0) {
            createPie(data)
            $("#tableNumber").html("")
            $("#operate").html("")
        } else if (x == 1) {
            createPic(data)
            createTable();
        } else if (x == 2) {
            createChord(data);
            $("#tableNumber").html("")
            $("#operate").html("")
        }
    }
})
$("#editName").html(titleData);
}
var rowclick = function (row, cloumn) {
    
    var chklength = [];
    $("input[name='" + row + "']:checked").each(function () {
        chklength.push(this)
    })
    if (chklength.length > 0) {
        $("input[name='" + cloumn + "']").attr("disabled", true)
    } else {
        $("input[name='" + cloumn + "']").attr("disabled", false)
    }
}

function createPic(obj){//obj为传过去的json串
    xaxisParam = new Array()
    yaxisParam = new Array()
    seriesData = new Array()
    legendData = new Array();
    datanum = new Array();
    dataname = new Array();
    var zlxAxisdatastr = "";
    $(obj).each(function () {
        titleData = this.title;
        zlxAxisdatastr = this.yea; //x轴数据
        var zlseriesData = new Object(); //数据对象
        var zlseriesDataList = new Array(); //数据对象
        if (this.unit != 2) {//生成线图和柱图
            if (this.unit == 1) {
                zlseriesData.type = "line";
            } else {
                zlseriesData.type = "bar";
            }
            var datasource = this.num.split("_");
            for (var zldata = 0; zldata < datasource.length; zldata++) {
                if (datasource[zldata] != "") {
                    zlseriesDataList.push(datasource[zldata])
                } else {
                    zlseriesDataList.push("0");
                }
            }
            zlseriesData.name = this.name;
            legendData.push(this.name)
            datanum.push(this.num)
            dataname.push(this.name)
            zlseriesData.data = zlseriesDataList;
            zlseriesData.yAxisIndex = this.unit
            seriesData.push(zlseriesData);
        } else {//生成专利类型比的饼图
            zlseriesData.type = "pie";
            zlseriesData.name = "10年各类型占比";
            var ds = this.num.split("_");
            var dsname = this.name.split("_");
            for (var zldata = 0; zldata < ds.length; zldata++) {
                var sdataobj = new Object();
                sdataobj.value = ds[zldata];
                sdataobj.name = dsname[zldata]
                zlseriesDataList.push(sdataobj)
            }
            var itemStylebj = new Object();
            var labelobj = new Object();
            var labellineobj = new Object();
            var normalobj = new Object();
            var tooltipobj = new Object();
            tooltipobj.trigger = "item";
            tooltipobj.formatter = "{a} <br/>{b} : {c} ({d}%)";
            labelobj.show = false;
            labellineobj.show = false;
            normalobj.label = labelobj;
            normalobj.labelLine = labellineobj;
            itemStylebj.normal = normalobj;
            zlseriesData.data = zlseriesDataList;
            zlseriesData.center = [160, 140];
            zlseriesData.radius = [0, 60];
            zlseriesData.tooltip = tooltipobj;
            zlseriesData.itemStyle = itemStylebj;
            seriesData.push(zlseriesData);
        }
    })
    var zlxAxis = new Object();
    zlxAxis.type = "category";
    var zlxAxisdata = new Array();
    var zlxAxisdatalen = zlxAxisdatastr.split("_");
    for (var zlx = 0; zlx < zlxAxisdatalen.length; zlx++) {
        zlxAxisdata.push(zlxAxisdatalen[zlx]);
    }
    xaxisParamdatalist = zlxAxisdata;
    zlxAxis.data = zlxAxisdata;
    xaxisParam.push(zlxAxis);
    var zlcreateyAxis = new Object();
    zlcreateyAxis.type = "value";
    zlcreateyAxis.name = "(个)";
    yaxisParam.push(zlcreateyAxis);
    zlcreateyAxis = new Object();
    zlcreateyAxis.type = "value";
    zlcreateyAxis.name = "(%)";
    yaxisParam.push(zlcreateyAxis);
    requerpic();
    require([
                'echarts',
                'echarts/chart/bar',
                'echarts/chart/line',
                'echarts/chart/pie'
            ],
            function (ec) {
                var myChart_Category = ec.init(document.getElementById('div_pieCategory'));
                myChart_Category.setOption(mainoption);
            })
}
function createPie(obj) {
    xaxisParam = new Array()
    yaxisParam = new Array()
    seriesData = new Array()
    legendData = new Array();
    $(obj).each(function () {
        titleData = this.title;
        var numstr = this.num.split("_");
        var classstr = this.classify.split("_");
        var pieLsit = new Array();
        var pieObj = new Object;
        for (var i = 0; i < classstr.length; i++) {
            var piedataobj = new Object();
            piedataobj.name = classstr[i];
            piedataobj.value = numstr[i];
            pieLsit.push(piedataobj);
            legendData.push(classstr[i])
        }
        pieObj.data = pieLsit;
        pieObj.name = '规上技术领域';
        pieObj.type = "pie";
        pieObj.radius = "50%";
        pieObj.center = ['50%', '50%']
        seriesData.push(pieObj)
    })
    mainoption = {
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        toolbox: {
            show: true,
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: false },
                magicType: {
                    show: true,
                    type: ['pie', 'funnel'],
                    option: {
                        funnel: {
                            x: '25%',
                            width: '50%',
                            funnelAlign: 'left',
                            max: 1548
                        }
                    }
                },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        legend: {
            orient: 'vertical',
            x: 'left',
            data: legendData
        },
        series: seriesData
    }
    require([
                'echarts',
                'echarts/chart/pie',
            'echarts/chart/funnel'
            ],
            function (ec) {
                var myChart_Category = ec.init(document.getElementById('div_pieCategory'));
                myChart_Category.setOption(mainoption);
            })
}
function createChord(obj) {
    seriesData = new Array()
    legendData = new Array();
    var sobj = new Object();
    var snodes = new Array();
    var targets = []; //links的target
    var nodes = [];
    var olinksList = new Array();//links数组
    $(obj).each(function () {
        titleData = this.title;
        targets = this.classify.split("_");
        nodes.push(this.name)
        for (var i = 0; i < targets.length; i++) {
            nodes.push(targets[i]);
            var olinks = new Object();
            olinks.source = this.name;
            olinks.target = targets[i];
            olinks.weight = 1;
            olinks.name = "属于";
            olinksList.push(olinks);
        }
        legendData.push(this.name)
    })
    nodes = nodes.deleteEle();
    for (var j = 0; j < nodes.length; j++) {
        var onodes = new Object();
        onodes.name = nodes[j];
        snodes.push(onodes); //nodes数组
    }
    sobj.type = "chord";
    sobj.radius = "65%";
    sobj.ribbonType = false;
    sobj.sort = 'ascending';
    sobj.sortSub = 'descending';
    var labelobj = new Object();
    labelobj.rotate = true;
    var normalobj = new Object();
    normalobj.label = labelobj;
    var itemobj = new Object();
    itemobj.normal = normalobj;
    sobj.itemStyle = itemobj;
//    sobj.minRadius = 7;
//    sobj.ribbonType = 'false',
//    sobj.maxRadius = 20;
    sobj.nodes = snodes;
    sobj.links = olinksList;
    seriesData.push(sobj);
    mainoption = {
        tooltip: {
            trigger: 'item',
            formatter: function (params) {
                if (params.indicator2) {    // is edge
                    return params.indicator2 + ' ' + params.name + ' ' + params.indicator;
                } else {    // is node
                    return params.name
                }
            }
        },
        toolbox: {
            show: true,
            feature: {
                restore: { show: true },
                magicType: { show: true, type: ['force', 'chord'] },
                saveAsImage: { show: true }
            }
        },
        legend: {
            data: legendData
        },
        series: seriesData
    }
    require([
                'echarts',
                 'echarts/chart/chord',
                'echarts/chart/force'
           
            ],
            function (ec) {
                var myChart_Category = ec.init(document.getElementById('div_pieCategory'));
                myChart_Category.setOption(mainoption);
            })
}
function requerpic() {
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
    
}
Array.prototype.deleteEle = function () {
    var arr = this, o = {}, newArr = [], i, n;
    for (i = 0; i < arr.length; i++) {
        n = arr[i] + typeof (arr[i]); //如果不需要类型判断，直接将后面的去掉即可
        if (typeof (o[n]) === "undefined") {
            newArr[newArr.length] = arr[i]
            o[n] = 1; //缓存
        }
    }
    return newArr;
}