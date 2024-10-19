namespace SpeedrunnerHelper {

    public class CloseButton : Clickable {
        protected override void Awake() =>
            base.Awake();

        public void CloseOptions() {
#if IS_MOD
            GameOptions.Instance.ShowOptionsForm(0);
#endif
        }

        public void CloseAndResetOptions() {
#if IS_MOD
            Setting.ResetAllSettingsToCurrent();
            Setting.UpdateAllSettingsGUI();
            CloseOptions();
#endif
        }
    }

}
