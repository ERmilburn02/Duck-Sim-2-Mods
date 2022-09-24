using MelonLoader;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Elizas_Mod
{
    public class Mod : MelonMod
    {
        bool rightScene = false;

        public override void OnApplicationStart()
        {
            
        }

        public override void OnUpdate()
        {
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {

        }

        public override void OnGUI()
        {
            if (GUILayout.Button("FPS1")) LoadLevel(DS2_Mod_Helper.Scenes.fpsLevel1);
            if (GUILayout.Button("PLATFORMER7")) LoadLevel(DS2_Mod_Helper.Scenes.platformerLevel7);
        }

        void LoadLevel(DS2_Mod_Helper.Scenes scene)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)scene);
        }
    }
}
