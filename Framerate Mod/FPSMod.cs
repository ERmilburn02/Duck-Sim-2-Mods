using MelonLoader;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DS2_Mod_Helper;

namespace Framerate_Mod
{
    public class FPSMod : MelonMod
    {
        public static MelonPreferences_Category fpsCategory;
        public static MelonPreferences_Entry<int> fpsTarget;
        public static MelonPreferences_Entry<int> lowerKey;
        public static MelonPreferences_Entry<int> restoreKey;

        private bool showSettings = false;
        private static string settingsFPSInput;
        private static int settingsLowerStore = -1;
        private static int settingsRestoreStore = -1;

        private static string settingsLower;
        private static string settingsLowerDefault = "Lower Key";
        private static string settingsRestore;
        private static string settingsRestoreDefault = "Restore Key";
        private const string settingsWaiting = "Waiting for input";

        private static bool waitingForInput = false;
        private static bool waitingForLower = false;
        private static bool waitingForRestore = false;

        public override void OnApplicationStart()
        {
            fpsCategory = MelonPreferences.CreateCategory("Framerate Mod");

            fpsTarget = fpsCategory.CreateEntry<int>("Framerate", 60, "Target Framerate", "Target framerate to switch to", false, false);
            lowerKey = fpsCategory.CreateEntry<int>("Lower", (int)KeyCode.Q, "Lower Keycode", "Keycode to lower framerate", false, false);
            restoreKey = fpsCategory.CreateEntry<int>("Restore", (int)KeyCode.E, "Restore Keycode", "Keycode to restore framerate", false, false);

            MelonPreferences.Load();

            settingsFPSInput = fpsTarget.Value.ToString();
            settingsLower = settingsLowerDefault;
            settingsRestore = settingsRestoreDefault;
        }

        public override void OnUpdate()
        {
            if (waitingForInput)
            {
                if (waitingForLower)
                {
                    if (Input.anyKeyDown)
                    {
                        KeyCode key = GetDownKey();
                        settingsLowerStore = (int)key;
                        settingsLower = key.ToString();
                        waitingForInput = false;
                    }
                }
                else if (waitingForRestore)
                {
                    if (Input.anyKeyDown)
                    {
                        KeyCode key = GetDownKey();
                        settingsRestoreStore = (int)key;
                        settingsRestore = key.ToString();
                        waitingForInput = false;
                    }
                }
                else
                {
                    waitingForInput = false;
                }

                return;
            }

            if (Input.GetKeyDown((KeyCode)lowerKey.Value))
            {
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = fpsTarget.Value;
                // LoggerInstance.Msg($"Changed framerate to {fpsTarget.Value}");
            }
            if (Input.GetKeyDown((KeyCode)restoreKey.Value))
            {
                QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = -1;
                // LoggerInstance.Msg($"Changed framerate to VSync");
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex == (int)Scenes.MainMenu)
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
                GUILayout.Space(24);

                GUILayout.Label("Framerate Mod Settings");

                settingsFPSInput = GUILayout.TextField(settingsFPSInput);

                if (GUILayout.Button(settingsLower))
                {
                    settingsLower = settingsWaiting;
                    settingsRestore = settingsRestoreDefault;
                    waitingForInput = true;
                    waitingForLower = true;
                    waitingForRestore = false;
                }

                if (GUILayout.Button(settingsRestore))
                {
                    settingsRestore = settingsWaiting;
                    settingsLower = settingsLowerDefault;
                    waitingForInput = true;
                    waitingForRestore = true;
                    waitingForLower = false;
                }

                if (GUILayout.Button("Save"))
                {
                    if (int.TryParse(settingsFPSInput, out int result))
                    {
                        fpsTarget.Value = result;
                    }

                    if (settingsLowerStore != -1)
                    {
                        lowerKey.Value = settingsLowerStore;
                    }

                    if (settingsRestoreStore != -1)
                    {
                        restoreKey.Value = settingsRestoreStore;
                    }

                    MelonPreferences.Save();
                }
            }
        }

        private KeyCode GetDownKey()
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vKey))
                {
                    return vKey;
                }
            }
            throw new Exception();
        }
    }
}
