﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Dyno_Geely {
    public class ModelOracle {
        private readonly Logger _log;
        private readonly OracleSetting _oracleMES;
        private readonly OracleSetting _oracleDyno;
        private string ConnectionMes { get; set; }
        private string ConnectionDyno { get; set; }
        public bool Connected { get; set; }

        public ModelOracle(OracleSetting oracleMES, OracleSetting oracleDyno, Logger log) {
            _log = log;
            _oracleMES = oracleMES;
            ConnectionMes = ReadConfig(oracleMES);
            _oracleDyno = oracleDyno;
            ConnectionDyno = ReadConfig(oracleDyno);
            Connected = false;
        }

        string ReadConfig(OracleSetting cfg) {
            string strRet = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=";
            strRet += cfg.Host + ")(PORT=";
            strRet += cfg.Port + "))(CONNECT_DATA=(SERVICE_NAME=";
            strRet += cfg.ServiceName + ")));";
            strRet += "Persist Security Info=True;";
            strRet += "User ID=" + cfg.UserID + ";";
            strRet += "Password=" + cfg.PassWord + ";";
            return strRet;
        }

        public void TestConnect(OracleDB DBType = OracleDB.MES) {
            string strConn = string.Empty;
            if (DBType == OracleDB.MES) {
                strConn = ConnectionMes;
            } else if (DBType == OracleDB.Dyno) {
                strConn = ConnectionDyno;
            }
            using (OracleConnection mySqlConn = new OracleConnection(strConn)) {
                mySqlConn.Open();
                mySqlConn.Close();  
            }
        }

        /// <summary>
        /// 执行update insert delete语句，失败了返回-1，成功了返回影响的行数,注意：自动commit
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        private int ExecuteNonQuery(string strSQL, string Connection) {
            using (OracleConnection connection = new OracleConnection(Connection)) {
                int val = -1;
                try {
                    connection.Open();
                    OracleCommand cmd = new OracleCommand(strSQL, connection);
                    val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                } catch (OracleException ex) {
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
            using (OracleConnection connection = new OracleConnection(Connection)) {
                try {
                    connection.Open();
                    OracleDataAdapter adapter = new OracleDataAdapter(strSQL, connection);
                    adapter.Fill(dt);
                } catch (OracleException ex) {
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
            using (OracleConnection connection = new OracleConnection(Connection)) {
                using (OracleCommand cmd = new OracleCommand(strSQL, connection)) {
                    try {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value))) {
                            return null;
                        } else {
                            return obj;
                        }
                    } catch (OracleException ex) {
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

        public int InsertRecords(string strTable, DataTable dt, OracleDB DBType = OracleDB.MES) {
            int iRet = 0;
            for (int iRow = 0; iRow < dt.Rows.Count; iRow++) {
                string strSQL = "insert into " + strTable + " (ID,";
                for (int iCol = 1; iCol < dt.Columns.Count; iCol++) {
                    if (dt.Rows[iRow][iCol].ToString().Length != 0) {
                        strSQL += dt.Columns[iCol].ColumnName + ",";
                    }
                }
                strSQL = strSQL.Trim(',');
                strSQL += ") values (SEQ_EM_WQPF_ID.NEXTVAL,";
                for (int iCol = 1; iCol < dt.Columns.Count; iCol++) {
                    if (dt.Rows[iRow][iCol].ToString().Length != 0) {
                        if (dt.Columns[iCol].DataType == typeof(DateTime)) {
                            strSQL += "to_date('" + ((DateTime)dt.Rows[iRow][iCol]).ToString("yyyyMMdd-HHmmss") + "', 'yyyymmdd-HH24MISS'),";
                        } else {
                            strSQL += "'" + dt.Rows[iRow][iCol].ToString() + "',";
                        }
                    }
                }
                strSQL = strSQL.Trim(',');
                strSQL += ")";
                if (DBType == OracleDB.MES) {
                    iRet += ExecuteNonQuery(strSQL, ConnectionMes);
                } else if (DBType == OracleDB.Dyno) {
                    iRet += ExecuteNonQuery(strSQL, ConnectionDyno);
                }
            }
            return iRet;
        }

        public int UpdateRecords(string strTable, DataTable dt, string strWhereKey, string[] strWhereValues, OracleDB DBType = OracleDB.MES) {
            int iRet = 0;
            if (dt.Rows.Count != strWhereValues.Length) {
                return -1;
            }
            for (int iRow = 0; iRow < dt.Rows.Count; iRow++) {
                string strSQL = "update " + strTable + " set ";
                for (int iCol = 1; iCol < dt.Columns.Count; iCol++) {
                    if (dt.Rows[iRow][iCol].ToString().Length != 0 && dt.Rows[iRow][iCol].ToString() != dt.Columns[iCol].ColumnName) {
                        if (dt.Columns[iCol].DataType == typeof(DateTime)) {
                            strSQL += dt.Columns[iCol].ColumnName + "=" + "to_date('" + ((DateTime)dt.Rows[iRow][iCol]).ToString("yyyyMMdd-HHmmss") + "', 'yyyymmdd-HH24MISS'),";
                        } else {
                            strSQL += dt.Columns[iCol].ColumnName + "='" + dt.Rows[iRow][iCol].ToString() + "',";
                        }
                    }
                }
                strSQL = strSQL.Trim(',');
                strSQL += " where " + strWhereKey + "='" + strWhereValues[iRow] + "'";
                if (DBType == OracleDB.MES) {
                    iRet += ExecuteNonQuery(strSQL, ConnectionMes);
                } else if (DBType == OracleDB.Dyno) {
                    iRet += ExecuteNonQuery(strSQL, ConnectionDyno);
                }
            }
            return iRet;
        }

        public string[] GetValue(string strTable, string strField, string strWhereKey, string strWhereValue, OracleDB DBType = OracleDB.MES) {
            string strSQL = "select " + strField + " from " + strTable + " where " + strWhereKey + " = '" + strWhereValue + "'";
            DataTable dt = new DataTable();
            if (DBType == OracleDB.MES) {
                Query(strSQL, dt, ConnectionMes);
            } else if (DBType == OracleDB.Dyno) {
                Query(strSQL, dt, ConnectionDyno);
            }
            string[] values = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++) {
                values[i] = dt.Rows[i][0].ToString();
            }
            Array.Sort(values);
            dt.Dispose();
            return values;
        }


        public int GetCNLenb(OracleDB DBType = OracleDB.MES) {
            int iRet = 0;
            string strSQL = "select lengthb('好') from dual";
            if (DBType == OracleDB.MES) {
                iRet = Convert.ToInt32(QueryOne(strSQL, ConnectionMes));
            } else if (DBType == OracleDB.Dyno) {
                iRet = Convert.ToInt32(QueryOne(strSQL, ConnectionDyno));
            }
            return iRet;
        }

        private void USP_GET_ENVIRONMENT_DATA(string strVIN, DataTable dtOut, out string errMsg) {
            using (OracleConnection connection = new OracleConnection(ConnectionDyno)) {
                try {
                    connection.Open();
                    // 调用存储过程名
#if DEBUG
                    // 本地测试使用用户ID
                    string cmdText = _oracleDyno.UserID + ".PKG_IF_MES.USP_GET_ENVIRONMENT_DATA";
#else
                    // 现场生产环境使用服务名
                    string cmdText = _oracleDyno.ServiceName + ".PKG_IF_MES.USP_GET_ENVIRONMENT_DATA";
#endif
                    _log.TraceInfo("OracleCommand Text: " + cmdText);
                    OracleCommand cmd = new OracleCommand(cmdText, connection) {
                        CommandType = CommandType.StoredProcedure
                    };
                    // 添加存储过程参数
                    OracleParameter IN_VIN = new OracleParameter {
                        ParameterName = "IN_VIN",
                        OracleDbType = OracleDbType.Varchar2,
                        Direction = ParameterDirection.Input,
                        Size = 17,
                        Value = strVIN
                    };
                    cmd.Parameters.Add(IN_VIN);

                    OracleParameter IN_MAKE_DATE = new OracleParameter {
                        ParameterName = "IN_MAKE_DATE",
                        OracleDbType = OracleDbType.Date,
                        Direction = ParameterDirection.Input,
                        Value = DateTime.Now
                    };
                    cmd.Parameters.Add(IN_MAKE_DATE);

                    OracleParameter OUT_DATA_SET = new OracleParameter {
                        ParameterName = "OUT_DATA_SET",
                        OracleDbType = OracleDbType.RefCursor,
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(OUT_DATA_SET);

                    OracleParameter ERROR_MESSAGE = new OracleParameter {
                        ParameterName = "ERROR_MESSAGE",
                        OracleDbType = OracleDbType.Varchar2,
                        Direction = ParameterDirection.Output,
                        Size = 1000
                    };
                    cmd.Parameters.Add(ERROR_MESSAGE);

                    // 执行存储过程并封装游标输出结果
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    adapter.Fill(dtOut);

                    // 获取输出参数结果
                    errMsg = cmd.Parameters["ERROR_MESSAGE"].Value.ToString();
                } catch (OracleException ex) {
                    _log.TraceError("Error Stored Procedure: USP_GET_ENVIRONMENT_DATA");
                    _log.TraceError(ex.Message);
                    throw new Exception(ex.Message);
                } finally {
                    if (connection.State != ConnectionState.Closed) {
                        connection.Close();
                    }
                }
            }
        }

        public void GetEmissionInfo(string strVIN, EmissionInfo ei, out string errMsg) {
            DataTable dtDynoParam = new DataTable("DYNO_PARAM");
            USP_GET_ENVIRONMENT_DATA(strVIN, dtDynoParam, out errMsg);
            if (dtDynoParam.Rows.Count > 0) {
                DataRow dr = dtDynoParam.Rows[dtDynoParam.Rows.Count - 1];
                ei.VehicleModel = dr["VEHICLE_MODEL_S"].ToString();
                ei.OpenInfoSN = dr["XXGKBH_S"].ToString();
                ei.VehicleMfr = dr["SCCMC_S"].ToString();
                ei.EngineModel = dr["FDJXH_S"].ToString();
                ei.EngineSN = string.Empty;
                ei.EngineMfr = dr["FDJSCQY_S"].ToString();
                ei.EngineVolume = dr["FDJPL_S"].ToString();
                ei.CylinderQTY = dr["QGS_S"].ToString();
                ei.FuelSupply = dr["RYGJSYS_S"].ToString();
                ei.RatedPower = dr["FDJEDGL_S"].ToString();
                ei.RatedRPM = Convert.ToInt32(dr["EDZHS_S"]);
                ei.EmissionStage = dr["CLPFJD_S"].ToString();
                ei.Transmission = dr["BSQXSH_S"].ToString();
                ei.CatConverter = dr["CHZHHQXH_S"].ToString();
                ei.RefMass = dr["JZHZHL_S"].ToString();
                ei.MaxMass = dr["MAXSJZZHL_S"].ToString();
                ei.OBDLocation = dr["OBDJKWZH_S"].ToString();
                ei.PostProcessing = dr["HCHLLX_S"].ToString();
                ei.PostProcessor = dr["HCHLXH_S"].ToString();
                ei.MotorModel = dr["DDJXH_S"].ToString();
                ei.EnergyStorage = dr["CHNZHZHXH_S"].ToString();
                ei.BatteryCap = dr["DCHRL_S"].ToString();
                // 处理检测方法字符串
                string strTestMethod = dr["JCFF_S"].ToString();
                if (strTestMethod.Contains("双怠速")) {
                    ei.TestMethod = 1;
                } else if (strTestMethod.Contains("稳态工况")) {
                    ei.TestMethod = 2;
                } else if (strTestMethod.Contains("简易瞬态")) {
                    ei.TestMethod = 3;
                } else if (strTestMethod.Contains("加载减速")) {
                    ei.TestMethod = 4;
                } else if (strTestMethod.Contains("自由加速")) {
                    ei.TestMethod = 6;
                } else if (strTestMethod.Contains("林格曼")) {
                    ei.TestMethod = 7;
                } else if (strTestMethod.Contains("瞬态工况")) {
                    ei.TestMethod = 8;
                } else {
                    ei.TestMethod = 9;
                }
                ei.Name = string.Empty;
            }
        }

    }
}