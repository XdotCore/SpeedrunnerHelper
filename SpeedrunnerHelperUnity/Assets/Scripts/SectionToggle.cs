namespace SpeedrunnerHelper {

    public class SectionToggle : Clickable {
        public int SectionIndex { get; set; }

        protected override void Awake() =>
            base.Awake();

        public void ShowSection() {
#if IS_MOD
            GameOptions.Instance.ShowOptionsForm(SectionIndex);
#endif
        }

    }

}
