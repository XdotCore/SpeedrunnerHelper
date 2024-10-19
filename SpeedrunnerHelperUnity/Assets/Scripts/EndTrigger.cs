#if IS_MOD
using MelonLoader;
#endif

namespace SpeedrunnerHelper {

    public class EndTrigger : RevisibleThing {
#if IS_MOD
        protected override MelonPreferences_Entry<bool> VisibleEntry => SpeedrunSettings.VisibleEndTrigger;
        protected override MelonPreferences_Entry<float> OpacityEntry => SpeedrunSettings.EndTriggerOpacity;

        protected override void Awake() => base.Awake();
        protected override void OnDestroy() => base.OnDestroy();
#endif
    }

}
