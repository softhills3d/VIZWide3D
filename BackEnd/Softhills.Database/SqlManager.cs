using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Softhills.Database
{
    public class SqlManager : Softhills.Data.BaseModel
    {
        // ====================================================================
        // Attribute
        // ====================================================================
        public Softhills.Data.SQLDatabaseConnection Database { get; set; }


        // ====================================================================
        // Construction
        // ====================================================================
        public SqlManager(Softhills.Data.SQLDatabaseConnection database) : base()
        {
            Database = database;
        }

        // ====================================================================
        // Method
        // ====================================================================

        /// <summary>
        /// 데이터 조회 예제
        /// </summary>
        /// <param name="testData1"></param>
        /// <returns></returns>
        public List<string> Get(string testData1)
        {
            List<string> items = new List<string>();

            string sql = @"SELECT Data1 from testTable
                           where testData1 = :testData";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("testData", testData1)
            };

            try
            {
                DataSet ds = Softhills.Data.SqlHelper.ExecuteDataset(Database.ToString(), sql, CommandType.Text, parameters);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return items;

                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    string data = item["Data1"].ToString();
                    items.Add(data);
                }
            }
            catch(Exception ex)
            {
                
            }

            return items;
        }
    }
}
