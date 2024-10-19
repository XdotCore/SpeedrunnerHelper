using System.IO;
using System.Reflection;
using UnityEngine;

namespace SpeedrunnerHelper {
    static class Revisible {

        private static readonly GameObject checkpointPrefab;
        private static readonly GameObject endTriggerPrefab;

        static Revisible() {
            using Stream revisibleBundleStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SpeedrunnerHelper.Unity.AssetBundles.revisible");
            AssetBundle revisible = AssetBundle.LoadFromStream(revisibleBundleStream);
            checkpointPrefab = revisible.LoadAsset<GameObject>("Checkpoint");
            endTriggerPrefab = revisible.LoadAsset<GameObject>("EndTrigger");
            Checkpoint.Material = revisible.LoadAsset<Material>("CheckpointMat");
            Checkpoint.SelectedMaterial = revisible.LoadAsset<Material>("SelectedCheckpointMat");
        }

        private static void DoCheckpoints() {
            GameObject checkpoints = GameObject.Find("Checkpoints");
            if (checkpoints is null)
                return;

            foreach (Transform checkpoint in checkpoints.transform) {
                SphereCollider checkpointSphere = checkpoint.GetComponent<SphereCollider>();
                if (checkpointSphere is null)
                    continue;

                GameObject visible = Object.Instantiate(checkpointPrefab, checkpoint);
                Transform visibleTransform = visible.GetComponent<Transform>();
                visibleTransform.localScale *= 2 * checkpointSphere.radius;
                visibleTransform.localPosition = checkpointSphere.center;
            }
        }

        private static void DoEndTrigger() {
            GameObject endTrigger = GameObject.Find("EndingTrigger");
            if (endTrigger is null)
                return;

            BoxCollider endTriggerBox = endTrigger.GetComponent<BoxCollider>();
            if (endTriggerBox is null)
                return;

            GameObject visible = Object.Instantiate(endTriggerPrefab, endTrigger.transform);
            Transform visibleTransform = visible.GetComponent<Transform>();
            visibleTransform.localScale = endTriggerBox.size;
            visibleTransform.localPosition = endTriggerBox.center;
        }

        public static void ShowCheckpointsAndEndTrigger() {
            DoCheckpoints();
            DoEndTrigger();
        }
    }
}
