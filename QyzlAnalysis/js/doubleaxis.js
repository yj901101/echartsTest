//按钮控制
function labelclick() {
    if ($("input[name='cloumn']:checked").length == 2) {
        $("#doubleaxis").html('<a href="javascript:createdoubleaxis()"><img src="../../img/btn-create-axis.png"/></a>')
    } else {
        $("#doubleaxis").html('<img src="../../img/btn-create-axis-gray.png" />')
    }
}
function createdoubleaxis() {
    var chkunit1 = [];
    var cloumnchk = new Array();
    var rowchk = new Array();
    var cloumnchkval = new Array();
    var rowchkval = new Array();
    var chkval = ""; //用来存储列头
    var rowhead = new Array();
    $("input[name='cloumn']:checked").each(function () {
        chkval = $(this).val();
        var chkstr = "";
        var chkstrhead = "";
        cloumnhead = []
        $("input[name='dtnum']").each(function () {
            if ($(this).attr("cloumnid") == chkval) {
                if ($(this).val() != "") {
                    chkstr += $(this).val() + '_';
                } else {
                    chkstr += 0 + "_";
                }
            }
        })
        cloumnchk.push(chkstr)//数据
        cloumnchkval.push(chkval)//名称
    })
    for (var i = 0; i < cloumnchkval.length; i++) {
        if (cloumnchkval[i].indexOf("(") != -1) {
            chkunit1.push(cloumnchkval[i].substring(cloumnchkval[i].indexOf("("), cloumnchkval[i].indexOf(")")+1))
        } else {
            chkunit1.push(cloumnchkval[i].substring(cloumnchkval[i].indexOf("（"), cloumnchkval[i].indexOf("）")+1))
        }
    }
    xaxisParam = new Array()
    yaxisParam = new Array()
    seriesData = new Array()
    legendData = new Array();
    var createxAxis = new Object();
    createxAxis.type = "category";
    createxAxis.data = customyear;
    xaxisParam.push(createxAxis)//x轴数据
    for (var i = 0; i < chkunit1.length; i++) {
        var createyAxis = new Object();
        createyAxis.type = "value";
        if (chkunit1[i] != "") {
            createyAxis.name = chkunit1[i];
        } else {
            createyAxis.name = "（个）";
        }
        yaxisParam.push(createyAxis); //y轴数据
    }
    for (var i = 0; i < cloumnchk.length; i++) {
        var seriesobj = new Object();
        var objlist = new Array();
        for (var j = 0; j < cloumnchk[i].split("_").length; j++) {
            if (cloumnchk[i].split("_")[j] != "") {
                objlist.push(cloumnchk[i].split("_")[j]);
            }
        }
        seriesobj.type = $("#" + cloumnchkval[i].changeval().changevalPer().changevalAdd().changevalAdd()).val();
        seriesobj.yAxisIndex = i;
        seriesobj.data = objlist;
        seriesobj.name = cloumnchkval[i].substring(2, cloumnchkval[i].length);
        legendData.push(cloumnchkval[i].substring(2, cloumnchkval[i].length))
        seriesData.push(seriesobj);
    }
    mainoption = {
        title: {
            text: $("#writeName").val()
        },
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
    if ($("#formCustomShow").hasClass("Customdis")) {
        $("#formCustomShow").removeClass("Customdis")
    }
    require([
                'echarts',
                'echarts/chart/bar',
                'echarts/chart/line'
            ],
            function (ec) {
                var myChart_Category = ec.init(document.getElementById('formCustomShow'));
                myChart_Category.setOption(mainoption);
            })
    $("#divContainer").animate({ scrollTop: '0px' }, 800);
}