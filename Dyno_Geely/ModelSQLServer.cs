using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Dyno_Geely {
    public class ModelSQLServer {
        public string StrConn { get; set; }
        public readonly Logger _log;
        public SQLSetting _sqlSetting;

        public ModelSQLServer(SQLSetting settings, Logger log) {
            _log = log;
            _sqlSetting = settings;
            this.StrConn = "";
            ReadConfig();
        }

        void ReadConfig() {
            StrConn = "user id=" + _sqlSetting.UserName + ";";
            StrConn += "password=" + _sqlSetting.PassWord + ";";
            StrConn += "database=" + _sqlSetting.DBName + ";";
            StrConn += "data source=" + _sqlSetting.IP + "," + _sqlSetting.Port;
        }

        public void ShowDB(string strTable) {
            string strSQL = "select * from " + strTable;

            using (SqlConnection sqlConn = new SqlConnection(StrConn)) {
                sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand(strSQL, sqlConn);
                SqlDataReader sqlData = sqlCmd.ExecuteReader();
                string str = "";
                int c = sqlData.FieldCount;
                while (sqlData.Read()) {
                    for (int i = 0; i < c; i++) {
                        object obj = sqlData.GetValue(i);
                        if (obj.GetType() == typeof(DateTime)) {
                            str += ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss") + "\t";
                        } else {
                            str += obj.ToString() + "\t";
                        }
                    }
                    str += "\n";
                }
                _log.TraceInfo(str);
                sqlCmd.Dispose();
                sqlConn.Close();
            }
        }

        public void TestConnect() {
            using (SqlConnection sqlConn = new SqlConnection(StrConn)) {
                sqlConn.Open();
                sqlConn.Close();
            }
        }

        public string[] GetTableColumns(string strTable) {
            using (SqlConnection sqlConn = new SqlConnection(StrConn)) {
                try {
                    sqlConn.Open();
                    DataTable schema = sqlConn.GetSchema("Columns", new string[] { null, null, strTable });
                    schema.DefaultView.Sort = "ORDINAL_POSITION";
                    schema = schema.DefaultView.ToTable();
                    int count = schema.Rows.Count;
                    string[] columns = new string[count];
                    for (int i = 0; i < count; i++) {
                        DataRow row = schema.Rows[i];
                        foreach (DataColumn col in schema.Columns) {
                            if (col.Caption == "COLUMN_NAME") {
                                if (col.DataType.Equals(typeof(DateTime))) {
                                    columns[i] = string.Format("{0:d}", row[col]);
                                } else if (col.DataType.Equals(typeof(decimal))) {
                                    columns[i] = string.Format("{0:C}", row[col]);
                                } else {
                                    columns[i] = string.Format("{0}", row[col]);
                                }
                            }
                        }
                    }
                    return columns;
                } catch (Exception ex) {
                    _log.TraceError("==> SQL ERROR: " + ex.Message);
                } finally {
                    sqlConn.Close();
                }
            }
            return new string[] { };
        }

        public Dictionary<string, int> GetTableColumnsDic(string strTable) {
            Dictionary<string, int> colDic = new Dictionary<string, int>();
            string[] cols = GetTableColumns(strTable);
            for (int i = 0; i < cols.Length; i++) {
                colDic.Add(cols[i], i);
            }
            return colDic;
        }

        public void InsertDB(string strTable, DataTable dt) {
            string columns = " (";
            for (int i = 0; i < dt.Columns.Count; i++) {
                columns += dt.Columns[i].ColumnName + ",";
            }
            columns = columns.Substring(0, columns.Length - 1) + ")";

            for (int i = 0; i < dt.Rows.Count; i++) {
                string row = " values ('";
                for (int j = 0; j < dt.Columns.Count; j++) {
                    row += dt.Rows[i][j].ToString() + "','";
                }
                row = row.Substring(0, row.Length - 2) + ")";
                string strSQL = "insert into " + strTable + columns + row;

                using (SqlConnection sqlConn = new SqlConnection(StrConn)) {
                    SqlCommand sqlCmd = new SqlCommand(strSQL, sqlConn);
                    try {
                        sqlConn.Open();
                        _log.TraceInfo(string.Format("==> T-SQL: {0}", strSQL));
                        _log.TraceInfo(string.Format("==> Insert {0} record(s)", sqlCmd.ExecuteNonQuery()));
                    } catch (Exception ex) {
                        _log.TraceError("==> SQL ERROR: " + ex.Message);
                    } finally {
                        sqlCmd.Dispose();
                        sqlConn.Close();
                    }
                }
            }
        }

        public void UpdateDB(string strTable, DataTable dt, Dictionary<string, string> whereDic) {
            for (int i = 0; i < dt.Rows.Count; i++) {
                string strSQL = "update " + strTable + " set ";
                for (int j = 0; j < dt.Columns.Count; j++) {
                    strSQL += dt.Columns[j].ColumnName + " = '" + dt.Rows[i][j].ToString() + "', ";
                }
                strSQL = strSQL.Substring(0, strSQL.Length - 2);
                strSQL += " where ";
                foreach (string key in whereDic.Keys) {
                    strSQL += key + " = '" + whereDic[key] + "' and ";
                }
                strSQL = strSQL.Substring(0, strSQL.Length - 5);

                using (SqlConnection sqlConn = new SqlConnection(StrConn)) {
                    SqlCommand sqlCmd = new SqlCommand(strSQL, sqlConn);
                    try {
                        sqlConn.Open();
                        _log.TraceInfo(string.Format("==> T-SQL: {0}", strSQL));
                        _log.TraceInfo(string.Format("==> Update {0} record(s)", sqlCmd.ExecuteNonQuery()));
                    } catch (Exception ex) {
                        _log.TraceError("==> SQL ERROR: " + ex.Message);
                    } finally {
                        sqlCmd.Dispose();
                        sqlConn.Close();
                    }
                }

            }
        }

        int RunSQL(string strSQL) {
            int count = 0;
            if (strSQL.Length == 0) {
                return -1;
            }
            try {
                using (SqlConnection sqlConn = new SqlConnection(StrConn)) {
                    SqlCommand sqlCmd = new SqlCommand(strSQL, sqlConn);
                    try {
                        sqlConn.Open();
                        count = sqlCmd.ExecuteNonQuery();
                        _log.TraceInfo(string.Format("==> T-SQL: {0}", strSQL));
                        _log.TraceInfo(string.Format("==> {0} record(s) affected", count));
                    } catch (Exception ex) {
                        _log.TraceError("==> SQL ERROR: " + ex.Message);
                    } finally {
                        sqlCmd.Dispose();
                        sqlConn.Close();
                    }
                }
            } catch (Exception ex) {
                _log.TraceError("==> SQL ERROR: " + ex.Message);
            }
            return count;
        }

        string[,] SelectDB(string strSQL) {
            string[,] records = null;
            try {
                int count = 0;
                List<string[]> rowList;
                using (SqlConnection sqlConn = new SqlConnection(StrConn)) {
                    SqlCommand sqlCmd = new SqlCommand(strSQL, sqlConn);
                    sqlConn.Open();
                    SqlDataReader sqlData = sqlCmd.ExecuteReader();
                    count = sqlData.FieldCount;
                    rowList = new List<string[]>();
                    while (sqlData.Read()) {
                        string[] items = new string[count];
                        for (int i = 0; i < count; i++) {
                            object obj = sqlData.GetValue(i);
                            if (obj.GetType() == typeof(DateTime)) {
                                items[i] = ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss");
                            } else {
                                items[i] = obj.ToString();
                            }
                        }
                        rowList.Add(items);
                    }
                    sqlCmd.Dispose();
                    sqlConn.Close();
                }
                records = new string[rowList.Count, count];
                for (int i = 0; i < rowList.Count; i++) {
                    for (int j = 0; j < count; j++) {
                        records[i, j] = rowList[i][j];
                    }
                }
                return records;
            } catch (Exception ex) {
                _log.TraceError("==> SQL ERROR: " + ex.Message);
            }
            return records;
        }

        public int GetRecordCount(string strTable, Dictionary<string, string> whereDic) {
            string strSQL = "select * from " + strTable + " where ";
            foreach (string key in whereDic.Keys) {
                strSQL += key + " = '" + whereDic[key] + "' and ";
            }
            strSQL = strSQL.Substring(0, strSQL.Length - 5);
            _log.TraceInfo("==> T-SQL: " + strSQL);
            string[,] strArr = SelectDB(strSQL);
            if (strArr != null) {
                return strArr.GetLength(0);
            } else {
                return -1;
            }
        }

        public string[,] GetRecords(string strTable, Dictionary<string, string> whereDic) {
            string strSQL = "select * from " + strTable + " where ";
            foreach (string key in whereDic.Keys) {
                strSQL += key + " = '" + whereDic[key] + "' and ";
            }
            strSQL = strSQL.Substring(0, strSQL.Length - 5);
            _log.TraceInfo("==> T-SQL: " + strSQL);
            return SelectDB(strSQL);
        }

        public bool ModifyDB(string strTable, DataTable dt) {
            for (int i = 0; i < dt.Rows.Count; i++) {
                Dictionary<string, string> whereDic = new Dictionary<string, string> {
                    { "VIN", dt.Rows[i][0].ToString() },
                    { "ECU_ID", dt.Rows[i][1].ToString() }
                };
                string strSQL = "";
                int count = GetRecordCount(strTable, whereDic);
                if (count > 0) {
                    strSQL = "update " + strTable + " set ";
                    for (int j = 0; j < dt.Columns.Count; j++) {
                        strSQL += dt.Columns[j].ColumnName + " = '" + dt.Rows[i][j].ToString() + "', ";
                    }
                    strSQL += "WriteTime = '" + DateTime.Now.ToLocalTime().ToString() + "' where ";
                    foreach (string key in whereDic.Keys) {
                        strSQL += key + " = '" + whereDic[key] + "' and ";
                    }
                    strSQL = strSQL.Substring(0, strSQL.Length - 5);
                } else if (count == 0) {
                    strSQL = "insert " + strTable + " (";
                    for (int j = 0; j < dt.Columns.Count; j++) {
                        strSQL += dt.Columns[j].ColumnName + ", ";
                    }
                    strSQL = strSQL.Substring(0, strSQL.Length - 2) + ") values ('";

                    for (int j = 0; j < dt.Columns.Count; j++) {
                        strSQL += dt.Rows[i][j].ToString() + "', '";
                    }
                    strSQL = strSQL.Substring(0, strSQL.Length - 3) + ")";
                } else if (count < 0) {
                    return false;
                }
                RunSQL(strSQL);
            }
            return true;
        }

        private string GetFieldValue(string field, string JCLSH, bool bNumber = false, int precision = 0, int scale = 0) {
            string[,] vals = null;
            string strRet = "";
            if (field.Length > 0 && field != "--") {
                string strSQL = "select [" + field.Split('.')[1] + "] from [" + field.Split('.')[0] + "] where [JCLSH] = '" + JCLSH + "'";
                //m_log.TraceInfo("==> T-SQL: " + strSQL);
                vals = SelectDB(strSQL);
            }
            if (vals != null && vals.GetLength(0) > 0) {
                if (bNumber) {
                    strRet = Normalize(vals[0, 0], precision, scale);
                    if (strRet != vals[0, 0]) {
                        _log.TraceError("original value\"" + vals[0, 0] + "\" larger than specified precision\"" + precision.ToString() + "\" and scale\"" + scale.ToString() + "\"");
                    }
                } else {
                    strRet = vals[0, 0];
                }
            }
            return strRet;
        }

        private string Normalize(string strNum, int precision, int scale) {
            if (strNum == null || strNum.Length == 0) {
                return strNum;
            }
            if (strNum.Split('.')[0].Length > precision - scale) {
                return new string('9', precision - scale) + "." + new string('9', scale);
            } else {
                return strNum;
            }
        }

        private string GetTestType(string strValue) {
            string strRet = "9";
            if (strValue.Contains("双怠速")) {
                strRet = "1";
            } else if (strValue.Contains("稳态工况")) {
                strRet = "2";
            } else if (strValue.Contains("简易瞬态")) {
                strRet = "3";
            } else if (strValue.Contains("加载减速")) {
                strRet = "4";
            } else if (strValue.Contains("自由加速")) {
                strRet = "6";
            } else if (strValue.Contains("林格曼")) {
                strRet = "7";
            } else if (strValue.Contains("瞬态工况")) {
                strRet = "8";
            }
            return strRet;
        }

        /// <summary>
        /// 获取环境数据，返回值为[RH, ET, AP]数组
        /// </summary>
        /// <param name="JCLSH">检测流水号</param>
        /// <returns></returns>
        private string[] GetEnvValue(string JCLSH) {
            string[,] vals;
            string[] rets = { "", "", "" };
            EnvStructure[] envs = new EnvStructure[3];
            envs[0] = new EnvStructure("SD", 4, 2);
            envs[1] = new EnvStructure("WD", 5, 2);
            envs[2] = new EnvStructure("DQY", 5, 2);
            string[] tables = { "双怠速法结果库", "稳态工况结果库", "简易瞬态结果库", "加载减速结果库" };

            for (int j = 0; j < tables.Length; j++) {
                string strSQL = "select [" + envs[0].Column + "], [" + envs[1].Column + "], [" + envs[2].Column + "] from [" + tables[j] + "] where [JCLSH] = '" + JCLSH + "'";
                //m_log.TraceInfo("==> T-SQL: " + strSQL);
                vals = SelectDB(strSQL);
                if (vals != null && vals.GetLength(0) > 0) {
                    for (int i = 0; i < envs.Length; i++) {
                        rets[i] = Normalize(vals[0, i], envs[i].Precision, envs[i].Scale);
                        if (rets[i] != vals[0, i]) {
                            _log.TraceError("original value\"" + vals[0, 0] + "\" larger than specified precision\"" + envs[0].Precision.ToString() + "\" and scale\"" + envs[0].Scale.ToString() + "\"");
                        }
                    }
                    break;
                }
            }
            return rets;
        }

        public int SetUpload(int value, string JCLSH) {
            string strSQL = "update [已检车辆库] set [Upload] = '" + value.ToString() + "' where [JCLSH] = '" + JCLSH + "'";
            return RunSQL(strSQL);
        }

        public Dictionary<string, string> GetJCLSH(string strVIN) {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string[,] result = SelectDB(string.Format("select [JCLSH] from [已检车辆库] where [VIN] = '{0}'", strVIN));
            ret.Add("VIN号", strVIN);
            if (result.GetLength(0) > 0) {
                ret.Add("检测流水号", result[0, 0]);
            }
            return ret;
        }

        public Dictionary<string, string> GetEnv(string JCLSH) {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string[] env = GetEnvValue(JCLSH);
            ret.Add("相对湿度(%)", env[0]);
            ret.Add("环境温度(°C)", env[1]);
            ret.Add("大气压力(kPa)", env[2]);
            return ret;
        }

        public void AddSkipField() {
            string strSQL = "select top (1) [Skip] from [已检车辆库]";
            string[,] rets = SelectDB(strSQL);
            if (rets == null || rets.GetLength(0) < 1) {
                strSQL = "alter table [已检车辆库] add [Skip] int not null default(0)";
                RunSQL(strSQL);
            }
        }

        public int SetSkip(int value, string JCLSH) {
            string strSQL = "update [已检车辆库] set [Skip] = '" + value.ToString() + "' where [JCLSH] = '" + JCLSH + "'";
            return RunSQL(strSQL);
        }

    }

    public class EnvStructure {
        public string Column { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }

        public EnvStructure(string column, int precision, int scale) {
            this.Column = column;
            this.Precision = precision;
            this.Scale = scale;
        }
    }
}
