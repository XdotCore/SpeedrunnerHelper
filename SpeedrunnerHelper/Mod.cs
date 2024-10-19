﻿using MelonLoader;

[assembly: MelonInfo(typeof(SpeedrunnerHelper.Mod), "Speedrunner Helper", "0.1.0", "X.Core")]
[assembly: MelonGame("Rubeki Games", "LornsLure")]

// TODO: hide fog
// TODO: show player velocity

namespace SpeedrunnerHelper {
    class Mod : MelonMod {
        public static MelonLogger.Instance Logger { get; private set; }

        public override void OnInitializeMelon() {
            Logger = LoggerInstance;
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName) {
            Revisible.ShowCheckpointsAndEndTrigger();
        }
    }
}