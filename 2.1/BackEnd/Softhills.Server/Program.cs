using Softhills.Database;
using Softhills.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Softhills.Server
{
    class Program
    {
        /// <summary>
        /// 파일 유형
        /// </summary>
        public enum FileKind
        {
            /// <summary>
            /// 기본
            /// </summary>
            BASE = 0,
        }

        /// <summary>
        /// File Extensions
        /// </summary>
        public enum FileExtensions
        {
            /// <summary>
            /// All File Format
            /// </summary>
            ALL = 0,

            /// <summary>
            /// VIZ
            /// </summary>
            VIZ = 1,

            /// <summary>
            /// VIZM (Mobile)
            /// </summary>
            VIZM = 2,

            /// <summary>
            /// VIZW (Web)
            /// </summary>
            VIZW = 3
        }

        /// <summary>
        /// 오라클 데이터베이스 연결자
        /// </summary>
        public static Softhills.Data.OracleDatabaseConnection OracleDatabase { get; set; }

        /// <summary>
        /// MSSQL 데이터베이스 연결자
        /// </summary>
        public static Softhills.Data.SQLDatabaseConnection SqlDatabase { get; set; }

        /// <summary>
        /// HTTP SERVER CORE
        /// </summary>
        internal static Softhills.Net.HttpServerCore server;

        private static OracleManager _OracleManager;

        private static SqlManager _SqlManager;

        static void Main(string[] args)
        {
            // ================================================
            // Init. Configuration
            // ================================================
            InitConfiguration();

            // Database Loding
            _OracleManager = new OracleManager(OracleDatabase);
            _SqlManager = new SqlManager(SqlDatabase);

            // ================================================
            // SERVER
            // ================================================
            server = new Softhills.Net.HttpServerCore(8080, "SOFTHILLS", true);
            server.OnClientRequestEvent += Server_OnClientRequestEvent;
            server.OnServerMessageEvent += Server_OnServerMessageEvent;

            server.Start();

            while (true)
            {
                string str = System.Console.ReadLine();
                if(String.IsNullOrEmpty(str) == false)
                {
                    if(str.ToUpper() == "EXIT")
                    {
                        break;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(5000);
                    }
                }
            }
        }

        /// <summary>
        /// 환경정보 초기화
        /// </summary>
        public static void InitConfiguration()
        {
            // IP / Port / Service Name / User ID / Password
            OracleDatabase = new Softhills.Data.OracleDatabaseConnection(
                Softhills.Configuration.AppSettingsHelper.GetSetting("DATABASE_HOST_TEST")
                , Softhills.Configuration.AppSettingsHelper.GetSetting("DATABASE_PORT_TEST")
                , Softhills.Configuration.AppSettingsHelper.GetSetting("DATABASE_SERVICE_NAME_TEST")
                , Softhills.Configuration.AppSettingsHelper.GetSetting("DATABASE_USERID_TEST")
                , Softhills.Configuration.AppSettingsHelper.GetSetting("DATABASE_PASSWORD_TEST")
                );

            // IP,Port / Service Name / User ID / Password
            SqlDatabase = new Softhills.Data.SQLDatabaseConnection(
                $"{Softhills.Configuration.AppSettingsHelper.GetSetting("DATABASE_HOST_TEST1")},{Softhills.Configuration.AppSettingsHelper.GetSetting("DATABASE_PORT_TEST1")}"
                , Softhills.Configuration.AppSettingsHelper.GetSetting("DATABASE_SERVICE_NAME_TEST1")
                , Softhills.Configuration.AppSettingsHelper.GetSetting("DATABASE_USERID_TEST1")
                , Softhills.Configuration.AppSettingsHelper.GetSetting("DATABASE_PASSWORD_TEST1")
                );

        }

        private static void Server_OnServerMessageEvent(object sender, Softhills.Net.ServerMessageEventArgs e)
        {
            if (String.IsNullOrEmpty(e.Message) == true) return;

            Softhills.Console.DisplayMessageHelper.Print(e.Message
                , Softhills.Console.DisplayMessageHelper.MessageKind.INFORMATION
                , true
                , true
                );
        }

        private static void Server_OnClientRequestEvent(object sender, Softhills.Net.ClientRequestEventArgs e)
        {
            string cmd = String.Empty;

            if (e.Parameters.ContainsKey("CMD") == true)
                cmd = e.Parameters["CMD"];

            // IP:PORT/SOFTHILLS
            switch (cmd)
            {
                case "VIZW":
                    // ?CMD=VIZW&SHIP=3380&BLOCK=A21P0&NAME=/A21P0_wh.vizw
                    ProcessVIZW(e.Context, e.Parameters);
                    break;
                case "GetOracleData":
                    // ?CMD=GetOracleData
                    ProcessGetOracleData(e.Context, e.Parameters);
                    break;
                case "GetSqlData":
                    // ?CMD=GetSqlData
                    ProcessGetSqlData(e.Context, e.Parameters);
                    break;
                default:
                    break;
            }
        }

        internal static bool GetParameters(Dictionary<string, string> parameters, string key, out string val)
        {
            if(parameters.ContainsKey(key) == true)
            {
                val = parameters[key];
                return true;
            }
            else
            {
                val = "";
                return false;
            }
        }

        /// <summary>
        /// 저장소 경로 반환
        /// </summary>
        /// <param name="filekind">저장소 유형</param>
        /// <returns>저장소 경로</returns>
        internal static string GetPath()
        {
            return Softhills.Configuration.AppSettingsHelper.GetSetting("REPOSITORY_BASE");
        }

        /// <summary>
        /// Repository 경로에 이름 가져오기
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        private static void ProcessVIZW(System.Net.HttpListenerContext context, Dictionary<string, string> parameters)
        {
            string SHIP_NO = string.Empty;

            if (GetParameters(parameters, "SHIP", out SHIP_NO) == false)
            {
                server.ResponseString(context, "");
                return;
            }

            string BLOCK_NO = string.Empty;

            if (GetParameters(parameters, "BLOCK", out BLOCK_NO) == false)
            {
                server.ResponseString(context, "");
                return;
            }

            string NAME = string.Empty;

            if (GetParameters(parameters, "NAME", out NAME) == false)
            {
                server.ResponseString(context, "");
                return;
            }

            string base_path = GetPath();
            string path = string.Format("{0}\\{1}\\{2}\\{3}", base_path, SHIP_NO, BLOCK_NO, NAME.Replace("/", ""));

            if (!System.IO.File.Exists(path))
            {
                server.ResponseString(context, "");
                return;
                
            }

            server.ResponseFile(context, path);
        }


        /// <summary>
        /// [ 예제 코드 ] 오라클 데이터베이스에서 데이터 가져오기
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        private static void ProcessGetOracleData(System.Net.HttpListenerContext context, Dictionary<string, string> parameters)
        {
            // 예제 코드임으로 동작은 하지 않습니다.
            List<string> test = _OracleManager.Get("test");
            List<string> result = new List<string>();
            foreach (string item in test)
            {
                result.Add(item);
            }

            string output = string.Join(",", result.ToArray());

            server.ResponseString(context, output);
        }

        /// <summary>
        /// [ 예제 코드 ] MSSQL 데이터베이스에서 데이터 가져오기
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        private static void ProcessGetSqlData(System.Net.HttpListenerContext context, Dictionary<string, string> parameters)
        {
            // 예제 코드임으로 동작은 하지 않습니다.
            List<string> test = _SqlManager.Get("test");
            List<string> result = new List<string>();
            foreach (string item in test)
            {
                result.Add(item);
            }

            string output = string.Join(",", result.ToArray());

            server.ResponseString(context, output);
        }
    }
}
