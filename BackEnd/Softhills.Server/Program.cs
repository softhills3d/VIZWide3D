using Softhills.Database;
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
            /// 기본. 선체 단위블록(D0)
            /// </summary>
            [Obsolete("사용되지 않음.", true)]
            BASE = 0,

            /// <summary>
            /// 병합된 블록. 조립블록(S0), 중조블록(G1), 단위블록(D0)
            /// </summary>
            MERGE = 1,

            /// <summary>
            /// 경량화(간소화) 블록
            /// </summary>
            SIMPLIFY = 2,

            /// <summary>
            /// 블록 이미지. 정반 Matrix 적용되어 있음
            /// </summary>
            [Obsolete("사용되지 않음.", true)]
            SNAPSHOT = 3
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
            server = new Softhills.Net.HttpServerCore(80, "SOFTHILLS", true);
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
                case "SHIP":
                    // ?CMD=SHIP&FILEKIND=1
                    ProcessSHIP(e.Context, e.Parameters);
                    break;
                case "BLOCK":
                    // ?CMD=BLOCK&FILEKIND=1&EXT=viz&SHIP=3322
                    ProcessBLOCK(e.Context, e.Parameters);
                    break;
                case "STREAM":
                    // ?CMD=STREAM&FILEKIND=1&ALLOWEMPTY=1&SHIP=3322&BLOCK=S21P0&FORMAT=viz
                    ProcessStream(e.Context, e.Parameters);
                    break;
                case "UPLOAD":
                    // ?CMD=UPLOAD&FILEKIND=1&ACCESSKEY=FD6D949E-62B7-46D7-8D0C-62B6D3C24F8F&SHIP=3322&BLOCK=S21P0&UD=BP18722&BUFFER=binary
                    ProcessUpload(e.Context, e.Parameters);
                    break;
                case "DELETE":
                    // ?CMD=DELETE&FILEKIND=1&ACCESSKEY=FD6D949E-62B7-46D7-8D0C-62B6D3C24F8F&SHIP=3322&BLOCK=S21P0&UD=BP18722
                    ProcessDelete(e.Context, e.Parameters);
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
        internal static string GetPath(FileKind filekind)
        {
            string path = String.Empty;
            switch (filekind)
            {
                //case FileKind.BASE:
                //    path = Softhills.Framework.Configuration.AppSettingsHelper.GetSetting("REPOSITORY_BASE");
                //    break;
                case FileKind.MERGE:
                    path = Softhills.Configuration.AppSettingsHelper.GetSetting("REPOSITORY_MERGE");
                    break;
                case FileKind.SIMPLIFY:
                    path = Softhills.Configuration.AppSettingsHelper.GetSetting("REPOSITORY_SIMPLIFY");
                    break;
                //case FileKind.SNAPSHOT:
                //    path = Softhills.Framework.Configuration.AppSettingsHelper.GetSetting("REPOSITORY_SNATSHOT");
                //    break;
                default:
                    break;
            }

            return path;
        }

        /// <summary>
        /// Repository 경로에 이름 가져오기
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        private static void ProcessSHIP(System.Net.HttpListenerContext context, Dictionary<string, string> parameters)
        {
            // MERGE or SIMPLIFY
            string filekind = "2";

            if(GetParameters(parameters, "FILEKIND", out filekind) == false)
            {
                server.ResponseString(context, "");
                return;
            }

            string path = GetPath((FileKind)Convert.ToInt32(filekind));

            string[] dir = System.IO.Directory.GetDirectories(path);

            List<string> result = new List<string>();
            foreach (string item in dir)
            {
                string ship = new System.IO.DirectoryInfo(item).Name;
                result.Add(ship);
            }

            string output = string.Join(",", result.ToArray());

            server.ResponseString(context, output);
        }

        /// <summary>
        /// [ 경로가 SHIP - BLOCK인 경우] Repository 경로에 블록 정보 가져오기
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        private static void ProcessBLOCK(System.Net.HttpListenerContext context, Dictionary<string, string> parameters)
        {
            // MERGE or SIMPLIFY
            string filekind = "2";

            if (GetParameters(parameters, "FILEKIND", out filekind) == false)
            {
                server.ResponseString(context, "");
                return;
            }

            string extCode = "0";
            string ext = ".viz";

            if (GetParameters(parameters, "EXT", out extCode) == false)
            {
                ext = ".viz";
            }
            else
            {
                if (extCode == "1") ext = ".viz";
                else if (extCode == "2") ext = ".vizm";
                else if (extCode == "3") ext = ".vizw";
                else ext = ".viz";
            }

            string SHIP_NO = String.Empty;

            if (GetParameters(parameters, "SHIP", out SHIP_NO) == false)
            {
                server.ResponseString(context, "");
                return;
            }

            string base_path = GetPath((FileKind)Convert.ToInt32(filekind));
            string path = System.IO.Path.Combine(base_path, SHIP_NO);

            if(System.IO.Directory.Exists(path) == false)
            {
                server.ResponseString(context, "");
                return;
            }

            string[] files = System.IO.Directory.GetFiles(path);

            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (string item in files)
            {
                string SHIP_BLOCK = System.IO.Path.GetFileNameWithoutExtension(item);
                string EXTENSION = System.IO.Path.GetExtension(item);
                if (ext.ToUpper() != EXTENSION.ToUpper()) continue;

                string[] name = SHIP_BLOCK.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                if (name.Length == 2)
                {
                    string ship = name[0];
                    string block = name[1];

                    if (result.ContainsKey(block) == false)
                        result.Add(block, block);
                }
                else
                {
                    string ship = name[0];
                    //string block = name[1];

                    if (result.ContainsKey(ship) == false)
                        result.Add(ship, ship);
                }
            }

            string output = string.Join(",", result.Keys.ToArray());

            server.ResponseString(context, output);
        }

        /// <summary>
        /// Repository 특정 경로의 파일 다운로드
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        private static void ProcessStream(System.Net.HttpListenerContext context, Dictionary<string, string> parameters)
        {
            // MERGE or SIMPLIFY
            string _filekind = "2";
            FileKind fileKind = FileKind.SIMPLIFY;

            if (GetParameters(parameters, "FILEKIND", out _filekind) == false)
            {
                server.ResponseString(context, "");
                return;
            }
            else
            {
                fileKind = (FileKind)Convert.ToInt32(_filekind);
            }

            // Allow Empty Model - 값이 없을 경우, 허용
            string allowEmptyModel = "1";
            if (GetParameters(parameters, "ALLOWEMPTY", out allowEmptyModel) == false)
            {
                allowEmptyModel = "1";
            }

            string SHIP_NO = String.Empty;

            if (GetParameters(parameters, "SHIP", out SHIP_NO) == false)
            {
                server.ResponseString(context, "");
                return;
            }

            string BLOCK_NO = String.Empty;

            if (GetParameters(parameters, "BLOCK", out BLOCK_NO) == false)
            {
                server.ResponseString(context, "");
                return;
            }

            string EXTENSION = "viz";

            if(GetParameters(parameters, "FORMAT", out EXTENSION) == false)
            {
                EXTENSION = "viz";
            }

            string base_path = GetPath(fileKind);
            string path = String.Empty;

            // 호선 전체 간소화 모델일 경우...
            if(BLOCK_NO.Contains("_SIMPLIFY") == true)
            {
                path = string.Format("{0}\\{1}\\{1}_SIMPLIFY_UD.{2}", base_path, SHIP_NO, EXTENSION);
            }
            // 호선/블록
            else
            {
                // 일반 블록
                if (SHIP_NO != BLOCK_NO) 
                {
                    path = string.Format("{0}\\{1}\\{1}-{2}_UD.{3}", base_path, SHIP_NO, BLOCK_NO, EXTENSION);
                }
                // 호선의 S0 단위 호선 모델
                else
                {
                    path = string.Format("{0}\\{1}\\{1}_UD.{2}", base_path, SHIP_NO, EXTENSION);
                }
            }


            if (System.IO.File.Exists(path) == false)
            {
                // 호선 전체 간소화 모델일 경우...
                if (BLOCK_NO.Contains("_SIMPLIFY") == true)
                {
                    path = string.Format("{0}\\{1}\\{1}_SIMPLIFY.{2}", base_path, SHIP_NO, EXTENSION);
                }
                // 호선/블록
                else
                {
                    // 일반 블록
                    if (SHIP_NO != BLOCK_NO)
                    {
                        path = string.Format("{0}\\{1}\\{1}-{2}.{3}", base_path, SHIP_NO, BLOCK_NO, EXTENSION);
                    }
                    // 호선의 S0 단위 호선 모델
                    else
                    {
                        path = string.Format("{0}\\{1}\\{1}.{2}", base_path, SHIP_NO, EXTENSION);
                    }
                }
            }

            // 빈 모델 : 파일이 없고, 빈모델이 허용일 경우
            if (System.IO.File.Exists(path) == false && allowEmptyModel == "1")
            {
                path = string.Format("{0}\\EMPTY.viz", base_path);
            }

            server.ResponseFile(context, path);
        }

        /// <summary>
        /// Repository에 파일 업로드
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        private static void ProcessUpload(System.Net.HttpListenerContext context, Dictionary<string, string> parameters)
        {
            string _filekind = "2"; // SIMPLIFY
            FileKind fileKind = FileKind.SIMPLIFY;

            if (GetParameters(parameters, "FILEKIND", out _filekind) == false)
            {
                server.ResponseString(context, "0");
                return;
            }
            else
            {
                fileKind = (FileKind)Convert.ToInt32(_filekind);
            }

            string ACCESS_KEY = String.Empty;

            if (GetParameters(parameters, "ACCESSKEY", out ACCESS_KEY) == false)
            {
                server.ResponseString(context, "0");
                return;
            }

            if (ACCESS_KEY != "FD6D949E-62B7-46D7-8D0C-62B6D3C24F8F")
            {
                server.ResponseString(context, "0");
                return;
            }

            string SHIP_NO = String.Empty;

            if (GetParameters(parameters, "SHIP", out SHIP_NO) == false)
            {
                server.ResponseString(context, "0");
                return;
            }

            string BLOCK_NO = String.Empty;

            if (GetParameters(parameters, "BLOCK", out BLOCK_NO) == false)
            {
                server.ResponseString(context, "0");
                return;
            }

            string USER_DEFINE = String.Empty;
            if (GetParameters(parameters, "UD", out USER_DEFINE) == false)
            {
                USER_DEFINE = "";
            }
            else
            {
                USER_DEFINE = "UD";
            }

            string binary_string = String.Empty;

            if (GetParameters(parameters, "BUFFER", out binary_string) == false)
            {
                server.ResponseString(context, "0");
                return;
            }

            byte[] buffer = Convert.FromBase64String(binary_string);

            string output = String.Empty;

            if (String.IsNullOrEmpty(USER_DEFINE) == true)
            {
                if (SHIP_NO != BLOCK_NO)
                    output = string.Format("{0}\\{1}\\{1}-{2}.viz", GetPath(fileKind), SHIP_NO, BLOCK_NO);
                else
                    output = string.Format("{0}\\{1}\\{1}.viz", GetPath(fileKind), SHIP_NO);
            }
            else
            {
                if (SHIP_NO != BLOCK_NO)
                    output = string.Format("{0}\\{1}\\{1}-{2}_UD.viz", GetPath(fileKind), SHIP_NO, BLOCK_NO);
                else
                    output = string.Format("{0}\\{1}\\{1}_UD.viz", GetPath(fileKind), SHIP_NO);
            }

            System.IO.File.WriteAllBytes(output, buffer);

            server.ResponseString(context, "1");
            return;
        }

        /// <summary>
        /// Repository에 특정 파일 삭제
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        private static void ProcessDelete(System.Net.HttpListenerContext context, Dictionary<string, string> parameters)
        {
            string _filekind = "2"; // SIMPLIFY
            FileKind fileKind = FileKind.SIMPLIFY;

            if (GetParameters(parameters, "FILEKIND", out _filekind) == false)
            {
                server.ResponseString(context, "0");
                return;
            }
            else
            {
                fileKind = (FileKind)Convert.ToInt32(_filekind);
            }

            string ACCESS_KEY = String.Empty;

            if (GetParameters(parameters, "ACCESSKEY", out ACCESS_KEY) == false)
            {
                server.ResponseString(context, "0");
                return;
            }

            if (ACCESS_KEY != "FD6D949E-62B7-46D7-8D0C-62B6D3C24F8F")
            {
                server.ResponseString(context, "0");
                return;
            }

            string SHIP_NO = String.Empty;

            if (GetParameters(parameters, "SHIP", out SHIP_NO) == false)
            {
                server.ResponseString(context, "0");
                return;
            }

            string BLOCK_NO = String.Empty;

            if (GetParameters(parameters, "BLOCK", out BLOCK_NO) == false)
            {
                server.ResponseString(context, "0");
                return;
            }

            string USER_DEFINE = String.Empty;
            if (GetParameters(parameters, "UD", out USER_DEFINE) == false)
            {
                USER_DEFINE = "";
            }
            else
            {
                USER_DEFINE = "UD";
            }

            string output = String.Empty;

            if (String.IsNullOrEmpty(USER_DEFINE) == true)
            {
                if (SHIP_NO != BLOCK_NO)
                    output = string.Format("{0}\\{1}\\{1}-{2}.viz", GetPath(fileKind), SHIP_NO, BLOCK_NO);
                else
                    output = string.Format("{0}\\{1}\\{1}.viz", GetPath(fileKind), SHIP_NO);
            }
            else
            {
                if (SHIP_NO != BLOCK_NO)
                    output = string.Format("{0}\\{1}\\{1}-{2}_UD.viz", GetPath(fileKind), SHIP_NO, BLOCK_NO);
                else
                    output = string.Format("{0}\\{1}\\{1}_UD.viz", GetPath(fileKind), SHIP_NO);
            }

            if (System.IO.File.Exists(output) == true)
            {
                System.IO.File.Delete(output);

                server.ResponseString(context, "1");
            }
            else
            {
                server.ResponseString(context, "0");
            }

            return;
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
