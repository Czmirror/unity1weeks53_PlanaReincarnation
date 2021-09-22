using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class ViewLife : MonoBehaviour
    {
        [SerializeField] private Image _image;

        /**
         * ライフUIを更新する
         * lifeRatio ライフの割合
         */
        public void SetRatio(float lifeRatio)
        {
            _image.fillAmount = lifeRatio;
        }
    }
}
