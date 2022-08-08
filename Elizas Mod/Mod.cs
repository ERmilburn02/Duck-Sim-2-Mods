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
            if (GUILayout.Button("CREDITS")) LoadCredits();
        }

        void LoadCredits()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)DS2_Mod_Helper.Scenes.Credits);
        }
    }
}
