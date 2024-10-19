using UnityEngine;
using UnityEngine.UI;

namespace SpeedrunnerHelper {

    public class FloatSetting : Setting<float> {
#if IS_MOD
        private Slider slider;

        protected override void Awake() {
            base.Awake();

            Text percent = transform.Find("Percent").GetComponent<Text>();
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(v => {
                EditedValue = v / 100;
                percent.text = $"{Mathf.RoundToInt(v)}%";
            });
            slider.value = Value * 100;
            percent.text = $"{Mathf.RoundToInt(slider.value)}%";
        }

        public override void UpdateGUI() =>
            slider.value = EditedValue * 100;
#endif
    }

}
