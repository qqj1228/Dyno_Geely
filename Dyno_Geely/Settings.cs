﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Dyno_Geely {
    public enum OracleDB {
        Auto,
        MES,
        Dyno,
    }

    [Serializable]
    public class OracleSetting {
        public string Host { get; set; }
        public string Port { get; set; }
        public string ServiceName { get; set; }
        public string UserID { get; set; }
        public string PassWord { get; set; }

        public OracleSetting() {
            // 此默认值是MES中间表默认参数
            Host = "10.50.252.106";
            Port = "1561";
            ServiceName = "IUAT2";
            UserID = "idevice";
            PassWord = "idevice";
        }

    }

    [Serializable]
    public class SQLSetting {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string DBName { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }

        public SQLSetting() {
            UserName = "sa";
            PassWord = "sh49";
            DBName = "Orient_ASM";
            IP = "127.0.0.1";
            Port = "1433";
        }

    }

    [Serializable]
    public class FlowmeterSetting {
        public double O2SpanLow { get; set; }
        public double O2SpanHigh { get; set; }
        public double LowFlowSpan { get; set; }
        public double TargetPressure { get; set; }
        public double TargetTempe { get; set; }
        public FlowmeterSetting() {
            O2SpanLow = 19.8;
            O2SpanHigh = 21.8;
            LowFlowSpan = 97;
            TargetPressure = 101.3;
            TargetTempe = 23.5;
        }
    }

    [Serializable]
    public class SmokerSetting {
        public double ErrKStd { get; set; }
        public double K50Std { get; set; }
        public double K70Std { get; set; }
        public SmokerSetting() {
            ErrKStd = 0.05;
            K50Std = 0;
            K70Std = 0;
        }
    }

    [Serializable]
    public class WeatherSetting {
        public double ErrTempeStd { get; set; }
        public double ErrHumidityStd { get; set; }
        public double ErrPressureStd { get; set; }
        public double EnvTempe { get; set; }
        public double EnvHumidity { get; set; }
        public double EnvPressure { get; set; }
        public WeatherSetting() {
            ErrTempeStd = 0.5;
            ErrHumidityStd = 3;
            ErrPressureStd = 2;
            EnvTempe = 0;
            EnvHumidity = 0;
            EnvPressure = 101.3;
        }
    }

    [Serializable]
    public class MainSetting {
        public OracleSetting MES { get; set; }
        public SQLSetting Native { get; set; }
        public OracleSetting Dyno { get; set; }
        public FlowmeterSetting Flowmeter { get; set; }
        public SmokerSetting Smoker { get; set; }
        public WeatherSetting Weather { get; set; }
        public int RealtimeInterval { get; set; } // 获取实时数据的间隔时间
        public string Name { get; set; } // 操作员名字
        public int LastID { get; set; }
        public string DynoIP { get; set; } // 测功机服务器IP地址
        public int DynoPort { get; set; } // 测功机服务器端口号
        public int RecvTimeout { get; set; } // 发送命令后等待返回的超时时间
        public string ScannerPort { get; set; } // 扫码枪串口号，空字符串表示不用串口扫码枪
        public int ScannerBaud { get; set; } // 扫码枪波特率
        [XmlIgnore]
        public DataTable VMASSpeed { get; set; }


        public MainSetting() {
            MES = new OracleSetting();
            Native = new SQLSetting();
            Dyno = new OracleSetting();
            Flowmeter = new FlowmeterSetting();
            Smoker = new SmokerSetting();
            Weather = new WeatherSetting();
            RealtimeInterval = 1000;
            Name = "Emission";
            LastID = 0;
            DynoIP = "127.0.0.1";
            DynoPort = 5555;
            RecvTimeout = 5000;
            ScannerPort = string.Empty;
            ScannerBaud = 9600;
            VMASSpeed = new DataTable();
        }
    }

    [Serializable]
    public class DeviceInfo {
        public string AnalyManuf { get; set; }
        public string AnalyName { get; set; }
        public string AnalyModel { get; set; }
        public string AnalyDate { get; set; }
        public string DynoModel { get; set; }
        public string DynoManuf { get; set; }
        public DeviceInfo() {
            AnalyManuf = string.Empty;
            AnalyName = string.Empty;
            AnalyModel = string.Empty;
            AnalyDate = string.Empty;
            DynoModel = string.Empty;
            DynoManuf = string.Empty;
        }
    }

    public class VehicleInfo {
        public string VIN { get; set; }
        public string VehicleModel { get; set; } // 车辆型号
        public string OpenInfoSN { get; set; } // 信息公开编号
        public string TestQTY { get; set; } // 检测次数
    }

    public class EmissionInfo {
        public string VehicleModel { get; set; } // 车辆型号
        public string OpenInfoSN { get; set; } // 信息公开编号
        public string VehicleMfr { get; set; } // 车辆生产企业
        public string EngineModel { get; set; } // 发动机型号
        public string EngineSN { get; set; } // 发动机编号
        public string EngineMfr { get; set; } // 发动机生产企业
        public string EngineVolume { get; set; } // 发动机排量
        public string CylinderQTY { get; set; } // 气缸数量
        public string FuelSupply { get; set; } // 燃油供给系统
        public string RatedPower { get; set; } // 额定功率
        public int RatedRPM { get; set; } // 额定转速
        public string EmissionStage { get; set; } // 车辆排放阶段
        public string Transmission { get; set; } // 变速箱形式
        public string CatConverter { get; set; } // 催化转化器
        public string RefMass { get; set; } // 基准质量
        public string MaxMass { get; set; } // 最大设计总质量
        public string OBDLocation { get; set; } // OBD接口位置
        public string PostProcessing { get; set; } // 后处理类型
        public string PostProcessor { get; set; } // 后处理型号
        public string MotorModel { get; set; } // 电动机型号
        public string EnergyStorage { get; set; } // 储能装置型号
        public string BatteryCap { get; set; } // 电池容量
        public int TestMethod { get; set; } // 检测方法
        public string Name { get; set; } // 检验员名字
    }

    public class EnvironmentData {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
    }

    public class LDResultData {
        public int RatedRPM { get; set; }
        public int MaxRPM { get; set; }
        public double VelMaxHP { get; set; }
        public double RealMaxPowerLimit { get; set; }
        public double RealMaxPower { get; set; }
        public double KLimit { get; set; }
        public double K100 { get; set; }
        public double K80 { get; set; }
        public double NOx80Limit { get; set; }
        public double NOx80 { get; set; }
        public string Result { get; set; }
    }

    public class ASMResultData {
        public string HC5025Limit { get; set; }
        public string CO5025Limit { get; set; }
        public string NO5025Limit { get; set; }
        public string HC5025 { get; set; }
        public string CO5025 { get; set; }
        public string NO5025 { get; set; }
        public string HC5025Evl { get; set; }
        public string CO5025Evl { get; set; }
        public string NO5025Evl { get; set; }
        public string HC2540Limit { get; set; }
        public string CO2540Limit { get; set; }
        public string NO2540Limit { get; set; }
        public string HC2540 { get; set; }
        public string CO2540 { get; set; }
        public string NO2540 { get; set; }
        public string HC2540Evl { get; set; }
        public string CO2540Evl { get; set; }
        public string NO2540Evl { get; set; }
        public string Result { get; set; }
    }

    public class FALResultData {
        public int RatedRPM { get; set; }
        public int MaxRPM { get; set; }
        public double KLimit { get; set; }
        public double KAvg { get; set; }
        public double K1 { get; set; }
        public double K2 { get; set; }
        public double K3 { get; set; }
        public string Result { get; set; }
    }

    public class TSIResultData {
        public double HighCOLimit { get; set; }
        public double HighHCLimit { get; set; }
        public double HighCO { get; set; }
        public double HighHC { get; set; }
        public string HighIdleResult { get; set; }
        public double LowCOLimit { get; set; }
        public double LowHCLimit { get; set; }
        public double LowCO { get; set; }
        public double LowHC { get; set; }
        public string LowIdleResult { get; set; }
        public double LambdaLimit { get; set; }
        public double Lambda { get; set; }
        public string LambdaResult { get; set; }
        public string Result { get; set; }
    }

    public class VMASResultData {
        public string HCLimit { get; set; }
        public string COLimit { get; set; }
        public string NOLimit { get; set; }
        public string HC { get; set; }
        public string CO { get; set; }
        public string NO { get; set; }
        public string HCNO { get; set; }
        public string HCEvl { get; set; }
        public string COEvl { get; set; }
        public string NOEvl { get; set; }
        public string Result { get; set; }
    }

}