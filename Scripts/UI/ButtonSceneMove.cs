using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI
{
    public class ButtonSceneMove : MonoBehaviour
    {
        public string loadScene;

        public void PushButton () {
            SceneManager.LoadScene ( loadScene );
        }
    }
}