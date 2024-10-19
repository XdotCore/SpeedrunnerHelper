namespace SpeedrunnerHelper {

    public class ResetButton : Clickable {
        protected override void Awake() =>
            base.Awake();

        public void ResetOptions() {
#if IS_MOD
            Setting.ResetAllSettingsToDefault();
            Setting.UpdateAllSettingsGUI();
#endif
        }
    }

}
