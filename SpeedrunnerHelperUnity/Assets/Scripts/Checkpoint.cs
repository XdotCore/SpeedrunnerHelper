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

        public Material Material { get; set; }
        public Material SelectedMaterial { get; set; }
        protected static Checkpoint selected = null;

        protected override void Awake() {
            GetComponent<MeshRenderer>().material = Instantiate(Material);
            base.Awake();
        }

        protected override void OnDestroy() => base.OnDestroy();

        private static Checkpoint GetCheckpointFrom(Transform t) {
            if (t.GetComponent<CheckpointSet>() is null && t.GetComponent<ClimberOrFlowingObjectCheckpoint>() is null)
                return null;
            return t.GetComponentInChildren<Checkpoint>();
        }

        [HarmonyPatch(typeof(CheckpointStore), "SetNewCheckpoint")]
        [HarmonyPostfix]
        private static void SwapSelectedCheckpoint(Transform t) {
            Checkpoint newSelected = GetCheckpointFrom(t) ?? GetCheckpointFrom(t.parent);
            if (newSelected is null || ReferenceEquals(newSelected, selected))
                return;

            Material selectedMaterial = Instantiate(newSelected.SelectedMaterial);
            SetAlpha(selectedMaterial, SpeedrunSettings.CheckpointOpacity.Value);
            newSelected.GetComponent<MeshRenderer>().material = selectedMaterial;

            // using unity's override to check if null or destroyed
            if (selected != null) {
                Material material = Instantiate(selected.Material);
                SetAlpha(material, SpeedrunSettings.CheckpointOpacity.Value);
                selected.GetComponent<MeshRenderer>().material = material;
            }

            selected = newSelected;
        }
#endif
    }

}
