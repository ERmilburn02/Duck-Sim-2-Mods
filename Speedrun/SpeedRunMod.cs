using MelonLoader;
using DS2_Mod_Helper;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO: Remove all cutscenes
// TODO: Prevent speed change in platformerLevel7
// TODO: Make the bridge faster in fpsLevel1.

namespace Speedrun
{
    public class SpeedRunMod : MelonMod
    {
        private GameObject m_HiddenCollider = null;
        private PlatformerPlayer m_PlatformerPlayer = null;
        private GameObject m_Bridge = null;
        private bool m_IsPlatformerLevel7 = false;

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            m_HiddenCollider = null;
            m_PlatformerPlayer = null;
            m_Bridge = null;
            m_IsPlatformerLevel7 = false;

            switch ((Scenes)buildIndex)
            {
                case Scenes.FifthWall:
                    SkipFifthWall();
                    break;
                case Scenes.fpsLevel1:
                    ChangeBridgeSpeed();
                    break;
                case Scenes.platformerLevel7:
                    m_IsPlatformerLevel7 = true;
                    SetupPlatformerLevel7();
                    break;
                default:
                    break;
            }
            ModHelper.DisableSpeedrunMode();
        }

        private void SkipFifthWall()
        {
            PlayerPrefs.SetInt("skipCutscene", 1);
            SceneManager.LoadScene(PlayerPrefs.GetInt("nextScene", 0));
        }

        private void ChangeBridgeSpeed()
        {
            Animation[] animations = GameObject.FindObjectsOfType<Animation>();
            Animation m_BridgeAnim = null;
            foreach (var anim in animations)
            {
                if (anim.clip.name == "Long Bridge")
                {
                    m_BridgeAnim = anim;
                    break;
                }
            }
            if (m_BridgeAnim != null)
            {
                ShootableButton[] buttons = GameObject.FindObjectsOfType<ShootableButton>();
                ShootableButton button = null;
                foreach (var sb in buttons)
                {
                    if (sb.activationAnimation == m_BridgeAnim)
                    {
                        button = sb;
                        break;
                    }
                }
                m_Bridge = m_BridgeAnim.gameObject;
                button.activationAnimation = null;
                m_Bridge.transform.localPosition = new Vector3(39.0f, 0.45f, -12.5f);
                m_Bridge.SetActive(false);
                button.onActivation.AddListener(() => { m_Bridge.SetActive(true); });
            }
        }

        private void SetupPlatformerLevel7()
        {
            m_PlatformerPlayer = GameObject.FindObjectOfType<PlatformerPlayer>();
            m_PlatformerPlayer.slowDown = false;
            m_HiddenCollider = m_PlatformerPlayer.eventObjects[0];
            m_HiddenCollider.transform.position = new Vector3(m_HiddenCollider.transform.position.x, -13.5f);
            m_HiddenCollider.GetComponent<Collider2D>().isTrigger = true;

            var go = new GameObject() { name = "Wall" };
            go.AddComponent<BoxCollider2D>();
            go.transform.position = new Vector3(41.5f, 1.5f, 0.0f);
            go.transform.localScale = new Vector3(1, 5, 1);
        }

        public override void OnUpdate()
        {
            if (m_IsPlatformerLevel7)
            {
                if (m_HiddenCollider.GetComponent<Collider2D>().IsTouching(m_PlatformerPlayer.GetComponent<Collider2D>()))
                {
                    SkipPlatformerLevel7Ending();
                    m_IsPlatformerLevel7 = false; // Prevent re-running until the scene is next loaded
                }
            }
        }

        private void SkipPlatformerLevel7Ending()
        {
            PlayerPrefs.SetInt("chapter", 3);
            PlayerPrefs.SetInt("skipCutscene", 0);
            PlayerPrefs.SetInt("nextScene", 13);
            SceneManager.LoadScene(0);
        }
    }
}
