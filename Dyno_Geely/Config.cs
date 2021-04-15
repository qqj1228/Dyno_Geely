using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Dyno_Geely {
    public class ConfigFile<T> where T : new() {
        public string File_xml { get; }
        public T Data { get; set; }
        public string Name {
            get { return Path.GetFileName(File_xml).Split('.')[0]; }
        }

        public ConfigFile(string xml) {
            this.File_xml = xml;
        }
    }

    public class Config {
        private readonly Logger _log;
        public ConfigFile<MainSetting> Main { get; set; }
        public ConfigFile<SelfCheckSetting> SelfCheck { get; set; }

        public Config(Logger logger) {
            _log = logger;
            Main = new ConfigFile<MainSetting>(".\\Configs\\MainSetting.xml");
            SelfCheck = new ConfigFile<SelfCheckSetting>(".\\Configs\\SelfCheck.xml");
            LoadConfig(Main);
            LoadConfig(SelfCheck);
            LoadVMASSpeed(".\\Configs\\VMASSpeed.csv");
        }

        public void LoadConfig<T>(ConfigFile<T> config) where T : new() {
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (FileStream reader = new FileStream(config.File_xml, FileMode.Open)) {
                    config.Data = (T)serializer.Deserialize(reader);
                    reader.Close();
                }
            } catch (Exception ex) {
                _log.TraceError("Using default " + config.Name + " because of failed to load them, reason: " + ex.Message);
                config.Data = new T();
                MessageBox.Show(null,
                    string.Format("{0}配置文件加载失败，{1}\n已使用默认配置启动软件",
                    config.Name, ex.Message),
                    "加载配置文件失败",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public void SaveConfig<T>(ConfigFile<T> config) where T : new() {
            if (config == null || config.Data == null) {
                throw new ArgumentNullException(nameof(config.Data));
            }
            try {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                using (TextWriter writer = new StreamWriter(config.File_xml)) {
                    xmlSerializer.Serialize(writer, config.Data, namespaces);
                    writer.Close();
                }
            } catch (Exception ex) {
                _log.TraceError("Save " + config.Name + " error, reason: " + ex.Message);
            }
        }

        public void LoadVMASSpeed(string speedCSV) {
            try {
                using (StreamReader reader = new StreamReader(speedCSV)) {
                    string line;
                    // 处理第一行表头
                    string[] cols = reader.ReadLine().Trim().Split(',');
                    foreach (string item in cols) {
                        Main.Data.VMASSpeed.Columns.Add(item);
                    }
                    while ((line = reader.ReadLine()) != null) {
                        if (line.Length > 0) {
                            DataRow dr = Main.Data.VMASSpeed.NewRow();
                            string[] rows = line.Trim().Split(',');
                            for (int i = 0; i < cols.Length; i++) {
                                dr[cols[i]] = rows[i];
                            }
                            Main.Data.VMASSpeed.Rows.Add(dr);
                        }
                    }
                }
            } catch (Exception ex) {
                _log.TraceError("Load VMAS speed .csv file error, reason: " + ex.Message);
                MessageBox.Show(null, "加载 VMAS 速度曲线文件出错：" + ex.Message, "加载配置文件失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public static class MainFileVersion {
        public static Version AssemblyVersion {
            get { return ((Assembly.GetEntryAssembly()).GetName()).Version; }
        }

        public static Version AssemblyFileVersion {
            get { return new Version(FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileVersion); }
        }

        public static string AssemblyInformationalVersion {
            get { return FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductVersion; }
        }
    }

}
