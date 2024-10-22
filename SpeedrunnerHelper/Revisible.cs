using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace SpeedrunnerHelper {
    [HarmonyPatch]
    static class Revisible {

        private static readonly GameObject spherePrefab;
        private static readonly GameObject boxPrefab;
        private static readonly Material checkpointMat;
        private static readonly Material selectedCheckpointMat;
        private static readonly Material boatCheckpointMat;
        private static readonly Material selectedBoatCheckpointMat;

        static Revisible() {
            using Stream revisibleBundleStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SpeedrunnerHelper.Unity.AssetBundles.revisible");
            AssetBundle revisible = AssetBundle.LoadFromStream(revisibleBundleStream);

            spherePrefab              = revisible.LoadAsset<GameObject>("Sphere");
            boxPrefab                 = revisible.LoadAsset<GameObject>("Box");
            checkpointMat             = revisible.LoadAsset<Material>("CheckpointMat");
            selectedCheckpointMat     = revisible.LoadAsset<Material>("SelectedCheckpointMat");
            boatCheckpointMat         = revisible.LoadAsset<Material>("BoatCheckpointMat");
            selectedBoatCheckpointMat = revisible.LoadAsset<Material>("SelectedBoatCheckpointMat");
        }

        private static GameObject MakeSphere(SphereCollider sphere) {
            GameObject visible = Object.Instantiate(spherePrefab, sphere.transform);
            visible.transform.localScale *= 2 * sphere.radius;
            visible.transform.localPosition = sphere.center;
            visible.SetActive(false);
            return visible;
        }

        private static GameObject MakeBox(BoxCollider box) {
            GameObject visible = Object.Instantiate(boxPrefab, box.transform);
            visible.transform.localScale = box.size;
            visible.transform.localPosition = box.center;
            visible.SetActive(false);
            return visible;
        }

        private static void MakeCheckpoint(Transform t, Material mat, Material selectedMat) {
            Checkpoint checkpoint = t.GetComponent<Collider>() switch {
                SphereCollider checkpointSphere => MakeSphere(checkpointSphere).AddComponent<Checkpoint>(),
                BoxCollider checkpointBox => MakeBox(checkpointBox).AddComponent<Checkpoint>(),
                _ => null
            };
            if (checkpoint is null)
                return;

            checkpoint.Material = mat;
            checkpoint.SelectedMaterial = selectedMat;
            checkpoint.gameObject.SetActive(true);
        }

        [HarmonyPatch(typeof(CheckpointSet), "Awake")]
        [HarmonyPostfix]
        private static void DoCheckpoint(CheckpointSet __instance) =>
            MakeCheckpoint(__instance.transform, checkpointMat, selectedCheckpointMat);

        [HarmonyPatch(typeof(ClimberOrFlowingObjectCheckpoint), "Start")]
        [HarmonyPostfix]
        private static void DoBoatCheckpoint(ClimberOrFlowingObjectCheckpoint __instance) =>
            MakeCheckpoint(__instance.transform, boatCheckpointMat, selectedBoatCheckpointMat);

        [HarmonyPatch(typeof(AnimateOnTrigger), "Start")]
        [HarmonyPostfix]
        private static void DoEndTrigger(AnimateOnTrigger __instance) {
            // check to see if this one is the ending trigger
            if (__instance.an.GetComponent<StatsScreenEnding>() is null)
                return;

            BoxCollider endTriggerBox = __instance.GetComponent<BoxCollider>();
            if (endTriggerBox is null)
                return;

            MakeBox(endTriggerBox).AddComponent<EndTrigger>().gameObject.SetActive(true);
        }
    }
}
