using UnityEngine;


#if IS_MOD
using MelonLoader;
using HarmonyLib;
#endif

namespace SpeedrunnerHelper {

#if IS_MOD
    [HarmonyPatch]
#endif
    public class Checkpoint : RevisibleThing {
#if IS_MOD
        protected override MelonPreferences_Entry<bool> VisibleEntry => SpeedrunSettings.VisibleCheckpoints;
        protected override MelonPreferences_Entry<float> OpacityEntry => SpeedrunSettings.CheckpointOpacity;

        protected override void Awake() => base.Awake();
        protected override void OnDestroy() => base.OnDestroy();

        public static Material Material { get; set; }
        public static Material SelectedMaterial { get; set; }
        private static Checkpoint selected = null;

        [HarmonyPatch(typeof(CheckpointStore), "SetNewCheckpoint")]
        [HarmonyPostfix]
        private static void SwapSelected(Transform t) {
            Checkpoint newSelected = t.GetComponentInChildren<Checkpoint>();
            if (newSelected is null || ReferenceEquals(newSelected, selected))
                return;

            Material selectedMaterial = Instantiate(SelectedMaterial);
            SetAlpha(selectedMaterial, SpeedrunSettings.CheckpointOpacity.Value);
            newSelected.GetComponent<MeshRenderer>().material = selectedMaterial;

            // using unity's override to check if null or destroyed
            if (selected != null) {
                Material material = Instantiate(Material);
                SetAlpha(material, SpeedrunSettings.CheckpointOpacity.Value);
                selected.GetComponent<MeshRenderer>().material = material;
            }

            selected = newSelected;
        }
#endif
    }

}
