using MelonLoader;
using UnityEngine;

namespace DS2_Mod_Helper
{
    public class ModHelper : MelonMod
    {
        public static void DisableSpeedrunMode()
        {
            if (SpeedrunManager.instance != null)
            {
                if (SpeedrunManager.instance.isRunning)
                {
                    SpeedrunManager.instance.isRunning = false;
                    GameObject.Destroy(SpeedrunManager.instance.gameObject);
                }
            }
        }
    }
}
