using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dyno_Geely {
    public class ModelMySQL {
        private string _strConn;
        private readonly Logger _log;
        private readonly SQLSetting _sqlSetting;

        public ModelMySQL(SQLSetting settings, Logger log) {
            _log = log;
            _sqlSetting = settings;
            _strConn = string.Empty;
            ReadConfig();
        }

        void ReadConfig() {
            _strConn = "server=" + _sqlSetting.IP + ";" + "port=" + _sqlSetting.Port + ";";
            _strConn += "uid=" + _sqlSetting.UserName + ";";
            _strConn += "pwd=" + _sqlSetting.PassWord + ";";
            _strConn += "database=" + _sqlSetting.DBName + ";";
            _strConn += "charset=utf8mb4;";
        }

        public void TestConnect() {
            using (MySqlConnection mySqlConn = new MySqlConnection(_strConn)) {
                mySqlConn.Open();
                mySqlConn.Close();
            }
        }

        /// <summary>
        /// 执行update insert delete语句，失败了返回-1，成功了返回影响的行数
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        private int ExecuteNonQuery(string strSQL, string Connection) {
            using (MySqlConnection connection = new MySqlConnection(Connection)) {
                int val = -1;
                try {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(strSQL, connection);
                    val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                } catch (MySqlException ex) {
                    _log.TraceError("Error SQL: " + strSQL);
                    _log.TraceError(ex.Message);
                    throw new Exception(ex.Message);
                } finally {
                    if (connection.State != ConnectionState.Closed) {
                        connection.Close();
                    }
                }
                return val;
            }
        }

        private void Query(string strSQL, DataTable dt, string Connection) {
            using (MySqlConnection connection = new MySqlConnection(Connection)) {
                try {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(strSQL, connection);
                    adapter.Fill(dt);
                } catch (MySqlException ex) {
                    _log.TraceError("Error SQL: " + strSQL);
                    _log.TraceError(ex.Message);
                    throw new Exception(ex.Message);
                } finally {
                    if (connection.State != ConnectionState.Closed) {
                        connection.Close();
                    }
                }
            }
        }

        private object QueryOne(string strSQL, string Connection) {
            using (MySqlConnection connection = new MySqlConnection(Connection)) {
                using (MySqlCommand cmd = new MySqlCommand(strSQL, connection)) {
                    try {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value))) {
                            return null;
                        } else {
                            return obj;
                        }
                    } catch (MySqlException ex) {
                        _log.TraceError("Error SQL: " + strSQL);
                        _log.TraceError(ex.Message);
                        throw new Exception(ex.Message);
                    } finally {
                        if (connection.State != ConnectionState.Closed) {
                            connection.Close();
                        }
                    }
                }
            }
        }

        public int InsertRecords(string strTable, DataTable dt) {
            int iRet = 0;
            for (int iRow = 0; iRow < dt.Rows.Count; iRow++) {
                string strSQL = "insert into " + strTable + " (";
                for (int iCol = 0; iCol < dt.Columns.Count; iCol++) {
                    if (dt.Rows[iRow][iCol].ToString().Length != 0) {
                        strSQL += dt.Columns[iCol].ColumnName + ",";
                    }
                }
                strSQL = strSQL.Trim(',');
                strSQL += ") values (";
                for (int iCol = 0; iCol < dt.Columns.Count; iCol++) {
                    if (dt.Rows[iRow][iCol].ToString().Length != 0) {
                        if (dt.Columns[iCol].DataType == typeof(DateTime)) {
                            strSQL += "str_to_date('" + ((DateTime)dt.Rows[iRow][iCol]).ToString("yyyyMMdd-HHmmss") + "', '%Y%m%d-%H%i%S'),";
                        } else {
                            strSQL += "'" + dt.Rows[iRow][iCol].ToString() + "',";
                        }
                    }
                }
                strSQL = strSQL.Trim(',');
                strSQL += ")";
                iRet += ExecuteNonQuery(strSQL, _strConn);
            }
            return iRet;
        }

        public int UpdateRecords(string strTable, DataTable dt, string strWhereKey, string[] strWhereValues) {
            int iRet = 0;
            if (dt.Rows.Count != strWhereValues.Length) {
                return -1;
            }
            for (int iRow = 0; iRow < dt.Rows.Count; iRow++) {
                string strSQL = "update " + strTable + " set ";
                for (int iCol = 0; iCol < dt.Columns.Count; iCol++) {
                    if (dt.Rows[iRow][iCol].ToString().Length != 0 && dt.Rows[iRow][iCol].ToString() != dt.Columns[iCol].ColumnName) {
                        if (dt.Columns[iCol].DataType == typeof(DateTime)) {
                            strSQL += dt.Columns[iCol].ColumnName + "=" + "str_to_date('" + ((DateTime)dt.Rows[iRow][iCol]).ToString("yyyyMMdd-HHmmss") + "', '%Y%m%d-%H%i%S'),";
                        } else {
                            strSQL += dt.Columns[iCol].ColumnName + "='" + dt.Rows[iRow][iCol].ToString() + "',";
                        }
                    }
                }
                strSQL = strSQL.Trim(',');
                strSQL += " where " + strWhereKey + "='" + strWhereValues[iRow] + "'";
                iRet += ExecuteNonQuery(strSQL, _strConn);
            }
            return iRet;
        }

        public string[] GetValue(string strTable, string strField, string strWhereKey, string strWhereValue, string strDataTimeFormat = "yyyy-MM-dd HH:mm:ss") {
            string strSQL = "select " + strField + " from " + strTable + " where " + strWhereKey + " = '" + strWhereValue + "'";
            DataTable dt = new DataTable();
            Query(strSQL, dt, _strConn);
            string[] values = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++) {
                if (dt.Columns[0].DataType == typeof(DateTime)) {
                    values[i] = ((DateTime)dt.Rows[i][0]).ToString(strDataTimeFormat);
                } else {
                    values[i] = dt.Rows[i][0].ToString();
                }
            }
            Array.Sort(values);
            dt.Dispose();
            return values;
        }

        public void UpdatePreheating() {
            DataTable dt = new DataTable("Preheating");
            dt.Columns.Add("LastTime", typeof(DateTime));
            DataRow dr = dt.NewRow();
            dr["LastTime"] = DateTime.Now;
            dt.Rows.Add(dr);
            UpdateRecords("Preheating", dt, "ID", new string[] { "1" });
        }

        private void GenerateDataTableFromClass<T>(DataTable dt, T obj) {
            PropertyInfo[] PropertyList = obj.GetType().GetProperties();
            foreach (PropertyInfo item in PropertyList) {
                dt.Columns.Add(item.Name);
            }
            DataRow dr = dt.NewRow();
            foreach (PropertyInfo item in PropertyList) {
                dr[item.Name] = item.GetValue(obj, null);
            }
            dt.Rows.Add(dr);
        }

        private void FillClassFromDataTable<T>(DataTable dt, T obj) {
            if (dt.Rows.Count > 0) {
                PropertyInfo[] PropertyList = obj.GetType().GetProperties();
                foreach (PropertyInfo item in PropertyList) {
                    object value = dt.Rows[dt.Rows.Count - 1][item.Name];
                    if (value.GetType() == typeof(decimal)) {
                        value = Convert.ToInt32(value);
                    }
                    item.SetValue(obj, value, null);
                }
            }
        }

        public void GetVehicleInfo(string strVIN, VehicleInfo vi) {
            string strSQL = "select * from VehicleInfo where VIN = '" + strVIN + "'";
            DataTable dtVI = new DataTable("VehicleInfo");
            Query(strSQL, dtVI, _strConn);
            if (dtVI.Rows.Count > 0) {
                FillClassFromDataTable(dtVI, vi);
            }
        }

        public void SetTestQTYInVehicleInfo(string strVIN) {
            string strSQL = "select * from VehicleInfo where VIN = '" + strVIN + "'";
            DataTable dtVI = new DataTable("VehicleInfo");
            Query(strSQL, dtVI, _strConn);
            if (dtVI.Rows.Count > 0) {
                DataRow dr = dtVI.Rows[dtVI.Rows.Count - 1];
                dr["TestQTY"] = Convert.ToInt32(dr["TestQTY"]) + 1;
                UpdateRecords(dtVI.TableName, dtVI, "ID", new string[] { dr["ID"].ToString() });
            }
        }

        public void GetEmissionInfo(string strVIN, EmissionInfo ei) {
            string strSQL = "select * from VehicleInfo where VIN = '" + strVIN + "'";
            DataTable dtVI = new DataTable("VehicleInfo");
            Query(strSQL, dtVI, _strConn);
            if (dtVI.Rows.Count > 0) {
                DataRow dr = dtVI.Rows[dtVI.Rows.Count - 1];
                DataTable dtEI = new DataTable("EmissionInfo");
                strSQL = "select * from EmissionInfo where VehicleModel = '" + dr["VehicleModel"] + "' and OpenInfoSN = '" + dr["OpenInfoSN"] + "'";
                Query(strSQL, dtEI, _strConn);
                FillClassFromDataTable(dtEI, ei);
            }
        }

        public void SaveEmissionInfo(string strVIN, EmissionInfo ei) {
            string strSQL = "select * from VehicleInfo where VIN = '" + strVIN + "'";
            DataTable dtVI = new DataTable("VehicleInfo");
            Query(strSQL, dtVI, _strConn);

            strSQL = "select ID from EmissionInfo where VehicleModel = '" + ei.VehicleModel + "' and OpenInfoSN = '" + ei.OpenInfoSN + "'";
            object EI_ID = QueryOne(strSQL, _strConn);

            DataTable dtEI = new DataTable("EmissionInfo");
            GenerateDataTableFromClass(dtEI, ei);
            if (dtVI.Rows.Count > 0) {
                if (EI_ID != null) {
                    UpdateRecords(dtEI.TableName, dtEI, "ID", new string[] { EI_ID.ToString() });
                } else {
                    InsertRecords(dtEI.TableName, dtEI);
                }
            } else {
                DataRow dr = dtVI.NewRow();
                dr["VIN"] = strVIN;
                dr["VehicleModel"] = ei.VehicleModel;
                dr["OpenInfoSN"] = ei.OpenInfoSN;
                dtVI.Rows.Add(dr);
                InsertRecords(dtVI.TableName, dtVI);
                InsertRecords(dtEI.TableName, dtEI);
            }
        }

        public void SaveLDResult(string strVIN, DateTime startTime, double runningTime, EnvironmentData envData, LDResultData resultData) {
            DataTable dt = new DataTable("LugdownResult");
            dt.Columns.Add("VIN");
            dt.Columns.Add("Temperature");
            dt.Columns.Add("Humidity");
            dt.Columns.Add("Pressure");
            dt.Columns.Add("StartTime");
            dt.Columns.Add("RunningTime");
            dt.Columns.Add("RatedRPM");
            dt.Columns.Add("MaxRPM");
            dt.Columns.Add("VelMaxHP");
            dt.Columns.Add("RealMaxPowerLimit");
            dt.Columns.Add("RealMaxPower");
            dt.Columns.Add("KLimit");
            dt.Columns.Add("K100");
            dt.Columns.Add("K80");
            dt.Columns.Add("NOx80Limit");
            dt.Columns.Add("NOx80");
            dt.Columns.Add("Result");
            DataRow dr = dt.NewRow();
            dr["VIN"] = strVIN;
            dr["Temperature"] = envData.Temperature;
            dr["Humidity"] = envData.Humidity;
            dr["Pressure"] = envData.Pressure;
            dr["StartTime"] = startTime;
            dr["RunningTime"] = runningTime;
            dr["RatedRPM"] = resultData.RatedRPM;
            dr["MaxRPM"] = resultData.MaxRPM;
            dr["VelMaxHP"] = resultData.VelMaxHP;
            dr["RealMaxPowerLimit"] = resultData.RealMaxPowerLimit;
            dr["RealMaxPower"] = resultData.RealMaxPower;
            dr["KLimit"] = resultData.KLimit;
            dr["K100"] = resultData.K100;
            dr["K80"] = resultData.K80;
            dr["NOx80Limit"] = resultData.NOx80Limit;
            dr["NOx80"] = resultData.NOx80;
            dr["Result"] = resultData.Result == "合格" ? 1 : 0;
            dt.Rows.Add(dr);
            InsertRecords(dt.TableName, dt);
        }

        public void SaveASMResult(string strVIN, DateTime startTime, double runningTime, EnvironmentData envData, ASMResultData resultData) {
            DataTable dt = new DataTable("ASMResult");
            dt.Columns.Add("VIN");
            dt.Columns.Add("Temperature");
            dt.Columns.Add("Humidity");
            dt.Columns.Add("Pressure");
            dt.Columns.Add("StartTime");
            dt.Columns.Add("RunningTime");
            dt.Columns.Add("HC5025Limit");
            dt.Columns.Add("CO5025Limit");
            dt.Columns.Add("NO5025Limit");
            dt.Columns.Add("HC5025");
            dt.Columns.Add("CO5025");
            dt.Columns.Add("NO5025");
            dt.Columns.Add("HC5025Evl");
            dt.Columns.Add("CO5025Evl");
            dt.Columns.Add("NO5025Evl");
            dt.Columns.Add("Power5025");
            dt.Columns.Add("HC2540Limit");
            dt.Columns.Add("CO2540Limit");
            dt.Columns.Add("NO2540Limit");
            dt.Columns.Add("HC2540");
            dt.Columns.Add("CO2540");
            dt.Columns.Add("NO2540");
            dt.Columns.Add("HC2540Evl");
            dt.Columns.Add("CO2540Evl");
            dt.Columns.Add("NO2540Evl");
            dt.Columns.Add("Power2540");
            dt.Columns.Add("Result");
            DataRow dr = dt.NewRow();
            dr["VIN"] = strVIN;
            dr["Temperature"] = envData.Temperature;
            dr["Humidity"] = envData.Humidity;
            dr["Pressure"] = envData.Pressure;
            dr["StartTime"] = startTime;
            dr["RunningTime"] = runningTime;
            dr["HC5025Limit"] = resultData.HC5025Limit;
            dr["CO5025Limit"] = resultData.CO5025Limit;
            dr["NO5025Limit"] = resultData.NO5025Limit;
            dr["HC5025"] = resultData.HC5025;
            dr["CO5025"] = resultData.CO5025;
            dr["NO5025"] = resultData.NO5025;
            dr["HC5025Evl"] = resultData.HC5025Evl == "合格" ? 1 : 0;
            dr["CO5025Evl"] = resultData.CO5025Evl == "合格" ? 1 : 0;
            dr["NO5025Evl"] = resultData.NO5025Evl == "合格" ? 1 : 0;
            dr["HC2540Limit"] = resultData.HC2540Limit;
            dr["CO2540Limit"] = resultData.CO2540Limit;
            dr["NO2540Limit"] = resultData.NO2540Limit;
            dr["HC2540"] = resultData.HC2540;
            dr["CO2540"] = resultData.CO2540;
            dr["NO2540"] = resultData.NO2540;
            dr["HC2540Evl"] = resultData.HC2540Evl == "合格" ? 1 : 0;
            dr["CO2540Evl"] = resultData.CO2540Evl == "合格" ? 1 : 0;
            dr["NO2540Evl"] = resultData.NO2540Evl == "合格" ? 1 : 0;
            dr["Result"] = resultData.Result == "合格" ? 1 : 0;
            dt.Rows.Add(dr);
            InsertRecords(dt.TableName, dt);
        }

        public void SaveFALResult(string strVIN, DateTime startTime, double runningTime, EnvironmentData envData, FALResultData resultData) {
            DataTable dt = new DataTable("FALResult");
            dt.Columns.Add("VIN");
            dt.Columns.Add("Temperature");
            dt.Columns.Add("Humidity");
            dt.Columns.Add("Pressure");
            dt.Columns.Add("StartTime");
            dt.Columns.Add("RunningTime");
            dt.Columns.Add("RatedRPM");
            dt.Columns.Add("MaxRPM");
            dt.Columns.Add("KLimit");
            dt.Columns.Add("KAvg");
            dt.Columns.Add("K1");
            dt.Columns.Add("K2");
            dt.Columns.Add("K3");
            dt.Columns.Add("Result");
            DataRow dr = dt.NewRow();
            dr["VIN"] = strVIN;
            dr["Temperature"] = envData.Temperature;
            dr["Humidity"] = envData.Humidity;
            dr["Pressure"] = envData.Pressure;
            dr["StartTime"] = startTime;
            dr["RunningTime"] = runningTime;
            dr["RatedRPM"] = resultData.RatedRPM;
            dr["MaxRPM"] = resultData.MaxRPM;
            dr["KLimit"] = resultData.KLimit;
            dr["KAvg"] = resultData.KAvg;
            dr["K1"] = resultData.K1;
            dr["K2"] = resultData.K2;
            dr["K3"] = resultData.K3;
            dr["Result"] = resultData.Result == "合格" ? 1 : 0;
            dt.Rows.Add(dr);
            InsertRecords(dt.TableName, dt);
        }

        public void SaveTSIResult(string strVIN, DateTime startTime, double runningTime, EnvironmentData envData, TSIResultData resultData) {
            DataTable dt = new DataTable("TSIResult");
            dt.Columns.Add("VIN");
            dt.Columns.Add("Temperature");
            dt.Columns.Add("Humidity");
            dt.Columns.Add("Pressure");
            dt.Columns.Add("StartTime");
            dt.Columns.Add("RunningTime");
            dt.Columns.Add("HighCOLimit");
            dt.Columns.Add("HighHCLimit");
            dt.Columns.Add("HighCO");
            dt.Columns.Add("HighHC");
            dt.Columns.Add("HighIdleResult");
            dt.Columns.Add("LowCOLimit");
            dt.Columns.Add("LowHCLimit");
            dt.Columns.Add("LowCO");
            dt.Columns.Add("LowHC");
            dt.Columns.Add("LowIdleResult");
            dt.Columns.Add("LambdaLimit");
            dt.Columns.Add("Lambda");
            dt.Columns.Add("LambdaResult");
            dt.Columns.Add("Result");
            DataRow dr = dt.NewRow();
            dr["VIN"] = strVIN;
            dr["Temperature"] = envData.Temperature;
            dr["Humidity"] = envData.Humidity;
            dr["Pressure"] = envData.Pressure;
            dr["StartTime"] = startTime;
            dr["RunningTime"] = runningTime;
            dr["HighCOLimit"] = resultData.HighCOLimit;
            dr["HighHCLimit"] = resultData.HighHCLimit;
            dr["HighCO"] = resultData.HighCO;
            dr["HighHC"] = resultData.HighHC;
            dr["HighIdleResult"] = resultData.HighIdleResult == "合格" ? 1 : 0;
            dr["LowCOLimit"] = resultData.LowCOLimit;
            dr["LowHCLimit"] = resultData.LowHCLimit;
            dr["LowCO"] = resultData.LowCO;
            dr["LowHC"] = resultData.LowHC;
            dr["LowIdleResult"] = resultData.LowIdleResult == "合格" ? 1 : 0;
            dr["LambdaLimit"] = resultData.LambdaLimit;
            dr["Lambda"] = resultData.Lambda;
            dr["LambdaResult"] = resultData.LambdaResult == "合格" ? 1 : 0;
            dr["Result"] = resultData.Result == "合格" ? 1 : 0;
            dt.Rows.Add(dr);
            InsertRecords(dt.TableName, dt);
        }

        public void SaveVMASResult(string strVIN, DateTime startTime, double runningTime, EnvironmentData envData, VMASResultData resultData) {
            DataTable dt = new DataTable("VMASResult");
            dt.Columns.Add("VIN");
            dt.Columns.Add("Temperature");
            dt.Columns.Add("Humidity");
            dt.Columns.Add("Pressure");
            dt.Columns.Add("StartTime");
            dt.Columns.Add("RunningTime");
            dt.Columns.Add("HCLimit");
            dt.Columns.Add("COLimit");
            dt.Columns.Add("NOLimit");
            dt.Columns.Add("HC");
            dt.Columns.Add("CO");
            dt.Columns.Add("NO");
            dt.Columns.Add("HCNO");
            dt.Columns.Add("HCEvl");
            dt.Columns.Add("COEvl");
            dt.Columns.Add("NOEvl");
            dt.Columns.Add("Result");
            DataRow dr = dt.NewRow();
            dr["VIN"] = strVIN;
            dr["Temperature"] = envData.Temperature;
            dr["Humidity"] = envData.Humidity;
            dr["Pressure"] = envData.Pressure;
            dr["StartTime"] = startTime;
            dr["RunningTime"] = runningTime;
            dr["HCLimit"] = resultData.HCLimit;
            dr["COLimit"] = resultData.COLimit;
            dr["NOLimit"] = resultData.NOLimit;
            dr["HC"] = resultData.HC;
            dr["CO"] = resultData.CO;
            dr["NO"] = resultData.NO;
            dr["HCNO"] = resultData.HCNO;
            dr["HCEvl"] = resultData.HCEvl == "合格" ? 1 : 0;
            dr["COEvl"] = resultData.COEvl == "合格" ? 1 : 0;
            dr["NOEvl"] = resultData.NOEvl == "合格" ? 1 : 0;
            dr["Result"] = resultData.Result == "合格" ? 1 : 0;
            dt.Rows.Add(dr);
            InsertRecords(dt.TableName, dt);
        }

    }
}
