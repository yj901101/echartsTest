using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Score_Version_By_Hxy
{
    public partial class ScoreTest : System.Web.UI.Page
    {
        public double scorepoint = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //有效专利
            DataTable dt1 = DbHelperSQL.GetDataTable("select *from Qyzl.dbo.zl_count WHERE flzt='有效专利' AND name='铁基新材料专利'");
            DataTable dt2 = DbHelperSQL.GetDataTable("select *from Qyzl.dbo.zl_count WHERE flzt='有效专利' AND name='专用汽车产业专利'");
            DataTable dt3 = DbHelperSQL.GetDataTable("select *from Qyzl.dbo.zl_count WHERE flzt='有效专利' AND name='电子信息产业专利'");
            int valid1 = dt1.Rows.Count;
            int valid2 = dt2.Rows.Count;
            int valid3 = dt3.Rows.Count;
            //实质审查
            DataTable dt4 = DbHelperSQL.GetDataTable("select *from Qyzl.dbo.zl_count WHERE flzt='实质审查' AND name='铁基新材料专利'");
            DataTable dt5 = DbHelperSQL.GetDataTable("select *from Qyzl.dbo.zl_count WHERE flzt='实质审查' AND name='专用汽车产业专利'");
            DataTable dt6 = DbHelperSQL.GetDataTable("select *from Qyzl.dbo.zl_count WHERE flzt='实质审查' AND name='电子信息产业专利'");
            int valid4 = dt4.Rows.Count;
            int valid5 = dt5.Rows.Count;
            int valid6 = dt6.Rows.Count;
            //公开发明
            DataTable dt7 = DbHelperSQL.GetDataTable("select *from Qyzl.dbo.zl_count WHERE flzt='公开发明' AND name='铁基新材料专利'");
            DataTable dt8 = DbHelperSQL.GetDataTable("select *from Qyzl.dbo.zl_count WHERE flzt='公开发明' AND name='专用汽车产业专利'");
            DataTable dt9 = DbHelperSQL.GetDataTable("select *from Qyzl.dbo.zl_count WHERE flzt='公开发明' AND name='电子信息产业专利'");
            int valid7 = dt7.Rows.Count;
            int valid8 = dt8.Rows.Count;
            int valid9 = dt9.Rows.Count;
            //失效专利
            DataTable dt10 = DbHelperSQL.GetDataTable("select *from Qyzl.dbo.zl_count WHERE flzt='失效专利' AND name='铁基新材料专利'");
            DataTable dt11 = DbHelperSQL.GetDataTable("select *from Qyzl.dbo.zl_count WHERE flzt='失效专利' AND name='专用汽车产业专利'");
            DataTable dt12 = DbHelperSQL.GetDataTable("select *from Qyzl.dbo.zl_count WHERE flzt='失效专利' AND name='电子信息产业专利'");
            int valid10 = dt10.Rows.Count;
            int valid11 = dt11.Rows.Count;
            int valid12= dt12.Rows.Count;
            //valid1-7得分是大于6分的
            scorepoint = ((valid1 + valid2 + valid3 + valid4 + valid5 + valid6 + valid7) * 1.0) / (valid1 + valid2 + valid3 + valid4 + valid5 + valid6 + valid7 + valid8 + valid9 + valid10 + valid11 + valid12)*100;
        }
        }
    }
