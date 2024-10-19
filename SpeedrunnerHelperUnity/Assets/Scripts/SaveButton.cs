namespace SpeedrunnerHelper {

    public class SaveButton : CloseButton {
        protected override void Awake() =>
            base.Awake();

        public void SaveOptions() {
#if IS_MOD
            SpeedrunSettings.SpeedrunCat.SaveToFile(false);
#endif
        }

        public void SaveAndCloseOptions() {
            SaveOptions();
            CloseOptions();
        }
    }

}
