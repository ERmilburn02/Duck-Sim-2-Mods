using MelonLoader;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elizas_Mod
{
    public class Mod : MelonMod
    {
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
            if (GUI.Button(new Rect((Screen.width / 2 - 7.5f), 10.0f, 15.0f, 20), "QUACKCORE"))
            {
                // PlayerPrefs.GetInt("quackcore") == 1
                PlayerPrefs.SetInt("quackcore", 1);
            }

        }
    }
}
