using HarmonyLib;
using MelonLoader;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace SpeedrunnerHelper {
    [HarmonyPatch]
    static class SpeedrunSettings {
        private static MelonPreferences_Category _speedrunCat = null;
        public static MelonPreferences_Category SpeedrunCat { get {
                if (_speedrunCat is null) {
                    _speedrunCat = MelonPreferences.CreateCategory("SpeedrunnerHelper");
                    _speedrunCat.SetFilePath("UserData/SpeedrunnerHelper.cfg");
                }
                return _speedrunCat;
            } }

        private static MelonPreferences_Entry<bool> _visibleCheckpoints = null;
        public static MelonPreferences_Entry<bool> VisibleCheckpoints => _visibleCheckpoints ??= SpeedrunCat.CreateEntry("VisibleCheckpoints", true);

        private static MelonPreferences_Entry<float> _checkpointOpacity = null;
        public static MelonPreferences_Entry<float> CheckpointOpacity => _checkpointOpacity ??= SpeedrunCat.CreateEntry("CheckpointOpacity", .01f);

        private static MelonPreferences_Entry<bool> _visibleEndTrigger = null;
        public static MelonPreferences_Entry<bool> VisibleEndTrigger => _visibleEndTrigger ??= SpeedrunCat.CreateEntry("VisibleEndTrigger", true);

        private static MelonPreferences_Entry<float> _endTriggerOpacity = null;
        public static MelonPreferences_Entry<float> EndTriggerOpacity => _endTriggerOpacity ??= SpeedrunCat.CreateEntry("EndTriggerOpacity", .2f);

        private static readonly AssetBundle settingsBundle;
        private static int sectionIndex;

        static SpeedrunSettings() {
            using Stream settingsBundleStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SpeedrunnerHelper.Unity.AssetBundles.settings");
            settingsBundle = AssetBundle.LoadFromStream(settingsBundleStream);
        }

        [HarmonyPatch(typeof(GameOptions), "Awake")]
        [HarmonyPrefix]
        private static void AddGUI(GameOptions __instance) {
            static void RemoveCloneInName(GameObject go) =>
                go.name = go.name.Replace("(Clone)", "");

            // Creating the form
            GameObject sectionForm = Object.Instantiate(settingsBundle.LoadAsset<GameObject>("SpeedrunnerForm"), __instance.Forms[0].parent);
            RemoveCloneInName(sectionForm);
            sectionForm.SetActive(false);
            sectionIndex = __instance.Forms.Count;
            __instance.Forms.Add(sectionForm.transform);

            // Adding the section toggle button
            Transform sectionMenu = __instance.transform.Find("Window/SectionMenu");
            RectTransform lastToggle = sectionMenu.GetChild(sectionMenu.childCount - 1) as RectTransform;
            GameObject sectionToggle = Object.Instantiate(settingsBundle.LoadAsset<GameObject>("Speedrunner_MenuButton"), sectionMenu);
            RemoveCloneInName(sectionToggle);

            sectionToggle.GetComponent<SectionToggle>().SectionIndex = sectionIndex;
            sectionToggle.GetComponent<Toggle>().group = sectionMenu.GetComponent<ToggleGroup>();

            sectionToggle.transform.localPosition = new Vector3(sectionToggle.transform.localPosition.x, lastToggle.localPosition.y - lastToggle.rect.height);
        }

        [HarmonyPatch(typeof(GameOptions), "ShowOptionsForm")]
        [HarmonyPostfix]
        private static void CancelSettings(int index) {
            if (index != sectionIndex) {
                Setting.ResetAllSettingsToCurrent();
                Setting.UpdateAllSettingsGUI();
            }
        }
    }
}
