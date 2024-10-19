using UnityEngine;
using System.Collections.Generic;


#if IS_MOD
using MelonLoader;
#endif

namespace SpeedrunnerHelper {

    public abstract class Setting : MonoBehaviour {
        public string settingName;

#if IS_MOD
        private static readonly List<Setting> allSettings = [];

        protected virtual void Awake() {
            allSettings.Add(this);
        }

        public abstract void UpdateGUI();
        public abstract void ResetToDefault();
        public abstract void ResetToCurrent();

        public static void UpdateAllSettingsGUI() {
            foreach (Setting setting in allSettings)
                setting.UpdateGUI();
        }

        public static void ResetAllSettingsToDefault() {
            foreach (Setting setting in allSettings)
                setting.ResetToDefault();
        }

        public static void ResetAllSettingsToCurrent() {
            foreach (Setting setting in allSettings)
                setting.ResetToCurrent();
        }
#endif
    }

    public abstract class Setting<T> : Setting {
#if IS_MOD
        private MelonPreferences_Entry<T> Entry => typeof(SpeedrunSettings).GetProperty(settingName)?.GetValue(null) as MelonPreferences_Entry<T>;

        public override void ResetToDefault() {
            if (Entry is not null)
                Entry.EditedValue = Entry.DefaultValue;
        }

        public override void ResetToCurrent() {
            if (Entry is not null)
                Entry.EditedValue = Entry.Value;
        }

        protected T Value {
            get => Entry is null ? default : Entry.Value;
            set {
                if (Entry is not null)
                    Entry.Value = value;
            }
        }

        protected T EditedValue {
            get => Entry is null ? default : Entry.EditedValue;
            set {
                if (Entry is not null)
                    Entry.EditedValue = value;
            }
        }
#endif
    }

}
