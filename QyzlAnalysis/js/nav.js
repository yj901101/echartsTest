var Menu = ["subSectionUL"];
var Nav = function () {
var li = document.getElementById(Menu).getElementsByTagName("li");
for(var i=0;i<li.length;i++){
var li_ul = li[i].getElementsByTagName("ul");
if(li_ul.length){
li[i].onmouseout = function(){
this.getElementsByTagName("a")[0].className = ""; 
this.getElementsByTagName("a")[0].style.backgroundColor = "";
this.getElementsByTagName("ul")[0].style.display ="none";
}
li[i].onmouseover= function(){
if(this.parentNode.id==Menu){
this.getElementsByTagName("a")[0].className = "ok"; 
}else{
this.getElementsByTagName("a")[0].style.backgroundColor = "#F00"; 
}	
this.getElementsByTagName("ul")[0].style.display ="block";
}
}
}
};