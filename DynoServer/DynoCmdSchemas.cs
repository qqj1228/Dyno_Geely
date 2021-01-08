using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno_Geely {
    public class MsgBaseStr {
        public string MsgID { get; set; }
        public string ServiceID { get; set; }
        public string Cmd { get; set; }
        public object Params { get; set; }
    }

    public class MsgAckBaseStr {
        public string MsgID { get; set; }
        public string ServiceID { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
        public string Cmd { get; set; }
        public object Params { get; set; }
    }

    public class LoginParams {
        public string ClientID { get; set; }
    }
    public class LoginAckParams {
        public string permissionCtrl { get; set; }
        public string permissionView { get; set; }
    }

    public class StartDynoPreheatParams {
        public string ClientID { get; set; }
        public bool stopCheck { get; set; }
    }
    public class StartDynoPreheatAckParams {
        public string msg { get; set; }
    }

    public class GetDynoPreheatRealTimeDataParams {
        public string ClientID { get; set; }
    }
    public class GetDynoPreheatRealTimeDataAckParams {
        public string msg { get; set; }
        public double speed { get; set; }
    }

    public class SaveDynoPreheatDataParams {
        public string ClientID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Operator { get; set; }
    }

    public class StartGasBoxPreheatSelfCheckParams {
        public string ClientID { get; set; }
        public bool stopCheck { get; set; }
        public int step { get; set; }
    }

    public class GetGasBoxPreheatSelfCheckRealTimeDataParams {
        public string ClientID { get; set; }
        public bool Abandon { get; set; }
    }
    public class GetGasBoxPreheatSelfCheckRealTimeDataAckParams {
        public string msg { get; set; }
        public double HC { get; set; }
        public double NO { get; set; }
        public double CO { get; set; }
        public double CO2 { get; set; }
        public double O2 { get; set; }
        public double PEF { get; set; }
        public string GasBoxPreheatWarmUpResult { get; set; }
        public string GasBoxPreheatZeroResult { get; set; }
        public string GasBoxPreheatLeakCheckResult { get; set; }
        public string GasBoxPreheatLowFlowResult { get; set; }
        public string GasBoxPreheatO2SpanCheckResult { get; set; }
        public string GasBoxPreheatResult { get; set; }
    }

    public class StartFlowmeterCheckParams {
        public string ClientID { get; set; }
        public bool stopCheck { get; set; }
        public int step { get; set; }
    }
    public class StartFlowmeterCheckAckParams {
        public double FlowmeterO2SpanLow { get; set; }
        public double FlowmeterO2SpanHight { get; set; }
        public double FlowmeterLowFlowSpan { get; set; }
        public double FlowmeterTargetPressure { get; set; }
        public double FlowmeterTargetTempe { get; set; }
    }

    public class GetFlowmeterCheckRealTimeDataParams {
        public string ClientID { get; set; }
        public bool Abandon { get; set; }
    }
    public class GetFlowmeterCheckRealTimeDataAckParams {
        public string msg { get; set; }
        public double Flow { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public double DiluteO2 { get; set; }
        public string FlowCheckResult { get; set; }
        public string O2SpanCheckResult { get; set; }
        public string TempeCheckResult { get; set; }
        public string PressureCheckResult { get; set; }
        public string FlowmeterCheckResult { get; set; }
    }

    public class GetWeatherCheckRealTimeDataParams {
        public string ClientID { get; set; }
    }
    public class GetWeatherCheckRealTimeDataAckParams {
        public string msg { get; set; }
        public double EnvTemperature { get; set; }
        public double EnvHumidity { get; set; }
        public double EnvPressure { get; set; }
    }

    public class SaveWeatherCheckDataParams {
        public string ClientID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string CommCheck { get; set; }
        public double EnvTemperature { get; set; }
        public double InstrumentTemperature { get; set; }
        public double EnvHumidity { get; set; }
        public double InstrumentHumidity { get; set; }
        public double EnvPressure { get; set; }
        public double InstrumentPressure { get; set; }
        public string Result { get; set; }
    }

    public class GetSmokerPreheatSelfCheckRealTimeDataParams {
        public string ClientID { get; set; }
    }
    public class GetSmokerPreheatSelfCheckRealTimeDataAckParams {
        public string msg { get; set; }
        public double K { get; set; }
    }

    public class SaveSmokerPreheatSelfCheckDataParams {
        public string ClientID { get; set; }
        public double Smoker70LabelValue { get; set; }
        public double Smoker70TestValue { get; set; }
        public double Smoker70Ratio { get; set; }
        public double Smoker50LabelValue { get; set; }
        public double Smoker50TestValue { get; set; }
        public double Smoker50Ratio { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Result { get; set; }
    }

    public class GetEquipmentPreheatTimeAndCheckTimeParams {
        public string ClientID { get; set; }
    }
    public class GetEquipmentPreheatTimeAndCheckTimeAckParams {
        public string QYGasBoxRemindInfo { get; set; }
        public double dynoPreheatCheckPeriodHour { get; set; }
        public DateTime dynoPreheatCheckTime { get; set; }
        public double gasBosQYPreheatCheckPeriodHour { get; set; }
        public DateTime gasBosQYPreheatCheckTime { get; set; }
        public double gasBosCYPreheatCheckPeriodHour { get; set; }
        public DateTime gasBosCYPreheatCheckTime { get; set; }
        public double flowmeterPreheatCheckPeriodHour { get; set; }
        public DateTime flowmeterPreheatCheckTime { get; set; }
        public double lightSmokeCheckPeriodHour { get; set; }
        public DateTime lightSmokePreheatCheckTime { get; set; }
        public double filterSmokePreheatCheckPeriodHour { get; set; }
        public DateTime filterSmokePreheatCheckTime { get; set; }
        public double weatherPreheatCheckPeriodHour { get; set; }
        public DateTime weatherPreheatCheckTime { get; set; }
        public double dynoTaxiCheckPeriodHour { get; set; }
        public DateTime dynoTaxiCheckTime { get; set; }
        public double dynoSpeedCheckPeriodHour { get; set; }
        public DateTime dynoSpeedCheckTime { get; set; }
        public double dynoTorqueFCheckPeriodHour { get; set; }
        public DateTime dynoTorqueFCheckTime { get; set; }
        public double dynoDIWCheckPeriodHour { get; set; }
        public DateTime dynoDIWCheckTime { get; set; }
        public double dynoPLHPCheckPeriodHour { get; set; }
        public DateTime dynoPLHPCheckTime { get; set; }
        public double gasBoxLowGasCheckPeriodHour { get; set; }
        public DateTime gasBoxLowGasCheckTime { get; set; }
        public double gasBoxLeakCheckPeriodHour { get; set; }
        public DateTime gasBoxLeakCheckTime { get; set; }
        public double gasBoxNOxConvertRatioCheckPeriodHour { get; set; }
        public DateTime gasBoxNOxConvertRatioCheckTime { get; set; }
        public double gasBoxSinglePointCalibrationCheckPeriodHour { get; set; }
        public DateTime gasBoxSinglePointCalibrationCheckTime { get; set; }
        public double flowmeterFlowCheckPeriodHour { get; set; }
        public DateTime flowmeterFlowCheckTime { get; set; }
        public double flowmeterO2CheckPeriodHour { get; set; }
        public DateTime flowmeterO2CheckTime { get; set; }
        public double tachometerQYCheckPeriodHour { get; set; }
        public DateTime tachometerQYCheckTime { get; set; }
        public double tachometerCYCheckPeriodHour { get; set; }
        public DateTime tachometerCYCheckTime { get; set; }
        public double oiltempCheckPeriodHour { get; set; }
        public DateTime oiltempCheckTime { get; set; }
        public DateTime lastCarCheckTime { get; set; }
    }

    public class SetEquipmentPreheatTimeAndCheckTimeParams {
        public string QYGasBoxRemindInfo { get; set; }
        public double dynoPreheatCheckPeriodHour { get; set; }
        public DateTime dynoPreheatCheckTime { get; set; }
        public double gasBosQYPreheatCheckPeriodHour { get; set; }
        public DateTime gasBosQYPreheatCheckTime { get; set; }
        public double gasBosCYPreheatCheckPeriodHour { get; set; }
        public DateTime gasBosCYPreheatCheckTime { get; set; }
        public double flowmeterPreheatCheckPeriodHour { get; set; }
        public DateTime flowmeterPreheatCheckTime { get; set; }
        public double lightSmokeCheckPeriodHour { get; set; }
        public DateTime lightSmokePreheatCheckTime { get; set; }
        public double filterSmokePreheatCheckPeriodHour { get; set; }
        public DateTime filterSmokePreheatCheckTime { get; set; }
        public double weatherPreheatCheckPeriodHour { get; set; }
        public DateTime weatherPreheatCheckTime { get; set; }
    }

    public class StartGasboxPrepareParams {
        public string ClientID { get; set; }
        public bool IsStopGasboxPrepare { get; set; }
        public bool Abandon { get; set; }
    }
    public class StartGasboxPrepareAckParams {
        public string ClientID { get; set; }
    }

    public class GetGasboxPrepareRealTimeDataParams {
        public string ClientID { get; set; }
        public bool IsStartCheck { get; set; }
        public bool Abandon { get; set; }
    }
    public class GetGasboxPrepareRealTimeDataAckParams {
        public string msg { get; set; }
        public int step { get; set; }
        public bool Zero { get; set; }
        public bool AmibientCheck { get; set; }
        public bool BackGroundCheck { get; set; }
        public bool HCResidualCheck { get; set; }
        public bool O2SpanCheck { get; set; }
        public bool TestGasInLowFlowCheck { get; set; }
        public string AmibientHC { get; set; }
        public string AmibientCO { get; set; }
        public string AmibientCO2 { get; set; }
        public string AmibientNO { get; set; }
        public string AmibientO2 { get; set; }
        public string BackHC { get; set; }
        public string BackCO { get; set; }
        public string BackCO2 { get; set; }
        public string BackNO { get; set; }
        public string BackO2 { get; set; }
        public string ResidualHC { get; set; }
        public string NowOperationTimeRemaining { get; set; }
        public string HCOperationTimeRemaining { get; set; }
        public double SumCO2CO { get; set; }
    }

    public class StartFlowmeterPrepareParams {
        public string ClientID { get; set; }
        public bool IsStopFlowmeterPrepare { get; set; }
    }

    public class GetFlowmeterPrepareRealTimeDataParams {
        public string ClientID { get; set; }
        public bool IsStartCheck { get; set; }
        public bool Abandon { get; set; }
    }
    public class GetFlowmeterPrepareRealTimeDataAckParams {
        public string msg { get; set; }
        public int step { get; set; }
        public string flow { get; set; }
        public string O2 { get; set; }
        public string time { get; set; }
        public string ZeroResult { get; set; }
        public string FlowCheckResult { get; set; }
        public string O2SpanCheckResult { get; set; }
    }

    public class StartSmokePrepareParams {
        public string ClientID { get; set; }
        public bool IsStopSmokePrepare { get; set; }
    }

    public class GetSmokePrepareRealTimeDataParams {
        public string ClientID { get; set; }
        public bool IsStartCheck { get; set; }
        public bool Abandon { get; set; }
    }
    public class GetSmokePrepareRealTimeDataAckParams {
        public string msg { get; set; }
        public int step { get; set; }
        public string Ns { get; set; }
        public string K { get; set; }
        public double CO2 { get; set; }
        public string Zero { get; set; }
        public bool DistancepointCheck { get; set; }
    }

    public class GetOilTempPrepareRealTimeDataParams {
        public string ClientID { get; set; }
        public bool IsStartCheck { get; set; }
        public bool Abandon { get; set; }
    }
    public class GetOilTempPrepareRealTimeDataAckParams {
        public double oilTemp { get; set; }
        public string oilTitle { get; set; }
    }

    public class GetTachometerPrepareRealTimeDataParams {
        public string ClientID { get; set; }
        public bool IsStartCheck { get; set; }
        public bool Abandon { get; set; }
    }
    public class GetTachometerPrepareRealTimeDataAckParams {
        public string RPM { get; set; }
    }

    public class GetWeatherPrepareRealTimeDataParams {
        public string ClientID { get; set; }
        public bool IsStartCheck { get; set; }
        public bool Abandon { get; set; }
    }
    public class GetWeatherPrepareRealTimeDataAckParams {
        public double temperature { get; set; }
        public double humidity { get; set; }
        public double amibientPressure { get; set; }
    }

    public class GetWaitCheckQueueInfoParams {
        public string ClientID { get; set; }
    }
    public class GetWaitCheckQueueInfoAckParams {
        public string waitCheckQueueInfo { get; set; }
    }

    public class SaveVehicleAppearanceInfoParams {
        public string ClientID { get; set; }
        public string SurfaceCheckData { get; set; }
    }
    public class SaveVehicleAppearanceInfoAckParams {
        public string SurfaceCheckMark { get; set; }
    }

    public class StartOBDCheckParams {
        public string ClientID { get; set; }
        public bool stopCheck { get; set; }
    }
    public class StartOBDCheckAckParams {
        public bool SkipOBDCheck { get; set; }
    }

    public class GetOBDRealTimeDataParams {
        public string ClientID { get; set; }
        public bool stopCheck { get; set; }
        public bool Abandon { get; set; }
        public bool GZZSQSFTG { get; set; }
        public bool DHSGZZSQSFDL { get; set; }
        public bool DHHGZZSQSFXM { get; set; }
        public bool ManualSelectionOK { get; set; }
        public bool GZDZTSFYZ { get; set; }
        public bool txqkyy { get; set; }
        public bool IsInUseCar { get; set; }
    }
    public class GetOBDRealTimeDataAckParams {
        public string msg { get; set; }
        public int step { get; set; }
        public string hphm { get; set; }
        public string hpys { get; set; }
        public string vin { get; set; }
        public string rylx { get; set; }
        public string gzzsqbj { get; set; }
        public string txqk { get; set; }
        public string GZDMGZXX { get; set; }
        public string JXZTWWCXM { get; set; }
        public string MILXSLC { get; set; }
        public string FDJKZCALID { get; set; }
        public string FDJKZCVN { get; set; }
        public string HCLKZCALID { get; set; }
        public string HCLKZCVN { get; set; }
        public string QTKZCALID { get; set; }
        public string QTKZCVN { get; set; }
        public string OBDCheckResult { get; set; }
        public string GZDZT { get; set; }
    }

    public class StartASMCheckParams {
        public string ClientID { get; set; }
        public bool stopCheck { get; set; }
    }

    public class GetASMRealTimeDataParams {
        public string ClientID { get; set; }
        public bool stopCheck { get; set; }
    }
    public class GetASMRealTimeDataAckParams {
        public int SpeedMax { get; set; }
        public int SpeedMin { get; set; }
        public double PaintGreen { get; set; }
        public double PaintOrange { get; set; }
        public string msg { get; set; }
        public int step { get; set; }
        public double speed { get; set; }
        public int RPM { get; set; }
        public double testTime { get; set; }
        public double singleWorkingCondition { get; set; }
        public double accTime { get; set; }
        public double stableTime { get; set; }
        public double lmd { get; set; }
        public double speedContinuityOverProofTime { get; set; }
        public double speedCumulativeOverProofTime { get; set; }
        public double torqueContinuityOverProofTime { get; set; }
        public double torqueCumulativeOverProofTime { get; set; }
        public bool autoPrintAfterCheck { get; set; }
    }

    public class GetASMCheckResultDataParams {
        public string ClientID { get; set; }
    }
    public class GetASMCheckResultAckParams {
        public string msg { get; set; }
        public int step { get; set; }
        public string HC5025Limit { get; set; }
        public string CO5025Limit { get; set; }
        public string NO5025Limit { get; set; }
        public string HC2540Limit { get; set; }
        public string CO2540Limit { get; set; }
        public string NO2540Limit { get; set; }
        public string HC5025 { get; set; }
        public string CO5025 { get; set; }
        public string NO5025 { get; set; }
        public string HC5025Evl { get; set; }
        public string CO5025Evl { get; set; }
        public string NO5025Evl { get; set; }
        public string HC2540 { get; set; }
        public string CO2540 { get; set; }
        public string NO2540 { get; set; }
        public string HC2540Evl { get; set; }
        public string CO2540Evl { get; set; }
        public string NO2540Evl { get; set; }
        public string ASMCheckeResult { get; set; }
    }

    public class StartTsiCheckParams {
        public string ClientID { get; set; }
        public bool stopCheck { get; set; }
    }

    public class GetTsiRealTimeDataParams {
        public string ClientID { get; set; }
        public bool stopCheck { get; set; }
        public bool ignoreRpm70 { get; set; }
        public bool ignoreRpmHigh { get; set; }
        public bool ignoreRpmLow { get; set; }
        public bool ignoreOil { get; set; }
    }
    public class GetTsiRealTimeDataAckParams {
        public string msg { get; set; }
        public string Hresult { get; set; }
        public string Lresult { get; set; }
        public int step { get; set; }
        public int RPM { get; set; }
        public int CurrentStageTime { get; set; }
        public double lmd { get; set; }
        public double temperature { get; set; }
        public double humidity { get; set; }
        public double amibientPressure { get; set; }
        public double oilTemp { get; set; }
        public bool autoPrintAfterCheck { get; set; }
    }

    public class GetTsiCheckResultDataParams {
        public string ClientID { get; set; }
    }
    public class GetTsiCheckResultAckParams {
        public string msg { get; set; }
        public int step { get; set; }
        public double LowCOLimit { get; set; }
        public double LowHCLimit { get; set; }
        public double HighCOLimit { get; set; }
        public double HighHCLimit { get; set; }
        public double LowCO { get; set; }
        public double LowHC { get; set; }
        public double HighCO { get; set; }
        public double HighHC { get; set; }
        public string HighIdeResult { get; set; }
        public string LowIdeResult { get; set; }
        public double LumdaLimit { get; set; }
        public double Lumda { get; set; }
        public string LumdaResult { get; set; }
        public string TsiCheckeResult { get; set; }
    }

    public class StartVmasCheckParams {
        public string ClientID { get; set; }
        public bool stopCheck { get; set; }
    }
    public class StartVmasCheckAckParams {
        public double VmasCheckSpeedSpan { get; set; }
    }

    public class GetVmasRealTimeDataParams {
        public string ClientID { get; set; }
        public bool stopCheck { get; set; }
    }
    public class GetVmasRealTimeDataAckParams {
        public string msg { get; set; }
        public int step { get; set; }
        public double speed { get; set; }
        public int RPM { get; set; }
        public double speedContinuityOverProofTime { get; set; }
        public bool autoPrintAfterCheck { get; set; }
        public double power { get; set; }
        public double HC { get; set; }
        public double NO { get; set; }
        public double CO { get; set; }
        public double CO2 { get; set; }
        public double O2 { get; set; }
        public double dilutionO2 { get; set; }
        public double environmentalO2 { get; set; }
        public double dilutionRatio { get; set; }
        public double flow { get; set; }
        public int MoveStep { get; set; }
    }

    public class GetVmasCheckResultDataParams {
        public string ClientID { get; set; }
    }
    public class GetVmasCheckResultAckParams {
        public string msg { get; set; }
        public int step { get; set; }
        public string HCLimit { get; set; }
        public string COLimit { get; set; }
        public string NOLimit { get; set; }
        public string HCTest { get; set; }
        public string COTest { get; set; }
        public string NOTest { get; set; }
        public string HCNOTest { get; set; }
        public string HCEvl { get; set; }
        public string COEvl { get; set; }
        public string NOEvl { get; set; }
        public string VmasCheckeResult { get; set; }
    }

    public class StartFalCheckParams {
        public string ClientID { get; set; }
        public bool stopCheck { get; set; }
    }

    public class GetFalRealTimeDataParams {
        public string ClientID { get; set; }
        public bool stopCheck { get; set; }
    }
    public class GetFalRealTimeDataAckParams {
        public string msg { get; set; }
        public int step { get; set; }
        public int CurrentStageTime { get; set; }
        public int RPM { get; set; }
        public double temperature { get; set; }
        public double humidity { get; set; }
        public double amibientPressure { get; set; }
        public double oilTemp { get; set; }
        public double K { get; set; }
        public double K1 { get; set; }
        public double K2 { get; set; }
        public double K3 { get; set; }
        public bool autoPrintAfterCheck { get; set; }
    }

    public class GetFalCheckResultDataParams {
        public string ClientID { get; set; }
    }
    public class GetFalCheckResultAckParams {
        public string msg { get; set; }
        public int step { get; set; }
        public double KLimit { get; set; }
        public double K1 { get; set; }
        public double K2 { get; set; }
        public double K3 { get; set; }
        public double KAvg { get; set; }
        public string FalCheckeResult { get; set; }
    }

    public class StartLdCheckParams {
        public string ClientID { get; set; }
        public bool stopCheck { get; set; }
    }

    public class GetLdRealTimeDataParams {
        public string ClientID { get; set; }
        public bool stopCheck { get; set; }
        public bool canGetMaxRpm { get; set; }
    }
    public class GetLdRealTimeDataAckParams {
        public string msg { get; set; }
        public int step { get; set; }
        public int RPM { get; set; }
        public double Speed { get; set; }
        public double Power { get; set; }
        public double K { get; set; }
        public double NOx { get; set; }
        public double CO2 { get; set; }
        public bool autoPrintAfterCheck { get; set; }
    }

    public class GetLdCheckResultDataParams {
        public string ClientID { get; set; }
    }
    public class GetLdCheckResultAckParams {
        public string msg { get; set; }
        public int step { get; set; }
        public double KLimit { get; set; }
        public double NOx80Limit { get; set; }
        public double RealMaxPowerLimit { get; set; }
        public double VelMaxHP { get; set; }
        public double K100 { get; set; }
        public double K80 { get; set; }
        public double NOx80 { get; set; }
        public double RealMaxPower { get; set; }
        public string LdCheckeResult { get; set; }
    }

    public class DeviceCtrlParams {
        public string CtrlIDStr { get; set; }
        //public string RYLX { get; set; }
        //public double startspeed { get; set; }
        //public double endspeed { get; set; }
        //public double targetspeed { get; set; }
        //public double targetacce { get; set; }
        //public double eddycurrentcount { get; set; }
        //public double targetihp { get; set; }
        //public bool bco2 { get; set; }
        //public double co2 { get; set; }
        //public bool bco { get; set; }
        //public double co { get; set; }
        //public bool bhc { get; set; }
        //public double chc { get; set; }
        //public bool bno { get; set; }
        //public double no { get; set; }
        //public bool bo2 { get; set; }
        //public double o2 { get; set; }
        //public bool bno2 { get; set; }
        //public double no2 { get; set; }
        //public double tempeCali { get; set; }
        //public double humidityCali { get; set; }
        //public double atmosCali { get; set; }
        //public double freq { get; set; }
        //public double targetfreq { get; set; }
        //public double realfreq { get; set; }
    }

    public class DeviceCtrlAckParams {
        public bool Calibrating { get; set; }
        public bool CalibResult { get; set; }
    }

    public class DeviceStatusParams {
        public double speed { get; set; }
        public double turque { get; set; }
        public double power { get; set; }
        public double acce { get; set; }
        public double Diw { get; set; }
        public double RollerDiameter { get; set; }
        public bool beamStatus { get; set; }
        public bool motorStatus { get; set; }
        public bool motorError { get; set; }
        public bool speedTooHigh { get; set; }
        public bool speedSysError { get; set; }
        public bool pauError { get; set; }
        public bool speedSensorError { get; set; }
        public bool forceSensorError { get; set; }
        public bool oneEddyCurrent { get; set; }
        public bool twoeddyCurrent { get; set; }
        public bool threeEddyCurrent { get; set; }
        public bool fourEddyCurrent { get; set; }
        public double HC { get; set; }
        public double NO { get; set; }
        public double CO { get; set; }
        public double CO2 { get; set; }
        public double O2 { get; set; }
        public double Lamda { get; set; }
        public double PEF { get; set; }
        public double RPM { get; set; }
        public double OilTemperature { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
        public double K { get; set; }
        public double N { get; set; }
        public double Ns { get; set; }
        public bool GasWarmUping { get; set; }
        public bool GasLeakChecking { get; set; }
        public bool GasZeroing { get; set; }
        public bool GasCalibrateing { get; set; }
        public bool GasLowFlow { get; set; }
        public bool GasNeedZero { get; set; }
        public bool GasLeakCheckResult { get; set; }
        public bool GasZeroResult { get; set; }
        public bool GasCalibrateResult { get; set; }
        public double StandardFlow { get; set; }
        public double DiluteO2 { get; set; }
        public double NotStandardFlow { get; set; }
        public bool FlowmeterFlowOutRange { get; set; }
        public bool FlowmeterWarmUping { get; set; }
        public bool FlowmeterPressureError { get; set; }
        public bool FlowmeterTemperatureError { get; set; }
        public double WeatherTemperature { get; set; }
        public double WeatherAirpressure { get; set; }
        public double WeatherHumidity { get; set; }
        public double LightSmokeDistancePointsCalibration { get; set; }
        public double LightSmokeK { get; set; }
        public double LightSmokeN { get; set; }
        public double LightSmokeNs { get; set; }
        public bool LightSmokeAdjust { get; set; }
        public bool LightSmokeRealTime { get; set; }
        public double InverterTargetFreq { get; set; }
        public double InverterRealFreq { get; set; }
    }
}
