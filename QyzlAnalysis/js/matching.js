function addmatch() {
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
    var d = new Date();
    timequeue = d.getFullYear() + "" + (d.getMonth() + 1) + "" + d.getDate() + "" + d.getHours() + "" + d.getMinutes() + "" + d.getSeconds() + "" + d.getMilliseconds()
    $.post("../Matching/insert", { "nums": cloumnchk.join(","), "timequene": timequeue, "names": cloumnchkval.join(",") }, function (jsobj) {
        alert(jsobj.msg)
    })
}