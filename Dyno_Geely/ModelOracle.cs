using Dyno_Geely;
using LibBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Dyno_Geely {
    public class ModelOracle : ModelBase {
        private readonly OracleSetting _setting;
        public string IDValue { get; set; }

        public ModelOracle(OracleSetting oracleMESSetting, Logger log) {
            _setting = oracleMESSetting;
            ModelParameter dbParam = new ModelParameter {
                DataBaseType = DataBaseType.Oracle,
                UserName = _setting.UserID,
                PassWord = _setting.PassWord,
                Host = _setting.Host,
                Port = _setting.Port,
                DBorService = _setting.ServiceName
            };
            InitDataBase(dbParam, log);
            IDValue = "SEQ_EM_WQPF_ID.NEXTVAL";
        }

    }
}
