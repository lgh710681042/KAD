namespace 发卡模块.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0"), CompilerGenerated]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = ((Settings) SettingsBase.Synchronized(new Settings()));

        [DebuggerNonUserCode, UserScopedSetting, DefaultSettingValue("\"\"")]
        public string BuildingNo
        {
            get
            {
                return (string) this["BuildingNo"];
            }
            set
            {
                this["BuildingNo"] = value;
            }
        }

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [DefaultSettingValue("0"), UserScopedSetting, DebuggerNonUserCode]
        public int flag
        {
            get
            {
                return (int) this["flag"];
            }
            set
            {
                this["flag"] = value;
            }
        }
    }
}

