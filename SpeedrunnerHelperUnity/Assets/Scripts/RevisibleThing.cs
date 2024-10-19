using UnityEngine;

#if IS_MOD
using MelonLoader;
#endif

namespace SpeedrunnerHelper {

    public abstract class RevisibleThing : MonoBehaviour {
#if IS_MOD
        protected abstract MelonPreferences_Entry<bool> VisibleEntry { get; }
        protected abstract MelonPreferences_Entry<float> OpacityEntry { get; }

        protected static void SetAlpha(Material mat, float alpha) =>
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, alpha);

        private MeshRenderer renderer;

        protected virtual void Awake() {
            renderer = GetComponent<MeshRenderer>();
            renderer.enabled = VisibleEntry.Value;
            VisibleEntry.OnEntryValueChanged.Subscribe(OnVisibleChanged);

            SetAlpha(renderer.material, OpacityEntry.Value);
            OpacityEntry.OnEntryValueChanged.Subscribe(OnOpacityChanged);
        }

        private void OnVisibleChanged(bool oldVal, bool newVal) =>
            renderer.enabled = newVal;

        private void OnOpacityChanged(float oldVal, float newVal) =>
            SetAlpha(renderer.material, newVal);

        protected virtual void OnDestroy() {
            VisibleEntry.OnEntryValueChanged.Unsubscribe(OnVisibleChanged);
            OpacityEntry.OnEntryValueChanged.Unsubscribe(OnOpacityChanged);
        }
#endif
    }

}
