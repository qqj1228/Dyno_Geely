using Dyno_Geely;
using LibBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Dyno_Geely {
    public class ModelLocal : ModelBase {
        private readonly SQLSetting _setting;

        public ModelLocal(SQLSetting setting, DataBaseType type, Logger log) {
            _setting = setting;
            ModelParameter dbParam = new ModelParameter {
                DataBaseType = type,
                UserName = _setting.UserName,
                PassWord = _setting.PassWord,
                Host = _setting.IP,
                Port = _setting.Port,
                DBorService = _setting.DBName
            };
            InitDataBase(dbParam, log);
        }

        public int DeleteAllRecords(string strTable) {
            return DeleteRecord(strTable, null);
        }

        public int DeleteRecordByID(string strTable, string strID) {
            return DeleteRecords(strTable, "ID", new List<string>() { strID });
        }

        public DateTime GetPreheating() {
            DataTable dt = new DataTable("SH_Preheating");
            dt.Columns.Add("LastTime", typeof(DateTime));
            GetRecords(dt, new Dictionary<string, string> { { "ID", "1" } });
            return (DateTime)dt.Rows[0]["LastTime"];
        }

        public void UpdatePreheating() {
            DataTable dt = new DataTable("SH_Preheating");
            dt.Columns.Add("LastTime", typeof(DateTime));
            DataRow dr = dt.NewRow();
            dr["LastTime"] = DateTime.Now;
            dt.Rows.Add(dr);
            UpdateRecords(dt, "ID", new List<string>() { "1" });
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
            string strSQL = "select * from SH_VehicleInfo where VIN = '" + strVIN + "'";
            DataTable dtVI = new DataTable("VehicleInfo");
            Query(strSQL, dtVI);
            if (dtVI.Rows.Count > 0) {
                FillClassFromDataTable(dtVI, vi);
            }
        }

        public void SetTestQTYInVehicleInfo(string strVIN) {
            string strSQL = "select * from SH_VehicleInfo where VIN = '" + strVIN + "'";
            DataTable dtVI = new DataTable("SH_VehicleInfo");
            Query(strSQL, dtVI);
            if (dtVI.Rows.Count > 0) {
                DataRow dr = dtVI.Rows[dtVI.Rows.Count - 1];
                dr["TestQTY"] = Convert.ToInt32(dr["TestQTY"]) + 1;
                UpdateRecords(dtVI, "ID", new List<string>() { dr["ID"].ToString() });
            }
        }

        public void GetEmissionInfo(string strVIN, EmissionInfo ei) {
            string strSQL = "select * from SH_VehicleInfo where VIN = '" + strVIN + "'";
            DataTable dtVI = new DataTable("SH_VehicleInfo");
            Query(strSQL, dtVI);
            if (dtVI.Rows.Count > 0) {
                DataRow dr = dtVI.Rows[dtVI.Rows.Count - 1];
                DataTable dtEI = new DataTable("SH_EmissionInfo");
                strSQL = "select * from SH_EmissionInfo where VehicleModel = '" + dr["VehicleModel"] + "' and OpenInfoSN = '" + dr["OpenInfoSN"] + "'";
                Query(strSQL, dtEI);
                FillClassFromDataTable(dtEI, ei);
            }
        }

        public void SaveEmissionInfo(string strVIN, EmissionInfo ei) {
            string strSQL = "select * from SH_VehicleInfo where VIN = '" + strVIN + "'";
            DataTable dtVI = new DataTable("SH_VehicleInfo");
            Query(strSQL, dtVI);

            strSQL = "select ID from SH_EmissionInfo where VehicleModel = '" + ei.VehicleModel + "' and OpenInfoSN = '" + ei.OpenInfoSN + "'";
            object EI_ID = QueryOne(strSQL);

            DataTable dtEI = new DataTable("SH_EmissionInfo");
            GenerateDataTableFromClass(dtEI, ei);
            if (dtVI.Rows.Count > 0) {
                if (EI_ID != null) {
                    UpdateRecords(dtEI, "ID", new List<string>() { EI_ID.ToString() });
                } else {
                    InsertRecords(dtEI);
                }
            } else {
                dtVI.Columns.Remove("TestQTY");
                DataRow dr = dtVI.NewRow();
                dr["VIN"] = strVIN;
                dr["VehicleModel"] = ei.VehicleModel;
                dr["OpenInfoSN"] = ei.OpenInfoSN;
                dtVI.Rows.Add(dr);
                InsertRecords(dtVI);
                InsertRecords(dtEI);
            }
        }

        public void SaveLDResult(string strVIN, DateTime startTime, double runningTime, EnvironmentData envData, LDResultData resultData) {
            DataTable dt = new DataTable("SH_LugdownResult");
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
            InsertRecords(dt);
        }

        public void SaveASMResult(string strVIN, DateTime startTime, double runningTime, EnvironmentData envData, ASMResultData resultData) {
            DataTable dt = new DataTable("SH_ASMResult");
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
            InsertRecords(dt);
        }

        public void SaveFALResult(string strVIN, DateTime startTime, double runningTime, EnvironmentData envData, FALResultData resultData) {
            DataTable dt = new DataTable("SH_FALResult");
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
            InsertRecords(dt);
        }

        public void SaveTSIResult(string strVIN, DateTime startTime, double runningTime, EnvironmentData envData, TSIResultData resultData) {
            DataTable dt = new DataTable("SH_TSIResult");
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
            InsertRecords(dt);
        }

        public void SaveVMASResult(string strVIN, DateTime startTime, double runningTime, EnvironmentData envData, VMASResultData resultData) {
            DataTable dt = new DataTable("SH_VMASResult");
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
            InsertRecords(dt);
        }

    }
}
