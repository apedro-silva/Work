﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18046
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMIZEE.Notification.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.1")]
        public double NewFormsAlarmInterval {
            get {
                return ((double)(this["NewFormsAlarmInterval"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("SF.Expand.Notification.SmtpService, NotificationSmptService")]
        public string NotificationTypeName {
            get {
                return ((string)(this["NotificationTypeName"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("smtp.gmail.com;587;softfinanca.hb@gmail.com;softfinanca.hb@gmail.com;_softfinanca" +
            "_;EnableSsl")]
        public string NotificationParams {
            get {
                return ((string)(this["NotificationParams"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Sistema de Monitorização de Indicadores ZEE")]
        public string NotificationSubject {
            get {
                return ((string)(this["NotificationSubject"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Vimos por esta forma informar que o utilizar {0} com a empresa {1}, tem disponíve" +
            "l o seguinte formulário para preenchimento :{2} - {3} ")]
        public string NotificationMessage {
            get {
                return ((string)(this["NotificationMessage"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.1")]
        public double LateFormsAlarmInterval {
            get {
                return ((double)(this["LateFormsAlarmInterval"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2012-01-01")]
        public global::System.DateTime ReferenceStartDate {
            get {
                return ((global::System.DateTime)(this["ReferenceStartDate"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Vimos por esta forma informar que o utilizar {0} com a empresa {1}, tem em atraso" +
            " o seguinte formulário para preenchimento :{2} - {3} ")]
        public string LateNotificationMessage {
            get {
                return ((string)(this["LateNotificationMessage"]));
            }
        }
    }
}
