using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Softhills.Database
{
    public class OracleManager : Softhills.Data.BaseModel
    {
        // ====================================================================
        // Attribute
        // ====================================================================
        public Softhills.Data.OracleDatabaseConnection Database { get; set; }


        // ====================================================================
        // Construction
        // ====================================================================
        public OracleManager(Softhills.Data.OracleDatabaseConnection database) : base()
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

            List<OracleParameter> parameters = new List<OracleParameter>
            {
                new OracleParameter("testData", OracleDbType.Varchar2, testData1, ParameterDirection.Input)
            };

            try
            {
                DataSet ds = Softhills.Data.OracleHelper.ExecuteDataset(Database.ToString(), sql, CommandType.Text, parameters);

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
