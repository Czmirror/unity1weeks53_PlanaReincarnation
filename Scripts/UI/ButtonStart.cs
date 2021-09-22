using UnityEngine;
using UniRx;

namespace Scripts.UI
{
    public class ButtonStart : MonoBehaviour
    {
        /// <summary>ボタンが押されたか監視するためのReactiveProperty</summary>
        public ReactiveProperty<bool> isPush = new ReactiveProperty<bool>(false);

        /// <summary>遷移速度</summary>
        [SerializeField] private float speed = 2.0f;
        /// <summary>切り替え後のシーン名</summary>
        [SerializeField] private string loadScene;

        public void PushedButton()
        {
            isPush.Value = true;
            FadeManager.Instance.LoadScene(loadScene, speed);
        }
    }
}
