using MelonLoader;
using UnityEngine;
using DS2_Mod_Helper;

namespace Teleport_Mod
{
    public class TPMod : MelonMod
    {
        private void Teleport(Scenes toScene)
        {
            if (SpeedrunManager.instance != null)
            {
                if (SpeedrunManager.instance.isRunning)
                {
                    SpeedrunManager.instance.isRunning = false;
                    GameObject.Destroy(SpeedrunManager.instance.gameObject);
                }
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)toScene);
        }

        public override void OnGUI()
        {
            GUILayout.Space(24);
            GUILayout.Label("Teleport Mod");
            if (GUILayout.Button("Main Menu (z)")) Teleport(Scenes.MainMenu);
            if (GUILayout.Button("Clicker (x)")) Teleport(Scenes.ClickerScene);
            if (GUILayout.Button("Shooter (c)")) Teleport(Scenes.fpsLevel1);
            if (GUILayout.Button("Platformer (v)")) Teleport(Scenes.platformerLevel1);
            if (GUILayout.Button("RPG (b)")) Teleport(Scenes.rpgLevel1);
            if (GUILayout.Button("Credits (n)")) Teleport(Scenes.Credits);
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Z)) Teleport(Scenes.MainMenu);
            if (Input.GetKeyDown(KeyCode.X)) Teleport(Scenes.ClickerScene);
            if (Input.GetKeyDown(KeyCode.C)) Teleport(Scenes.fpsLevel1);
            if (Input.GetKeyDown(KeyCode.V)) Teleport(Scenes.platformerLevel1);
            if (Input.GetKeyDown(KeyCode.B)) Teleport(Scenes.rpgLevel1);
            if (Input.GetKeyDown(KeyCode.N)) Teleport(Scenes.Credits);
        }
    }
}
