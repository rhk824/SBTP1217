using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Data
{
    public class ExcelHelper
    {
        
        /// <summary>
        /// 读取 Excel 数据
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static DataTable ReadExcelToTable(string path)
        {
            try
            {
                string connstring = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes;IMEX=1';", path);
                using (OleDbConnection conn = new OleDbConnection(connstring))
                {
                    conn.Open();
                    // 读取分表名称（sheetname）
                    //DataTable sheetsname = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Tabel" });
                    DataTable sheetsname = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string first_sheetname = sheetsname.Rows[0]["TABLE_NAME"].ToString();
                    string sql = string.Format("select * from [{0}]", first_sheetname);
                    OleDbDataAdapter ada = new OleDbDataAdapter(sql, connstring);
                    DataSet set = new DataSet();
                    ada.Fill(set);
                    return set.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        

    }
}
