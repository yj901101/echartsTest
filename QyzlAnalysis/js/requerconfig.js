require.config({packages:[{
                name: "echarts",
                location: "../../js/echarts-2.1.9/src",
                main: "echarts" },{
                name: 'zrender',
                location: '../../js/zrender-2.0.6/src', // zrender与echarts在同一级目录
                main: 'zrender'}]});