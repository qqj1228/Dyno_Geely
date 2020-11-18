CREATE DATABASE DYNO_GEELY DEFAULT CHARACTER SET = utf8;

# 预热时间表
CREATE TABLE `dyno_geely`.`Preheating` (
    `ID` INT NOT NULL DEFAULT 1,
    `LastTime` DATETIME NOT NULL DEFAULT now(), # 上一次设备成功预热的时间
    PRIMARY KEY (`ID`)
) DEFAULT CHARACTER SET = utf8;

INSERT INTO `dyno_geely`.`preheating` (`LastTime`) VALUES (now());

# 车辆信息表
CREATE TABLE `dyno_geely`.`VehicleInfo` (
    `ID` INT NOT NULL AUTO_INCREMENT,
    `VIN` VARCHAR(17) NOT NULL,
    `VehicleModel` VARCHAR(100) NOT NULL, # 车型
    `OpenInfoSN` VARCHAR(29) NOT NULL, # 信息公开号
    `TestQTY` INT NOT NULL DEFAULT 0, # 检测次数
    PRIMARY KEY (`ID`)
) DEFAULT CHARACTER SET = utf8;

# 车型排放信息表
CREATE TABLE `dyno_geely`.`EmissionInfo` (
    `ID` INT NOT NULL AUTO_INCREMENT,
    `VehicleModel` VARCHAR(100) NOT NULL, # 车型
    `OpenInfoSN` VARCHAR(29) NOT NULL, # 信息公开号
    `VehicleMfr` VARCHAR(200), # 车辆生产企业
    `EngineModel` VARCHAR(100), # 发动机型号
    `EngineSN` VARCHAR(50), # 发动机编号
    `EngineMfr` VARCHAR(100), # 发动机生产企业
    `EngineVolume` VARCHAR(50), # 发动机排量(L)
    `CylinderQTY` VARCHAR(50), # 气缸数
    `FuelSupply` VARCHAR(50), # 燃油供给方式
    `RatedPower` VARCHAR(50), # 发动机额定功率(kW)
    `RatedRPM` NUMERIC(5) NOT NULL, # 额定转速(r/min)
    `EmissionStage` VARCHAR(50), # 车辆排放阶段
    `Transmission` VARCHAR(50), # 变速器形式
    `CatConverter` VARCHAR(100), # 催化转化器型号
    `RefMass` VARCHAR(50), # 基准质量(kg)
    `MaxMass` VARCHAR(50), # 最大设计总质量(kg)
    `OBDLocation` VARCHAR(50), # OBD接口位置
    `PostProcessing` VARCHAR(100), # 后处理类型
    `PostProcessor` VARCHAR(100), # 后处理型号
    `MotorModel` VARCHAR(100), # 电动机型号
    `EnergyStorage` VARCHAR(100), # 储能装置型号
    `BatteryCap` VARCHAR(50), # 电池容量
    `TestMethod` NUMERIC(1) NOT NULL, # 检测方法
    `Name` VARCHAR(10), # 检验员名字
    PRIMARY KEY (`ID`)
) DEFAULT CHARACTER SET = utf8;

# 加载减速结果表
CREATE TABLE `dyno_geely`.`LugdownResult` (
    `ID` INT NOT NULL AUTO_INCREMENT,
    `VIN` VARCHAR(17) NOT NULL,
    `Temperature` FLOAT NULL, # 环境温度(℃)
    `Humidity` FLOAT NULL, # 环境湿度(%)
    `Pressure` FLOAT NULL, # 环境气压(kPa)
    `StartTime` DATETIME NOT NULL, # 检测开始时间，可以作为“XXXX过程数据表”的外键使用
    `RunningTime` INT NOT NULL, # 持续时间(s)
    `RatedRPM` FLOAT NULL, # 额定转速(r/min)
    `MaxRPM` FLOAT NULL, # 最大转速(r/min)
    `VelMaxHP` FLOAT NULL, # 最大转股线速度(km/h)
    `RealMaxPowerLimit` FLOAT NULL, # 实测最大轮边功率限值(kW)
    `RealMaxPower` FLOAT NULL, # 实测最大轮边功率测量值(kW)
    `KLimit` FLOAT NULL, # 烟度值限值
    `K100` FLOAT NULL, # 100%点烟度值测量值
    `K80` FLOAT NULL, # 80%点烟度值测量值
    `NOx80Limit` FLOAT NULL, # 80%点NOx限值(ppm)
    `NOx80` FLOAT NULL, # 80%点NOx测量值(ppm)
    `Result` DECIMAL(1) NOT NULL DEFAULT 0, # 检测结果，0：失败；1：成功
    PRIMARY KEY (`ID`)
) DEFAULT CHARACTER SET = utf8;

# 加载减速过程数据表
CREATE TABLE `dyno_geely`.`LugdownRealTime` (
    `ID` INT NOT NULL AUTO_INCREMENT,
    `VIN` VARCHAR(17) NOT NULL,
    `StartTime` DATETIME NOT NULL, # 检测开始时间，可以作为“XXXX结果表”的外键使用
    `TimeSN` FLOAT NOT NULL, # 时间序列(s)
    `RPM` INT NULL, # 发动机转速(r/min)
    `Speed` FLOAT NULL, # 车速(Km/h)
    `Power` FLOAT NULL, # 实时功率(kw)
    `Torque` FLOAT NULL, # 扭矩(Nm)
    `K` FLOAT NULL, # 光吸收系数
    `NOx` FLOAT NULL, # NOx(ppm)
    `CO2` FLOAT NULL, # CO2(%)
    PRIMARY KEY (`ID`)
) DEFAULT CHARACTER SET = utf8;

# 稳态工况结果表
CREATE TABLE `dyno_geely`.`ASMResult` (
    `ID` INT NOT NULL AUTO_INCREMENT,
    `VIN` VARCHAR(17) NOT NULL,
    `Temperature` FLOAT NULL, # 环境温度(℃)
    `Humidity` FLOAT NULL, # 环境湿度(%)
    `Pressure` FLOAT NULL, # 环境气压(kPa)
    `StartTime` DATETIME NOT NULL, # 检测开始时间，可以作为“XXXX过程数据表”的外键使用
    `RunningTime` INT NOT NULL, # 持续时间(s)
    `HC5025Limit` FLOAT NULL, # 5025HC限值(ppm)
    `CO5025Limit` FLOAT NULL, # 5025CO限值(%)
    `NO5025Limit` FLOAT NULL, # 5025NO限值(ppm)
    `HC5025` FLOAT NULL, # 5025HC(ppm)
    `CO5025` FLOAT NULL, # 5025CO(%)
    `NO5025` FLOAT NULL, # 5025NO(ppm)
    `HC5025Evl` DECIMAL(1) NOT NULL DEFAULT 0, # 5025HC判定结果，0：失败；1：成功
    `CO5025Evl` DECIMAL(1) NOT NULL DEFAULT 0, # 5025CO判定结果，0：失败；1：成功
    `NO5025Evl` DECIMAL(1) NOT NULL DEFAULT 0, # 5025NO判定结果，0：失败；1：成功
    `Power5025` FLOAT NULL, # 5025加载的总功率
    `HC2540Limit` FLOAT NULL, # 2540HC限值(ppm)
    `CO2540Limit` FLOAT NULL, # 2540CO限值(%)
    `NO2540Limit` FLOAT NULL, # 2540NO限值(ppm)
    `HC2540` FLOAT NULL, # 2540HC(ppm)
    `CO2540` FLOAT NULL, # 2540CO(%)
    `NO2540` FLOAT NULL, # 2540NO(ppm)
    `HC2540Evl` DECIMAL(1) NULL, # 2540HC判定结果，0：失败；1：成功；NULL：无结果
    `CO2540Evl` DECIMAL(1) NULL, # 2540CO判定结果，0：失败；1：成功；NULL：无结果
    `NO2540Evl` DECIMAL(1) NULL, # 2540NO判定结果，0：失败；1：成功；NULL：无结果
    `Power2540` FLOAT NULL, # 2540加载的总功率
    `Result` DECIMAL(1) NOT NULL DEFAULT 0, # 检测结果，0：失败；1：成功
    PRIMARY KEY (`ID`)
) DEFAULT CHARACTER SET = utf8;

# 稳态工况过程数据表
CREATE TABLE `dyno_geely`.`ASMRealTime` (
    `ID` INT NOT NULL AUTO_INCREMENT,
    `VIN` VARCHAR(17) NOT NULL,
    `StartTime` DATETIME NOT NULL, # 检测开始时间，可以作为“XXXX结果表”的外键使用
    `TimeSN` FLOAT NOT NULL, # 时间序列(s)
    `Step` INT NULL, # 当前工况阶段
    `TestTime` FLOAT NULL, # 本次测试计时(s)
    `WorkingTime` FLOAT NULL, # 单工况计时(s)
    `RPM` INT NULL, # 发动机转速(r/min)
    `Speed` FLOAT NULL, # 车速(Km/h)
    `Power` FLOAT NULL, # 实时功率(kw)
    `HC` FLOAT NULL, # 实测HC(ppm)
    `CO` FLOAT NULL, # 实测CO(%)
    `NO` FLOAT NULL, # 实测NO(ppm)
    `CO2` FLOAT NULL, # 实测CO2(%)
    `O2` FLOAT NULL, # 实测O2(%)
    `lambda` FLOAT NULL, # 过量空气系数λ
    `KH` FLOAT NULL, # NO的湿度修正系数
    `DF` FLOAT NULL, # 稀释修正系数
    `HCNor` FLOAT NULL, # 修正后HC(ppm)
    `CONor` FLOAT NULL, # 修正后CO(%)
    `NONor` FLOAT NULL, # 修正后NO(ppm)
    PRIMARY KEY (`ID`)
) DEFAULT CHARACTER SET = utf8;

# 自由加速不透光结果表
CREATE TABLE `dyno_geely`.`FALResult` (
    `ID` INT NOT NULL AUTO_INCREMENT,
    `VIN` VARCHAR(17) NOT NULL,
    `Temperature` FLOAT NULL, # 环境温度(℃)
    `Humidity` FLOAT NULL, # 环境湿度(%)
    `Pressure` FLOAT NULL, # 环境气压(kPa)
    `StartTime` DATETIME NOT NULL, # 检测开始时间，可以作为“XXXX过程数据表”的外键使用
    `RunningTime` INT NOT NULL, # 持续时间(s)
    `RatedRPM` FLOAT NULL, # 额定转速(r/min)
    `MaxRPM` FLOAT NULL, # 实测最大转速(r/min)
    `KLimit` FLOAT NULL, # 光吸收系数限值(m^-1)
    `KAvg` FLOAT NULL, # 光吸收系数平均值(m^-1)
    `K1` FLOAT NULL, # 光吸收系数1(m^-1)
    `K2` FLOAT NULL, # 光吸收系数2(m^-1)
    `K3` FLOAT NULL, # 光吸收系数3(m^-1)
    `Result` DECIMAL(1) NOT NULL DEFAULT 0, # 检测结果，0：失败；1：成功
    PRIMARY KEY (`ID`)
) DEFAULT CHARACTER SET = utf8;

# 自由加速不透光过程数据表
CREATE TABLE `dyno_geely`.`FALRealTime` (
    `ID` INT NOT NULL AUTO_INCREMENT,
    `VIN` VARCHAR(17) NOT NULL,
    `StartTime` DATETIME NOT NULL, # 检测开始时间，可以作为“XXXX结果表”的外键使用
    `TimeSN` FLOAT NOT NULL, # 时间序列(s)
    `Step` INT NULL, # 当前工况阶段
    `CurrentStageTime` INT NULL, # 当前阶段计时
    `RPM` INT NULL, # 发动机转速(r/min)
    `K` FLOAT NULL, # 光吸收系数实测值(m^-1)
    PRIMARY KEY (`ID`)
) DEFAULT CHARACTER SET = utf8;

# 双怠速结果表
CREATE TABLE `dyno_geely`.`TSIResult` (
    `ID` INT NOT NULL AUTO_INCREMENT,
    `VIN` VARCHAR(17) NOT NULL,
    `Temperature` FLOAT NULL, # 环境温度(℃)
    `Humidity` FLOAT NULL, # 环境湿度(%)
    `Pressure` FLOAT NULL, # 环境气压(kPa)
    `StartTime` DATETIME NOT NULL, # 检测开始时间，可以作为“XXXX过程数据表”的外键使用
    `RunningTime` INT NOT NULL, # 持续时间(s)
    `HighCOLimit` FLOAT NULL, # 高怠速CO限值(%)
    `HighHCLimit` FLOAT NULL, # 高怠速HC限值(ppm)
    `HighCO` FLOAT NULL, # 高怠速CO(%)
    `HighHC` FLOAT NULL, # 高怠速HC(%ppm)
    `HighIdleResult` DECIMAL(1) NOT NULL DEFAULT 0, # 高怠速判定结果，0：失败；1：成功
    `LowCOLimit` FLOAT NULL, # 低怠速CO限值(%)
    `LowHCLimit` FLOAT NULL, # 低怠速HC限值(ppm)
    `LowCO` FLOAT NULL, # 低怠速CO(%)
    `LowHC` FLOAT NULL, # 低怠速HC(ppm)
    `LowIdleResult` DECIMAL(1) NOT NULL DEFAULT 0, # 低怠速判定结果，0：失败；1：成功
    `LambdaLimit` FLOAT NULL, # 过量空气系数λ限值
    `Lambda` FLOAT NULL, # 过量空气系数λ
    `LambdaResult` DECIMAL(1) NOT NULL DEFAULT 0, # 过量空气系数λ判定结果，0：失败；1：成功
    `Result` DECIMAL(1) NOT NULL DEFAULT 0, # 检测结果，0：失败；1：成功
    PRIMARY KEY (`ID`)
) DEFAULT CHARACTER SET = utf8;

# 双怠速过程数据表
CREATE TABLE `dyno_geely`.`TSIRealTime` (
    `ID` INT NOT NULL AUTO_INCREMENT,
    `VIN` VARCHAR(17) NOT NULL,
    `StartTime` DATETIME NOT NULL, # 检测开始时间，可以作为“XXXX结果表”的外键使用
    `TimeSN` FLOAT NOT NULL, # 时间序列(s)
    `Step` INT NULL, # 当前工况阶段
    `RPM` INT NULL, # 发动机转速(r/min)
    `CurrentStageTime` INT NULL, # 当前阶段计时
    `HC` FLOAT NULL, # 实测HC(ppm)
    `CO` FLOAT NULL, # 实测CO(%)
    `CO2` FLOAT NULL, # 实测CO2(%)
    `O2` FLOAT NULL, # 实测O2(%)
    `Lambda` FLOAT NULL, # 过量空气系数λ
    `OilTemp` FLOAT NULL, # 油温(℃)
    `HResult` DECIMAL(1) NOT NULL DEFAULT 0, # 检测结果，0：失败；1：成功
    `LResult` DECIMAL(1) NOT NULL DEFAULT 0, # 检测结果，0：失败；1：成功
    PRIMARY KEY (`ID`)
) DEFAULT CHARACTER SET = utf8;

# 简易瞬态工况结果表
CREATE TABLE `dyno_geely`.`VMASResult` (
    `ID` INT NOT NULL AUTO_INCREMENT,
    `VIN` VARCHAR(17) NOT NULL,
    `Temperature` FLOAT NULL, # 环境温度(℃)
    `Humidity` FLOAT NULL, # 环境湿度(%)
    `Pressure` FLOAT NULL, # 环境气压(kPa)
    `StartTime` DATETIME NOT NULL, # 检测开始时间，可以作为“XXXX过程数据表”的外键使用
    `RunningTime` INT NOT NULL, # 持续时间(s)
    `HCLimit` FLOAT NULL, # HC(g/km)限值
    `COLimit` FLOAT NULL, # CO(g/km)限值
    `NOLimit` FLOAT NULL, # NO(g/km)限值
    `HC` FLOAT NULL, # HC测量值
    `CO` FLOAT NULL, # CO测量值
    `NO` FLOAT NULL, # NO测量值
    `HCNO` FLOAT NULL, # HC+NO测量值
    `Distance` FLOAT NULL, # 测试过程实际行驶距离(km)
    `HCEvl` DECIMAL(1) NOT NULL DEFAULT 0, # HC判定结果，0：失败；1：成功
    `COEvl` DECIMAL(1) NOT NULL DEFAULT 0, # CO判定结果，0：失败；1：成功
    `NOEvl` DECIMAL(1) NOT NULL DEFAULT 0, # NO判定结果，0：失败；1：成功
    `Result` DECIMAL(1) NOT NULL DEFAULT 0, # 检测结果，0：失败；1：成功
    PRIMARY KEY (`ID`)
) DEFAULT CHARACTER SET = utf8;

# 简易瞬态工况过程数据表
CREATE TABLE `dyno_geely`.`VMASRealTime` (
    `ID` INT NOT NULL AUTO_INCREMENT,
    `VIN` VARCHAR(17) NOT NULL,
    `StartTime` DATETIME NOT NULL, # 检测开始时间，可以作为“XXXX结果表”的外键使用
    `TimeSN` FLOAT NOT NULL, # 时间序列(s)
    `Speed` FLOAT NULL, # 实时车速(Km/h)
    `RPM` INT NULL, # 发动机转速(r/min)
    `SpeedOverTime` INT NULL, # 速度连续超差时间(s)
    `Power` FLOAT NULL, # 功率(kw)
    `HC` FLOAT NULL, # 实测HC(ppm)
    `HCNor` FLOAT NULL, # 修正HC(ppm)
    `NO` FLOAT NULL, # 实测NO(ppm)
    `NONor` FLOAT NULL, # 修正NO(ppm)
    `CO` FLOAT NULL, # 实测CO(%)
    `CONor` FLOAT NULL, # 修正CO(%)
    `CO2` FLOAT NULL, # 实测CO2(%)
    `O2` FLOAT NULL, # 分析仪O2(%)
    `DilutionO2` FLOAT NULL, # 稀释O2(%)
    `EnvO2` FLOAT NULL, # 环境O2(%)
    `DilutionRatio` FLOAT NULL, # 稀释比
    `Flow` FLOAT NULL, # 流量(L/s)
    `DF` FLOAT NULL, # 稀释修正系数
    `KH` FLOAT NULL, # NO的湿度修正系数
    `lambda` FLOAT NULL, # 过量空气系数λ
    PRIMARY KEY (`ID`)
) DEFAULT CHARACTER SET = utf8;
