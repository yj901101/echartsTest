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
        if (diyVal != "diy") {
            var createyAxis = new Object();
            createyAxis.type = "value";
            createyAxis.name = chkunit[0];
            yaxisParam.push(createyAxis);
            createyAxis = new Object();
            createyAxis.type = "value";
            createyAxis.name = "（个）";
            yaxisParam.push(createyAxis);
        } else {
            var createyAxis = new Object();
            for (var j = 0; j < chkdiy.length; j++) {
                createyAxis = new Object();
                createyAxis.type = "value";
                createyAxis.name = chkdiy[j];
                yaxisParam.push(createyAxis);
            }
        }
        for (var i = 0; i < cloumnchk.length; i++) {
            var seriesobj = new Object();
            var objlist = new Array();
            for (var j = 0; j < cloumnchk[i].split("_").length; j++) {
                if (cloumnchk[i].split("_")[j] != "") {
                    objlist.push(cloumnchk[i].split("_")[j]);
                }
            }
            if ((cloumnchkval[i].indexOf("jl") >= 0 || cloumnchkval[i].indexOf("kj") >= 0 || cloumnchkval[i].indexOf("rc") >= 0) && diyVal != "diy") {
                seriesobj.type = $("#" + cloumnchkval[i].changeval()).val();
                seriesobj.yAxisIndex = 0;
            } else if (diyVal == "diy") {
                if (chkunit[i] == chkdiy[0]) {//自定义专利图让第一个单位相同
                    seriesobj.type = $("#" + cloumnchkval[i].changeval().changevalPer().changevalAdd().changevalAdd()).val();
                    seriesobj.yAxisIndex = 0;
                } else {
                    seriesobj.type = $("#" + cloumnchkval[i].changeval().changevalPer().changevalAdd().changevalAdd()).val();
                    seriesobj.yAxisIndex = 1;
                }
            } else {
                seriesobj.type = $("#" + cloumnchkval[i].changeval()).val();
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
    var diyVal = ""; //判断当前的选择是不是diy
    var chkdiy = []; //判断单位的顺序
    $(".btn").click(function () {
        var clunit = "";
        customyear = new Array();
        var idtype = "";
        if ($(this).attr("cltype") == "jj") {
            idtype = $("#son").val();
        } else if ($(this).attr("cltype") == "zl") {
            idtype = $("#zlfather").val() + "_" + $("#zlson").val() + "_" + $("#sonshow").val() + "_" + $("#sel_view").val();
        } else if ($(this).attr("cltype") == "kj") {
            idtype = $("#tec1").val();
        } else if ($(this).attr("cltype") == "diy") {//自定义图
            if ($(this).attr("clunit") == "lose") {
                idtype = $("#son").val();
            } else if ($(this).attr("clunit") == "lose1") {
                idtype = $("#son1").val();
            } else if ($(this).attr("clunit") == "lose2") {
                idtype = $("#son2").val();
            }
            diyVal = "diy";
            clunit = $(this).attr("clunit")
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
                var strselect = "<select id='" + stype.changeval().changevalPer().changevalAdd().changevalAdd() + "' style='width:100%'><option value='bar'>柱状图</option><option value='line'>线图</option></select>";
                if (clunit.indexOf("lose") != -1) {//判断是否为专利生成图
                    if (!chkdiy.in_array(sunit)) {//获取单位的顺序
                        chkdiy.push(sunit)
                    }
                    chkunit.push(sunit)
                    if (!chkjjzl.in_array(stype)) {
                        str = "<tr><td><label onclick='labelclick()'><input type='checkbox' name='cloumn' value='" + stype + "'>" + sname + "</label><a href='#' onclick=removeRow(this,'" + stype + "')><img src='../img/icon-delete.png' onMouseOver=this.src='../img/icon-delete-on.png' onMouseOut=this.src='../img/icon-delete.png'></a></td>" + strconn + "<td>" + strselect + "</td></tr>";
                        chkjjzl.push(stype);
                    } else {
                        alert("数据已被选择")
                    }
                } else {
                    if (sunit != "0") {
                        if (chkunit.length > 0) {
                            if (chkunit.in_array(sunit) || chkunit.isNull()) {
                                if (!chkjjzl.in_array(stype)) {
                                    chkunit.push(sunit)//添加数据添加单位
                                    str = "<tr><td><label onclick='labelclick()'><input type='checkbox' name='cloumn' value='" + stype + "'>" + sname + "</label><a href='#' onclick=removeRow(this,'" + stype + "','jj')><img src='../img/icon-delete.png' onMouseOver=this.src='../img/icon-delete-on.png' onMouseOut=this.src='../img/icon-delete.png'></a></td>" + strconn + "<td>" + strselect + "</td></tr>";
                                    chkjjzl.push(stype);
                                } else {
                                    alert("数据已被选择")
                                }
                            } else {
                                alert("单位过多")
                            }
                        } else {
                            //chkunit.push(sunit)
                            if (!chkjjzl.in_array(stype)) {
                                chkunit.push(sunit)//添加数据添加单位
                                str = "<tr><td><label onclick='labelclick()'><input type='checkbox' name='cloumn' value='" + stype + "'>" + sname + "</label><a href='#' onclick=removeRow(this,'" + stype + "','jj')><img src='../img/icon-delete.png' onMouseOver=this.src='../img/icon-delete-on.png' onMouseOut=this.src='../img/icon-delete.png'></a></td>" + strconn + "<td>" + strselect + "</td></tr>";
                                chkjjzl.push(stype);
                            } else {
                                alert("数据已被选择")
                            }
                        }
                    } else {
                        if (!chkjjzl.in_array(stype)) {
                            chkunit.push("")//记录专利单位为空，作为索引
                            str = "<tr><td><label onclick='labelclick()'><input type='checkbox' name='cloumn' value='" + stype + "'>" + sname + "</label><a href='#' onclick=removeRow(this,'" + stype + "','zl')><img src='../img/icon-delete.png' onMouseOver=this.src='../img/icon-delete-on.png' onMouseOut=this.src='../img/icon-delete.png'></a></td>" + strconn + "<td>" + strselect + "</td></tr>";
                            chkjjzl.push(stype);
                        } else {
                            alert("数据已被选择")
                        }
                    }
                } //判断是否为专利生成图
                $("#thtr").append(str);
            }
        })
    })
    Array.prototype.isNull = function () {
        var b = true;
        var newNullArray = this;
        for (var i = 0; i < newNullArray.lenght; i++) {
            if (newNullArray[i] != "") {
                b = false;
            }
        }
        return b;
    }
    function removeRow(r,aval,tys) {
//        var tbody = document.getElementById('thtr')
//        tbody.deleteRow(r.parentNode.parentNode.rowIndex);
        // alert(r.parentNode.parentNode.rowIndex)
        var index = r.parentNode.parentNode.rowIndex
        $(r).parent().parent().remove();
        chkjjzl.remove(aval);
        if (tys == "jj") {
            chkunit.splice(index - 1, 1)
        }
        if (chkjjzl.length == 0) {
            chkunit = [];
        }
        if ($("input[name='cloumn']:checked").length == 2) {
            $("#doubleaxis").html('<a href="javascript:createdoubleaxis()"><img src="../../img/btn-create-axis.png"/></a>')
        } else {
            $("#doubleaxis").html('<img src="../../img/btn-create-axis-gray.png" />')
        }
//        else {
//            if (!chkjjzl.in_array("jl")) {
//                chkunit = [];
//            }
//        }
    }
    String.prototype.changeval = function () {
        if (this.indexOf("/") != -1) {
            return this.replace("/", "")
        }
        return this;
    }
    String.prototype.changevalPer = function () {//去除%
        if (this.indexOf("(%)") != -1) {
            return this.replace("(%)", "")
        }
        return this;
    }
    String.prototype.changevalAdd = function () {//去除+
        if (this.indexOf("+") != -1) {
            return this.replace("+", "")
        }
        return this;
    }
    Array.prototype.in_array = function (e) {//判断元素是否存在于数组中
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
//    Array.prototype.deleteEle = function () {//去除数组中相同的元素
//        var arr = this, o = {}, newArr = [], i, n;
//        for (i = 0; i < arr.length; i++) {
//            n = arr[i] + typeof (arr[i]); //如果不需要类型判断，直接将后面的去掉即可
//            if (typeof (o[n]) === "undefined") {
//                newArr[newArr.length] = arr[i]
//                o[n] = 1; //缓存
//            }
//        }
//        return newArr;
//    }
    Array.prototype.deleteEle = function () {
        var o = {}, newArr = [], i, j;
        for (i = 0; i < this.length; i++) {
            if (typeof (o[this[i]]) == "undefined") {
                o[this[i]] = "";
            }
        }
        for (j in o) {
            newArr.push(j)
        }
        return newArr;
    }
