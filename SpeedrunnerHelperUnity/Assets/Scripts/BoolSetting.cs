using UnityEngine.UI;

namespace SpeedrunnerHelper {

    public class BoolSetting : Setting<bool> {
#if IS_MOD
        private Toggle toggle;

        protected override void Awake() {
            base.Awake();

            toggle = GetComponent<Toggle>();
            toggle.isOn = Value;
            toggle.onValueChanged.AddListener(v => EditedValue = v);
        }

        public override void UpdateGUI() =>
            toggle.isOn = EditedValue;
#endif
    }

}
