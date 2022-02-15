using MelonLoader;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framerate_Mod
{
    // DON'T USE
    public class FPSMod : MelonMod
    {
        public static MelonPreferences_Category fpsCategory;
        public static MelonPreferences_Entry<int> fpsTarget;

        private bool showSettings = false;
        private static string settingsInput;

        public override void OnApplicationStart()
        {
            fpsCategory = MelonPreferences.CreateCategory("Framerate Mod");

            fpsTarget = fpsCategory.CreateEntry<int>("Framerate", 60, "Target Framerate", "Target framerate to switch to", false, false);

            MelonPreferences.Load();

            settingsInput = fpsTarget.Value.ToString();
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = fpsTarget.Value;
                LoggerInstance.Msg($"Changed framerate to {fpsTarget.Value}");
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = -1;
                LoggerInstance.Msg($"Changed framerate to VSync");
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex == 0)
            {
                // Main menu scene
                showSettings = true;
            }
            else
            {
                showSettings = false;
            }
        }

        public override void OnGUI()
        {
            if (showSettings)
            {
                GUI.Box(new Rect(10, 10, 150, 90), "Framerate Mod Settings");
                settingsInput = GUI.TextField(new Rect(20, 40, 80, 20), settingsInput);
                if (GUI.Button(new Rect(20, 70, 80, 20), "Save"))
                {
                    if (int.TryParse(settingsInput, out int result))
                    {
                        fpsTarget.Value = result;
                        MelonPreferences.Save();
                    }
                }
            }
        }
    }
}
