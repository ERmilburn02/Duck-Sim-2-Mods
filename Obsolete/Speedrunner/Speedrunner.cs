using System;
using System.Collections;
using MelonLoader;
using DS2_Mod_Helper;
using UnityEngine;

namespace Speedrunner
{
    public class Speedrunner : MelonMod
    {
        public static Speedrunner instance;

        // We default times to DateTime.MinValue as a default
        public DateTime startTime = DateTime.MinValue;
        public DateTime clickerEnd = DateTime.MinValue;
        public DateTime fpsEnd = DateTime.MinValue;
        public DateTime platformerEnd = DateTime.MinValue;
        public DateTime rpgEnd = DateTime.MinValue;
        public DateTime endTime = DateTime.MinValue;

        public bool running = false;

        // We store times as strings to display, in order to cache them
        public string clickerDisplay = "00:00:00";
        public string fpsDisplay = "00:00:00";
        public string platformDisplay = "00:00:00";
        public string rpgDisplay = "00:00:00";
        public string totalDisplay = "00:00:00";

        // To ensure that even if the scene gets reloaded
        // the timer doesn't get messed up
        public bool clickerLoaded = false;
        public bool fpsLoaded = false;
        public bool platformerLoaded = false;
        public bool rpgLoaded = false;

        object co;

        // TODO: Move display methods into Mod Helper

        public override void OnApplicationLateStart()
        {
            if (instance == null) instance = this;
        }

        private void Reset()
        {
            MelonCoroutines.Stop(co);
            running = false;
            startTime = DateTime.MinValue;
            clickerEnd = DateTime.MinValue;
            fpsEnd = DateTime.MinValue;
            platformerEnd = DateTime.MinValue;
            rpgEnd = DateTime.MinValue;
            endTime = DateTime.MinValue;
            clickerLoaded = false;
            fpsLoaded = false;
            platformerLoaded = false;
            rpgLoaded = false;
            UpdateDisplays();
        }

        private void Start()
        {
            startTime = DateTime.Now;
            clickerLoaded = true;
            running = true;
            co = MelonCoroutines.Start(loop());
        }

        IEnumerator loop()
        {
            while (true)
            {
                UpdateDisplays();
                yield return new WaitForSeconds(1);
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            switch (buildIndex)
            {
                case (int)Scenes.MainMenu:
                    break;
                case (int)Scenes.ClickerScene:
                    if (!clickerLoaded)
                    {
                        // Start the timer
                        Start();
                    }
                    break;
                case (int)Scenes.FifthWall:
                    break;
                case (int)Scenes.fpsLevel1:
                    if (!fpsLoaded)
                    {
                        fpsLoaded = true;
                        clickerEnd = DateTime.Now;
                    }
                    break;
                case (int)Scenes.platformerLevel1:
                    if (!platformerLoaded)
                    {
                        platformerLoaded = true;
                        fpsEnd = DateTime.Now;
                    }
                    break;
                case (int)Scenes.rpgLevel1:
                    if (!rpgLoaded)
                    {
                        rpgLoaded = true;
                        platformerEnd = DateTime.Now;
                    }
                    break;
                case (int)Scenes.Credits:
                    // End run
                    var now = DateTime.Now;
                    rpgEnd = now;
                    endTime = now;
                    break;
                default:
                    break;
            }
        }

        public override void OnGUI()
        {
            GUI.backgroundColor = Color.black;
            GUI.Box(new Rect(10, 10, 100, 500), "");
            GUILayout.BeginArea(new Rect(10, 10, 100, 500));
            if (GUILayout.Button("Reset"))
            {
                Reset();
            }
            GUILayout.Label("Clicker");
            GUILayout.Label(clickerDisplay);
            GUILayout.Label("FPS");
            GUILayout.Label(fpsDisplay);
            GUILayout.Label("Platformer");
            GUILayout.Label(platformDisplay);
            GUILayout.Label("RPG");
            GUILayout.Label(rpgDisplay);
            GUILayout.Label("Total");
            GUILayout.Label(totalDisplay);
            GUILayout.EndArea();
        }

        void UpdateDisplays()
        {
            // LoggerInstance.Msg($"Updating:\nClicker ({clickerLoaded && !fpsLoaded})\nFPS ({fpsLoaded && !platformerLoaded})\nPlatformer ({platformerLoaded && !rpgLoaded})\nRPG ({rpgLoaded})");
            // LoggerInstance.Msg($"Times:\nStart ({startTime})\nClicker End ({clickerEnd})\nFPS End ({fpsEnd})\nPlatformer End ({platformerEnd})\nRPG End ({rpgEnd})\nEnd Time ({endTime})");
            if (startTime == DateTime.MinValue)
            {
                // Keep all timers empty
                clickerDisplay = "00:00:00";
                fpsDisplay = "00:00:00";
                platformDisplay = "00:00:00";
                rpgDisplay = "00:00:00";
                totalDisplay = "00:00:00";
                return;
            }
            if (!running) return;
            if (endTime != DateTime.MinValue)
            {
                LoggerInstance.Msg("Run over, updating times to ms");
                // Refresh all displays to show them as mm:ss:ff
                clickerDisplay = (clickerEnd - startTime).ToString(@"mm\:ss\:ff");
                fpsDisplay = (fpsEnd - clickerEnd).ToString(@"mm\:ss\:ff");
                platformDisplay = (platformerEnd - fpsEnd).ToString(@"mm\:ss\:ff");
                rpgDisplay = (rpgEnd - platformerEnd).ToString(@"mm\:ss\:ff");
                totalDisplay = (endTime - startTime).ToString(@"mm:\ss:\ff");
                running = false;
                return;
            }
            var now = DateTime.Now;
            var total = now - startTime;
            totalDisplay = total.ToString(@"hh\:mm\:ss");
            if (clickerLoaded && !fpsLoaded)
            {
                clickerDisplay = (now - startTime).ToString(@"hh\:mm\:ss");
            }

            if (fpsLoaded && !platformerLoaded)
            {
                fpsDisplay = (now - clickerEnd).ToString(@"hh\:mm\:ss");
            }

            if (platformerLoaded && !rpgLoaded)
            {
                platformDisplay = (now - fpsEnd).ToString(@"hh\:mm\:ss");
            }

            if (rpgLoaded)
            {
                rpgDisplay = (now - platformerEnd).ToString(@"hh\:mm\:ss");
            }
        }
    }
}
