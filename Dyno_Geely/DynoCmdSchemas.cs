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

    public class SocketLongConnectionParams {
        public string ClientID { get; set; }
    }

    public class SocketLongConnectionAckParams {
        public string threadType { get; set; }
        public int step { get; set; }
        public string msg { get; set; }
        public string NetthreadType { get; set; }
        public int Netstep { get; set; }
        public string NetMsg { get; set; }
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
        public bool dynoPreheat { get; set; }
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
        public bool isQY { get; set; }
        public bool isRetry { get; set; }
    }

    public class GetGasBoxPreheatSelfCheckRealTimeDataParams {
        public string ClientID { get; set; }
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
        public double FlowmeterTargetPressure { get; set; }
        public double FlowmeterTargetTempe { get; set; }
    }
    public class StartFlowmeterCheckAckParams {
        public double FlowmeterO2SpanLow { get; set; }
        public double FlowmeterO2SpanHight { get; set; }
        public double FlowmeterLowFlowSpan { get; set; }
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

    public class StartTachometerCheckParams {
        public string ClientID { get; set; }
    }
    public class StartTachometerCheckAckParams {
        public int QYRpmLow { get; set; }
        public int QYRpmHight { get; set; }
        public int CYRpmLow { get; set; }
        public int CYRpmHight { get; set; }
    }
    public class GetTachometerCheckRealTimeDataParams {
        public string ClientID { get; set; }
    }
    public class GetTachometerCheckRealTimeDataAckParams {
        public double RPM { get; set; }
        public double CYRPM { get; set; }
    }
    public class SaveTachometerCheckParams {
        public string ClientID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double IdleSpeed { get; set; }
        public string CommCheck { get; set; }
        public string Result { get; set; }
    }

    public class GetOilThermometerPreheatSelfCheckRealTimeDataParams {
        public string ClientID { get; set; }
    }
    public class GetOilThermometerPreheatSelfCheckRealTimeDataAckParams {
        public string msg { get; set; }
        public double OilTemperature { get; set; }
        public double CYOilTemperature { get; set; }
    }
    public class SaveOilThermometerPreheatSelfCheckParams {
        public string ClientID { get; set; }
        public string OilTemperatureData { get; set; }
        public string TempDataIn { get; set; }
        public string AbsError { get; set; }
        public string ReError { get; set; }
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

    public class GetPreheatStatusAndTimeAndSurplusTimeParams {
        public string ClientID { get; set; }
    }
    public class GetPreheatStatusAndTimeAndSurplusTimeAckParams {
        public string QYGasBoxRemindInfo { get; set; }
        public DateTime lastCarCheckTime { get; set; }
        public bool dynoPreheatStatus { get; set; }
        public bool gasBoxQYPreheatStatus { get; set; }
        public bool gasBoxCYPreheatStatus { get; set; }
        public bool weatherPreheatStatus { get; set; }
        public bool flowmeterPreheatStatus { get; set; }
        public bool smokePreheatStatus { get; set; }
        public double GasBosQYPreheatCheckSurplusTimeTip { get; set; }
        public double DynoTaxiCheckSurplusTimeTip { get; set; }
        public double GasBoxSinglePointCalibrationCheckTime { get; set; }
        public double FlowmeterFlowSurplusTimeTip { get; set; }
        public double FlowmeterO2SurplusTimeTip { get; set; }
        public double DynoSpeedCheckSurplusTimeTip { get; set; }
        public double DynoTorqueFCheckSurplusTimeTip { get; set; }
        public double DynoDIWCheckSurplusTimeTip { get; set; }
        public double DynoPLHPCheckSurplusTimeTip { get; set; }
        public double GasBoxLowGasCheckSurplusTimeTip { get; set; }
        public double GasBoxLeakCheckSurplusTimeTip { get; set; }
        public double GasBoxNOxConvertRatioCheckSurplusTimeTip { get; set; }
        public double TachometerQYCheckSurplusTimeTip { get; set; }
        public double TachometerCYCheckSurplusTimeTip { get; set; }
        public double OiltempCheckSurplusTimeTip { get; set; }
        public double temperature { get; set; }
        public double humidity { get; set; }
        public double amibientPressure { get; set; }
    }

    public class GetOneFinishCheckVehiclesInfoParams {
        public string ClientID { get; set; }
        public string WJBGBH { get; set; }
    }
    public class GetOneFinishCheckVehiclesInfoAckParams {
        public string oneFinishCheckVehiclesInfo { get; set; }
    }

    public class GetFinishCheckVehiclesInfoParams {
        public string ClientID { get; set; }
        public DateTime queryStartTime { get; set; }
        public DateTime queryEndTime { get; set; }
        public string HPHM { get; set; }
        public string JCFF { get; set; }
        public bool useHPHM { get; set; }
        public bool useJCFF { get; set; }
        public bool useDate { get; set; }
    }
    public class GetFinishCheckVehiclesInfoAckParams {
        public string finishCheckVehiclesInfo { get; set; }
    }

    public class SaveInUseVehicleInfoParams {
        public string ClientID { get; set; }
        public UseVehicle Usevehicle { get; set; }
    }

    public class SaveNewVehicleInfoParams {
        public string ClientID { get; set; }
        public NewVehicle Newvehicle { get; set; }
    }

    public class StartGasboxPrepareParams {
        public string ClientID { get; set; }
        public bool IsStopGasboxPrepare { get; set; }
        public bool Abandon { get; set; }
        public string rylx { get; set; }
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
        public double QYSumCO2COLimit { get; set; }
        public double CYSumCO2COLimit { get; set; }
    }

    public class StartFlowmeterPrepareParams {
        public string ClientID { get; set; }
        public bool IsStopFlowmeterPrepare { get; set; }
        public bool stopCheck { get; set; }
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
        public string FlowmeterPrepareResult { get; set; }
    }

    public class StartSmokePrepareParams {
        public string ClientID { get; set; }
        public bool IsStopSmokePrepare { get; set; }
        public bool stopCheck { get; set; }
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
        public double oilTempCY { get; set; }
        public string oilTitleCY { get; set; }
        public double oilTempOBD { get; set; }
        public double LQYTempOBD { get; set; }
    }

    public class GetTachometerPrepareRealTimeDataParams {
        public string ClientID { get; set; }
        public bool IsStartCheck { get; set; }
        public bool Abandon { get; set; }
        public bool IgnoreRpm { get; set; }
    }
    public class GetTachometerPrepareRealTimeDataAckParams {
        public int QYRPMHigt { get; set; }
        public int QYRPMLow { get; set; }
        public int CYRPMHigt { get; set; }
        public int CYRPMLow { get; set; }
        public int RPM { get; set; }
        public int CYRPM { get; set; }
        public int OBDRPM { get; set; }
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

    public class StartASMCheckParams {
        public string ClientID { get; set; }
        public bool stopCheck { get; set; }
        public bool retryCheck { get; set; }
    }

    public class GetASMRealTimeDataParams {
        public string ClientID { get; set; }
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
        public bool retryCheck { get; set; }
    }

    public class GetTsiRealTimeDataParams {
        public string ClientID { get; set; }
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
        public int TsiCurrentSurplusTime { get; set; }
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
        public bool retryCheck { get; set; }
    }
    public class StartVmasCheckAckParams {
        public double VmasCheckSpeedSpan { get; set; }
    }

    public class GetVmasRealTimeDataParams {
        public string ClientID { get; set; }
        public double VmasContinueDiffTime { get; set; }
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
        public int LDLoadPauCount { get; set; }

    }

    public class GetLdRealTimeDataParams {
        public string ClientID { get; set; }
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
        public CtrlIDStr ctrlid { get; set; }
        public string RYLX = "";
        public double startspeed = 0.0;
        public double endspeed = 0.0;
        public double targetspeed = 0.0;
        public double targetacce = 0.0;
        public int eddycurrentcount = 0;
        public double targetihp = 0.0;
        public bool bco2 { get; set; }
        public double co2 { get; set; }
        public bool bco { get; set; }
        public double co { get; set; }
        public bool bhc { get; set; }
        public int hc { get; set; }
        public bool bno { get; set; }
        public double no { get; set; }
        public bool bo2 { get; set; }
        public double o2 { get; set; }
        public bool bno2 { get; set; }
        public double no2 { get; set; }
        public int calibrationmodel { get; set; }
        public int gastype { get; set; }
        public double oiltemp { get; set; }
        public double tempeCali { get; set; }
        public double humidityCali { get; set; }
        public double atmosCali { get; set; }
        public double freq { get; set; }
        public double targetfreq { get; set; }
        public double realfreq { get; set; }
        public double o2hight { get; set; }
        public double o2low { get; set; }
        public double presshight { get; set; }
        public double presslow { get; set; }
        public double flowhight { get; set; }
        public double flowlow { get; set; }
        public double temphight { get; set; }
        public double templow { get; set; }
        public double press { get; set; }
        public double temp { get; set; }
        public double flow { get; set; }
        public double ZirconiaZero { get; set; }
        public double diluteO2 { get; set; }
        public enum CtrlIDStr {
            CTRLID_DYNO_MOTOR_ON = 41001,
            CTRLID_DYNO_MOTOR_OFF,
            CTRLID_DYNO_BEAM_UP,
            CTRLID_DYNO_BEAM_DOWN,
            CTRLID_DYNO_START_TIMER,
            CTRLID_DYNO_STOP_TIMER,
            CTRLID_DYNO_SENSORZERO,
            CTRLID_DYNO_PAU_ON_SPEED,
            CTRLID_DYNO_PAU_ON_TORQUE,
            CTRLID_DYNO_PAU_ON_POWER,
            CTRLID_DYNO_PAU_ON_ACCE,
            CTRLID_DYNO_PAU_ON_PWM,
            CTRLID_DYNO_PAU_OFF_ASM,
            CTRLID_DYNO_PAU_OFF,
            CTRLID_DYNO_PAU_RESTART,
            CTRLID_DYNO_PAU_PARAMSETTING,
            CTRLID_GASBOX_ZEROGASSTARTIN = 42001,
            CTRLID_GASBOX_ZEROGASENDIN,
            CTRLID_GASBOX_CHECKGASSTARTIN,
            CTRLID_GASBOX_CHECKGASENDIN,
            CTRLID_GASBOX_CALIBGASSTARTIN,
            CTRLID_GASBOX_CALIBGASENDIN,
            CTRLID_GASBOX_STARTZERO,
            CTRLID_GASBOX_LEAKCHECK,
            CTRLID_GASBOX_TESTGASSTARTIN,
            CTRLID_GASBOX_AMBIENTGASSTARTIN,
            CTRLID_GASBOX_PURGEON,
            CTRLID_GASBOX_PURGEOFF,
            CTRLID_GASBOX_STARTCALIBRATING,
            CTRLID_GASBOX_QUIT,
            CTRLID_GASBOX_CALIBRATINGING,
            CTRLID_GASBOX_CALIBRATINGRESULT,
            CTRLID_GASBOX_CALIBRATINGOILTEMP,
            CTRLID_FLOWMETER_ZERO = 43001,
            CTRLID_FLOWMETER_QUIT,
            CTRLID_FLOWMETER_ZirconiaZero,
            CTRLID_FLOWMETER_PressureCalibrationHight,
            CTRLID_FLOWMETER_PressureCalibrationLow,
            CTRLID_FLOWMETER_TemperatureCalibrationHight,
            CTRLID_FLOWMETER_TemperatureCalibrationLow,
            CTRLID_FLOWMETER_O2CalibrationHight,
            CTRLID_FLOWMETER_O2CalibrationLow,
            CTRLID_FLOWMETER_FlowCalibrationHight,
            CTRLID_FLOWMETER_FlowCalibrationLow,
            CTRLID_FLOWMETER_TemperatureCalibration,
            CTRLID_FLOWMETER_PressureCalibration,
            CTRLID_FLOWMETER_FlowCalibration,
            CTRLID_FLOWMETER_DiluteO2Calibration,
            CTRLID_LIGHTSMOKE_ADJUST = 44001,
            CTRLID_LIGHTSMOKE_REALTIME,
            CTRLID_LIGHTSMOKE_QUIT,
            CTRLID_LIGHTSMOKE_SpanPointCalibrationEnd,
            CTRLID_LIGHTSMOKE_SpanPointCalibrationStart,
            CTRLID_SETTARGETFREQ = 45001,
            CTRLID_GETTARGETFREQ,
            CTRLID_GETREALFREQ,
            CTRLID_CALIBTREMPE = 46001,
            CTRLID_CALIBHUMIDITY,
            CTRLID_CALIBATMOS
        }
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
