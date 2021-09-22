using Scripts.Player;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class PresenterLife : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private PlayerVital _playerVital;
        [SerializeField] private int maxLifeValue;

        void Start()
        {
            _playerVital.maxLifeObservable.Subscribe(maxLife => maxLifeValue = maxLife);
            _playerVital.lifeObservable.Subscribe(　life => RefreshUI(life));
        }

        void RefreshUI(int life)
        {
            float lifeRatio = (float)life / maxLifeValue;
            _image.fillAmount = lifeRatio;
        }
    }
}
