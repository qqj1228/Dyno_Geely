-- Ԥ��ʱ���
CREATE TABLE ProAsm_G4.dbo.SH_Preheating (
    ID INT IDENTITY PRIMARY KEY NOT NULL,
    LastTime DATETIME NOT NULL DEFAULT(getdate()), -- ��һ���豸�ɹ�Ԥ�ȵ�ʱ��
)
GO
INSERT INTO ProAsm_G4.dbo.SH_Preheating (LastTime) VALUES (getdate())
GO

-- ������Ϣ��
CREATE TABLE ProAsm_G4.dbo.SH_VehicleInfo (
    ID INT IDENTITY PRIMARY KEY NOT NULL,
    VIN VARCHAR(17) NOT NULL,
    VehicleModel VARCHAR(100) NOT NULL, -- ����
    OpenInfoSN VARCHAR(29), -- ��Ϣ������
    TestQTY INT NOT NULL DEFAULT(0), -- ������
    Upload INT NOT NULL DEFAULT(0), -- �����Ƿ����ϴ���1�����ϴ���0��δ�ϴ�
    Skip INT NOT NULL DEFAULT(0), -- �Ƿ��������ϴ���1�����ϴ���0��Ҫ�ϴ�
)
GO

-- �ڲ⹦�����Ѽ쳵����������2���ֶ�
ALTER TABLE ProAsm_G4.dbo.FinishCheckVehicles ADD Upload INT NOT NULL DEFAULT(0) -- ��ʾ�Ƿ����ϴ���1�����ϴ���0��δ�ϴ�
ALTER TABLE ProAsm_G4.dbo.FinishCheckVehicles ADD Skip INT NOT NULL DEFAULT(0) -- ��ʾ�Ƿ���Ҫ�����ϴ���1�������ϴ���0���������ϴ�

-- �����ŷ���Ϣ��
CREATE TABLE ProAsm_G4.dbo.SH_EmissionInfo (
    ID INT IDENTITY PRIMARY KEY NOT NULL,
    VehicleModel VARCHAR(100) NOT NULL UNIQUE, -- ����
    OpenInfoSN VARCHAR(29), -- ��Ϣ������
    VehicleMfr VARCHAR(200), -- ����������ҵ
    EngineModel VARCHAR(100), -- �������ͺ�
    EngineSN VARCHAR(50), -- ���������
    EngineMfr VARCHAR(100), -- ������������ҵ
    EngineVolume FLOAT, -- ����������(L)
    CylinderQTY INT, -- ������
    FuelSupply INT, -- ȼ�͹�����ʽ
    RatedPower FLOAT NOT NULL, -- �����������(kW)
    RatedRPM INT NOT NULL, -- �ת��(r/min)
    EmissionStage INT NOT NULL, -- �����ŷŽ׶�
    Transmission INT, -- ��������ʽ
    CatConverter VARCHAR(100), -- �߻�ת�����ͺ�
    RefMass INT NOT NULL, -- ��׼����(kg)
    MaxMass INT NOT NULL, -- ������������(kg)
    OBDLocation VARCHAR(50), -- OBD�ӿ�λ��
    PostProcessorType VARCHAR(100), -- ��������
    PostProcessorModel VARCHAR(100), -- �����ͺ�
    MotorModel VARCHAR(100), -- �綯���ͺ�
    EnergyStorage VARCHAR(100), -- ����װ���ͺ�
    BatteryCap VARCHAR(50), -- �������
    TestMethod INT NOT NULL DEFAULT(5), -- ��ⷽ��
    Name VARCHAR(10), -- ����Ա����
)
GO

-- -- ���ؼ��ٽ����
-- CREATE TABLE ProAsm_G4.dbo.SH_LugdownResult (
--     ID INT IDENTITY PRIMARY KEY NOT NULL,
--     VIN VARCHAR(17) NOT NULL,
--     Temperature FLOAT NULL, -- �����¶�(��)
--     Humidity FLOAT NULL, -- ����ʪ��(%)
--     Pressure FLOAT NULL, -- ������ѹ(kPa)
--     StartTime DATETIME NOT NULL, -- ��⿪ʼʱ�䣬������Ϊ��XXXX�������ݱ������ʹ��
--     RunningTime INT NOT NULL, -- ����ʱ��(s)
--     RatedRPM FLOAT NULL, -- �ת��(r/min)
--     MaxRPM FLOAT NULL, -- ���ת��(r/min)
--     VelMaxHP FLOAT NULL, -- ���ת�����ٶ�(km/h)
--     RealMaxPowerLimit FLOAT NULL, -- ʵ������ֱ߹�����ֵ(kW)
--     RealMaxPower FLOAT NULL, -- ʵ������ֱ߹��ʲ���ֵ(kW)
--     KLimit FLOAT NULL, -- �̶�ֵ��ֵ
--     K100 FLOAT NULL, -- 100%���̶�ֵ����ֵ
--     K80 FLOAT NULL, -- 80%���̶�ֵ����ֵ
--     NOx80Limit FLOAT NULL, -- 80%��NOx��ֵ(ppm)
--     NOx80 FLOAT NULL, -- 80%��NOx����ֵ(ppm)
--     Result DECIMAL(1) NOT NULL DEFAULT 0, -- �������0��ʧ�ܣ�1���ɹ�
-- )
-- GO

-- -- ���ؼ��ٹ������ݱ�
-- CREATE TABLE ProAsm_G4.dbo.SH_LugdownRealTime (
--     ID INT IDENTITY PRIMARY KEY NOT NULL,
--     VIN VARCHAR(17) NOT NULL,
--     StartTime DATETIME NOT NULL, -- ��⿪ʼʱ�䣬������Ϊ��XXXX����������ʹ��
--     TimeSN FLOAT NOT NULL, -- ʱ������(s)
--     RPM INT NULL, -- ������ת��(r/min)
--     Speed FLOAT NULL, -- ����(Km/h)
--     Power FLOAT NULL, -- ʵʱ����(kw)
--     Torque FLOAT NULL, -- Ť��(Nm)
--     K FLOAT NULL, -- ������ϵ��
--     NOx FLOAT NULL, -- NOx(ppm)
--     CO2 FLOAT NULL, -- CO2(%)
-- )
-- GO

-- -- ��̬���������
-- CREATE TABLE ProAsm_G4.dbo.SH_ASMResult (
--     ID INT IDENTITY PRIMARY KEY NOT NULL,
--     VIN VARCHAR(17) NOT NULL,
--     Temperature FLOAT NULL, -- �����¶�(��)
--     Humidity FLOAT NULL, -- ����ʪ��(%)
--     Pressure FLOAT NULL, -- ������ѹ(kPa)
--     StartTime DATETIME NOT NULL, -- ��⿪ʼʱ�䣬������Ϊ��XXXX�������ݱ������ʹ��
--     RunningTime INT NOT NULL, -- ����ʱ��(s)
--     HC5025Limit FLOAT NULL, -- 5025HC��ֵ(ppm)
--     CO5025Limit FLOAT NULL, -- 5025CO��ֵ(%)
--     NO5025Limit FLOAT NULL, -- 5025NO��ֵ(ppm)
--     HC5025 FLOAT NULL, -- 5025HC(ppm)
--     CO5025 FLOAT NULL, -- 5025CO(%)
--     NO5025 FLOAT NULL, -- 5025NO(ppm)
--     HC5025Evl DECIMAL(1) NOT NULL DEFAULT 0, -- 5025HC�ж������0��ʧ�ܣ�1���ɹ�
--     CO5025Evl DECIMAL(1) NOT NULL DEFAULT 0, -- 5025CO�ж������0��ʧ�ܣ�1���ɹ�
--     NO5025Evl DECIMAL(1) NOT NULL DEFAULT 0, -- 5025NO�ж������0��ʧ�ܣ�1���ɹ�
--     Power5025 FLOAT NULL, -- 5025���ص��ܹ���
--     HC2540Limit FLOAT NULL, -- 2540HC��ֵ(ppm)
--     CO2540Limit FLOAT NULL, -- 2540CO��ֵ(%)
--     NO2540Limit FLOAT NULL, -- 2540NO��ֵ(ppm)
--     HC2540 FLOAT NULL, -- 2540HC(ppm)
--     CO2540 FLOAT NULL, -- 2540CO(%)
--     NO2540 FLOAT NULL, -- 2540NO(ppm)
--     HC2540Evl DECIMAL(1) NULL, -- 2540HC�ж������0��ʧ�ܣ�1���ɹ���NULL���޽��
--     CO2540Evl DECIMAL(1) NULL, -- 2540CO�ж������0��ʧ�ܣ�1���ɹ���NULL���޽��
--     NO2540Evl DECIMAL(1) NULL, -- 2540NO�ж������0��ʧ�ܣ�1���ɹ���NULL���޽��
--     Power2540 FLOAT NULL, -- 2540���ص��ܹ���
--     Result DECIMAL(1) NOT NULL DEFAULT 0, -- �������0��ʧ�ܣ�1���ɹ�
-- )
-- GO

-- -- ��̬�����������ݱ�
-- CREATE TABLE ProAsm_G4.dbo.SH_ASMRealTime (
--     ID INT IDENTITY PRIMARY KEY NOT NULL,
--     VIN VARCHAR(17) NOT NULL,
--     StartTime DATETIME NOT NULL, -- ��⿪ʼʱ�䣬������Ϊ��XXXX����������ʹ��
--     TimeSN FLOAT NOT NULL, -- ʱ������(s)
--     Step INT NULL, -- ��ǰ�����׶�
--     TestTime FLOAT NULL, -- ���β��Լ�ʱ(s)
--     WorkingTime FLOAT NULL, -- ��������ʱ(s)
--     RPM INT NULL, -- ������ת��(r/min)
--     Speed FLOAT NULL, -- ����(Km/h)
--     Power FLOAT NULL, -- ʵʱ����(kw)
--     HC FLOAT NULL, -- ʵ��HC(ppm)
--     CO FLOAT NULL, -- ʵ��CO(%)
--     NO FLOAT NULL, -- ʵ��NO(ppm)
--     CO2 FLOAT NULL, -- ʵ��CO2(%)
--     O2 FLOAT NULL, -- ʵ��O2(%)
--     lambda FLOAT NULL, -- ��������ϵ����
--     KH FLOAT NULL, -- NO��ʪ������ϵ��
--     DF FLOAT NULL, -- ϡ������ϵ��
--     HCNor FLOAT NULL, -- ������HC(ppm)
--     CONor FLOAT NULL, -- ������CO(%)
--     NONor FLOAT NULL, -- ������NO(ppm)
-- )
-- GO

-- -- ���ɼ��ٲ�͸������
-- CREATE TABLE ProAsm_G4.dbo.SH_FALResult (
--     ID INT IDENTITY PRIMARY KEY NOT NULL,
--     VIN VARCHAR(17) NOT NULL,
--     Temperature FLOAT NULL, -- �����¶�(��)
--     Humidity FLOAT NULL, -- ����ʪ��(%)
--     Pressure FLOAT NULL, -- ������ѹ(kPa)
--     StartTime DATETIME NOT NULL, -- ��⿪ʼʱ�䣬������Ϊ��XXXX�������ݱ������ʹ��
--     RunningTime INT NOT NULL, -- ����ʱ��(s)
--     RatedRPM FLOAT NULL, -- �ת��(r/min)
--     MaxRPM FLOAT NULL, -- ʵ�����ת��(r/min)
--     KLimit FLOAT NULL, -- ������ϵ����ֵ(m^-1)
--     KAvg FLOAT NULL, -- ������ϵ��ƽ��ֵ(m^-1)
--     K1 FLOAT NULL, -- ������ϵ��1(m^-1)
--     K2 FLOAT NULL, -- ������ϵ��2(m^-1)
--     K3 FLOAT NULL, -- ������ϵ��3(m^-1)
--     Result DECIMAL(1) NOT NULL DEFAULT 0, -- �������0��ʧ�ܣ�1���ɹ�
-- )
-- GO

-- -- ���ɼ��ٲ�͸��������ݱ�
-- CREATE TABLE ProAsm_G4.dbo.SH_FALRealTime (
--     ID INT IDENTITY PRIMARY KEY NOT NULL,
--     VIN VARCHAR(17) NOT NULL,
--     StartTime DATETIME NOT NULL, -- ��⿪ʼʱ�䣬������Ϊ��XXXX����������ʹ��
--     TimeSN FLOAT NOT NULL, -- ʱ������(s)
--     Step INT NULL, -- ��ǰ�����׶�
--     CurrentStageTime INT NULL, -- ��ǰ�׶μ�ʱ
--     RPM INT NULL, -- ������ת��(r/min)
--     K FLOAT NULL, -- ������ϵ��ʵ��ֵ(m^-1)
-- )
-- GO

-- -- ˫���ٽ����
-- CREATE TABLE ProAsm_G4.dbo.SH_TSIResult (
--     ID INT IDENTITY PRIMARY KEY NOT NULL,
--     VIN VARCHAR(17) NOT NULL,
--     Temperature FLOAT NULL, -- �����¶�(��)
--     Humidity FLOAT NULL, -- ����ʪ��(%)
--     Pressure FLOAT NULL, -- ������ѹ(kPa)
--     StartTime DATETIME NOT NULL, -- ��⿪ʼʱ�䣬������Ϊ��XXXX�������ݱ������ʹ��
--     RunningTime INT NOT NULL, -- ����ʱ��(s)
--     HighCOLimit FLOAT NULL, -- �ߵ���CO��ֵ(%)
--     HighHCLimit FLOAT NULL, -- �ߵ���HC��ֵ(ppm)
--     HighCO FLOAT NULL, -- �ߵ���CO(%)
--     HighHC FLOAT NULL, -- �ߵ���HC(%ppm)
--     HighIdleResult DECIMAL(1) NOT NULL DEFAULT 0, -- �ߵ����ж������0��ʧ�ܣ�1���ɹ�
--     LowCOLimit FLOAT NULL, -- �͵���CO��ֵ(%)
--     LowHCLimit FLOAT NULL, -- �͵���HC��ֵ(ppm)
--     LowCO FLOAT NULL, -- �͵���CO(%)
--     LowHC FLOAT NULL, -- �͵���HC(ppm)
--     LowIdleResult DECIMAL(1) NOT NULL DEFAULT 0, -- �͵����ж������0��ʧ�ܣ�1���ɹ�
--     LambdaLimit FLOAT NULL, -- ��������ϵ������ֵ
--     Lambda FLOAT NULL, -- ��������ϵ����
--     LambdaResult DECIMAL(1) NOT NULL DEFAULT 0, -- ��������ϵ�����ж������0��ʧ�ܣ�1���ɹ�
--     Result DECIMAL(1) NOT NULL DEFAULT 0, -- �������0��ʧ�ܣ�1���ɹ�
-- )
-- GO

-- -- ˫���ٹ������ݱ�
-- CREATE TABLE ProAsm_G4.dbo.SH_TSIRealTime (
--     ID INT IDENTITY PRIMARY KEY NOT NULL,
--     VIN VARCHAR(17) NOT NULL,
--     StartTime DATETIME NOT NULL, -- ��⿪ʼʱ�䣬������Ϊ��XXXX����������ʹ��
--     TimeSN FLOAT NOT NULL, -- ʱ������(s)
--     Step INT NULL, -- ��ǰ�����׶�
--     RPM INT NULL, -- ������ת��(r/min)
--     CurrentStageTime INT NULL, -- ��ǰ�׶μ�ʱ
--     HC FLOAT NULL, -- ʵ��HC(ppm)
--     CO FLOAT NULL, -- ʵ��CO(%)
--     CO2 FLOAT NULL, -- ʵ��CO2(%)
--     O2 FLOAT NULL, -- ʵ��O2(%)
--     Lambda FLOAT NULL, -- ��������ϵ����
--     OilTemp FLOAT NULL, -- ����(��)
--     HResult DECIMAL(1) NOT NULL DEFAULT 0, -- �������0��ʧ�ܣ�1���ɹ�
--     LResult DECIMAL(1) NOT NULL DEFAULT 0, -- �������0��ʧ�ܣ�1���ɹ�
-- )
-- GO

-- -- ����˲̬���������
-- CREATE TABLE ProAsm_G4.dbo.SH_VMASResult (
--     ID INT IDENTITY PRIMARY KEY NOT NULL,
--     VIN VARCHAR(17) NOT NULL,
--     Temperature FLOAT NULL, -- �����¶�(��)
--     Humidity FLOAT NULL, -- ����ʪ��(%)
--     Pressure FLOAT NULL, -- ������ѹ(kPa)
--     StartTime DATETIME NOT NULL, -- ��⿪ʼʱ�䣬������Ϊ��XXXX�������ݱ������ʹ��
--     RunningTime INT NOT NULL, -- ����ʱ��(s)
--     HCLimit FLOAT NULL, -- HC(g/km)��ֵ
--     COLimit FLOAT NULL, -- CO(g/km)��ֵ
--     NOLimit FLOAT NULL, -- NO(g/km)��ֵ
--     HC FLOAT NULL, -- HC����ֵ
--     CO FLOAT NULL, -- CO����ֵ
--     NO FLOAT NULL, -- NO����ֵ
--     HCNO FLOAT NULL, -- HC+NO����ֵ
--     Distance FLOAT NULL, -- ���Թ���ʵ����ʻ����(km)
--     HCEvl DECIMAL(1) NOT NULL DEFAULT 0, -- HC�ж������0��ʧ�ܣ�1���ɹ�
--     COEvl DECIMAL(1) NOT NULL DEFAULT 0, -- CO�ж������0��ʧ�ܣ�1���ɹ�
--     NOEvl DECIMAL(1) NOT NULL DEFAULT 0, -- NO�ж������0��ʧ�ܣ�1���ɹ�
--     Result DECIMAL(1) NOT NULL DEFAULT 0, -- �������0��ʧ�ܣ�1���ɹ�
-- )
-- GO

-- -- ����˲̬�����������ݱ�
-- CREATE TABLE ProAsm_G4.dbo.SH_VMASRealTime (
--     ID INT IDENTITY PRIMARY KEY NOT NULL,
--     VIN VARCHAR(17) NOT NULL,
--     StartTime DATETIME NOT NULL, -- ��⿪ʼʱ�䣬������Ϊ��XXXX����������ʹ��
--     TimeSN FLOAT NOT NULL, -- ʱ������(s)
--     Speed FLOAT NULL, -- ʵʱ����(Km/h)
--     RPM INT NULL, -- ������ת��(r/min)
--     SpeedOverTime INT NULL, -- �ٶ���������ʱ��(s)
--     Power FLOAT NULL, -- ����(kw)
--     HC FLOAT NULL, -- ʵ��HC(ppm)
--     HCNor FLOAT NULL, -- ����HC(ppm)
--     NO FLOAT NULL, -- ʵ��NO(ppm)
--     NONor FLOAT NULL, -- ����NO(ppm)
--     CO FLOAT NULL, -- ʵ��CO(%)
--     CONor FLOAT NULL, -- ����CO(%)
--     CO2 FLOAT NULL, -- ʵ��CO2(%)
--     O2 FLOAT NULL, -- ������O2(%)
--     DilutionO2 FLOAT NULL, -- ϡ��O2(%)
--     EnvO2 FLOAT NULL, -- ����O2(%)
--     DilutionRatio FLOAT NULL, -- ϡ�ͱ�
--     Flow FLOAT NULL, -- ����(L/s)
--     DF FLOAT NULL, -- ϡ������ϵ��
--     KH FLOAT NULL, -- NO��ʪ������ϵ��
--     lambda FLOAT NULL, -- ��������ϵ����
-- )
-- GO
