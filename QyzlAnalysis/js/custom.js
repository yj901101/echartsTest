$("#creat-echart").click(function () {
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
        cloumnchk.push(chkstr)
        cloumnchkval.push(chkval)
    })
    if (cloumnchk.length > 0) { //勾选了一行的数据。
        xaxisParam = new Array()
        yaxisParam = new Array()
        seriesData = new Array()
        legendData = new Array();
        var createxAxis = new Object();
        createxAxis.type = "category";
        createxAxis.data = customyear;
        xaxisParam.push(createxAxis)
        var createyAxis = new Object();
        createyAxis.type = "value";
        createyAxis.name = chkunit[0];
        yaxisParam.push(createyAxis);
        createyAxis = new Object();
        createyAxis.type = "value";
        createyAxis.name = "（个）";
        yaxisParam.push(createyAxis);
        for (var i = 0; i < cloumnchk.length; i++) {
            var seriesobj = new Object();
            var objlist = new Array();
            for (var j = 0; j < cloumnchk[i].split("_").length; j++) {
                if (cloumnchk[i].split("_")[j] != "") {
                    objlist.push(cloumnchk[i].split("_")[j]);
                }
            }
            if (cloumnchkval[i].indexOf("jl") >= 0) {
                seriesobj.type = $("#" + cloumnchkval[i].changeval()).val();
                seriesobj.yAxisIndex = 0;
            } else {
                seriesobj.type = seriesobj.type = $("#" + cloumnchkval[i].changeval()).val();
                seriesobj.yAxisIndex = 1;
            }
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
    } else {
        alert("未选择数据！");
        return false;
    }

})
    var customyear = new Array();
    var chkjjzl = [];
    var chkunit = [];
    $(".btn").click(function () {
        customyear = new Array();
        var idtype = "";
        if ($(this).attr("cltype") == "jj") {
            idtype = $("#son").val();
        } else {
            idtype = $("#zlfather").val() + "_" + $("#zlson").val() + "_" + $("#sonshow").val() + "_" + $("#sel_view").val();
        }
        $.ajax({
            type: "post",
            url: "../CreateTab/Rjson",
            data: { "id": idtype, "valtype": $(this).attr("cltype") },
            dataType: "json",
            success: function (data) {
                var strconn = ""; var stype = ""; var sname = ""; var sunit = "";
                $(data).each(function () {
                    customyear.push(this.year);
                    sname = this.name; stype = this.typ; sunit = this.unit;
                    strconn += "<td><input type='text' name='dtnum' cloumnid='" + stype + "' value=" + this.num + " /></td>";
                })
                var str = "";
                var strselect = "<select id='" + stype.changeval() + "' style='width:100%'><option value='bar'>柱状图</option><option value='line'>线图</option></select>";
                if (sunit != "0") {
                    if (chkunit.length > 0) {
                        if (chkunit.in_array(sunit)) {
                            if (!chkjjzl.in_array(stype)) {
                                str = "<tr><td><label><input type='checkbox' name='cloumn' value='" + stype + "'>" + sname + "</label><a href='#' onclick=removeRow(this,'"+stype+"')><img src='../img/icon-delete.png' onMouseOver=this.src='../img/icon-delete-on.png' onMouseOut=this.src='../img/icon-delete.png'></a></td>" + strconn + "<td>" + strselect + "</td></tr>";
                                chkjjzl.push(stype);
                            } else {
                                alert("数据已被选择")
                            }
                        } else {
                            alert("单位过多")
                        }
                    } else {
                        chkunit.push(sunit)
                        if (!chkjjzl.in_array(stype)) {
                            str = "<tr><td><label><input type='checkbox' name='cloumn' value='" + stype + "'>" + sname + "</label><a href='#' onclick=removeRow(this,'" + stype + "')><img src='../img/icon-delete.png' onMouseOver=this.src='../img/icon-delete-on.png' onMouseOut=this.src='../img/icon-delete.png'></a></td>" + strconn + "<td>" + strselect + "</td></tr>";
                            chkjjzl.push(stype);
                        } else {
                            alert("数据已被选择")
                        }
                    }
                } else {
                    if (!chkjjzl.in_array(stype)) {
                        str = "<tr><td><label><input type='checkbox' name='cloumn' value='" + stype + "'>" + sname + "</label><a href='#' onclick=removeRow(this,'" + stype + "')><img src='../img/icon-delete.png' onMouseOver=this.src='../img/icon-delete-on.png' onMouseOut=this.src='../img/icon-delete.png'></a></td>" + strconn + "<td>" + strselect + "</td></tr>";
                        chkjjzl.push(stype);
                    } else {
                        alert("数据已被选择")
                    }
                }

                $("#thtr").append(str);
            }
        })
    })
    function removeRow(r,aval) {
        var tr = r.parentNode.parentNode;
        var tbody = tr.parentNode;
        tbody.deleteRow(tr);
        chkjjzl.remove(aval);
        if (chkjjzl.length == 0) {
            chkunit = [];
        } else {
            if (!chkjjzl.in_array("jl")) {
                chkunit = [];
            }
        }
    }
    String.prototype.changeval = function () {
        if (this.indexOf("/") != -1) {
            return this.replace("/", "")
        }
        return this;
    }
    Array.prototype.in_array = function (e) {
        for (i = 0; i < this.length; i++) {
            if (this[i] == e)
                return true;
        }
        return false;
    }
    Array.prototype.indexOf = function (val) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] == val) return i;
        }
        return -1;
    }; //给js的数组对象定义一个函数，用于查找指定的元素在数组中的位置，即索引
    Array.prototype.remove = function (val) {
        var index = this.indexOf(val);
        if (index > -1) {
            this.splice(index, 1);
        }
    };