using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace QyzlAnalysis.DbHelper
{
    public class PubHelper
    {
        public static string ConnectionString
        {
            get
            {
                string _connectionString =  ConfigurationManager.AppSettings["dburl"];
                //string _connectionString = ConfigurationManager.AppSettings["sendEmail"];
                //string ConStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
                //if (ConStringEncrypt == "true")
                //{
                //_connectionString = DESEncrypt.Decrypt(_connectionString);
                //}
                return _connectionString;
            }
        }
    }
}