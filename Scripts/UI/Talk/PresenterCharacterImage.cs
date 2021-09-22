using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Talk
{
    public class PresenterCharacterImage : MonoBehaviour
    {
        [SerializeField] private ReadJson _readJson;
        [SerializeField] private Image _image;
        [SerializeField] private Sprite blankIcon;
        [SerializeField] private Sprite pranaIcon;
        [SerializeField] private Sprite riaIcon;
        [SerializeField] private Sprite pranaDamage;
        [SerializeField] private Sprite riaDamage;
        [SerializeField] private Sprite pranaRelief;
        [SerializeField] private Sprite pranaJoy;
        
        private void Start()
        {
            _readJson.characterImageObservable.DistinctUntilChanged().Subscribe(x =>
            {
                SetImage(x);
            });
        }

        private void SetImage(string imageName)
        {
            switch (imageName)
            {
                case "PlanaIcon":
                    _image.sprite = pranaIcon;
                    break;
                case "RiaIcon":
                    _image.sprite = riaIcon;
                    break;
                case "PlanaDamage":
                    _image.sprite = pranaDamage;
                    break;
                case "PranaRelief":
                    _image.sprite = pranaRelief;
                    break;
                case "PranaJoy":
                    _image.sprite = pranaJoy;
                    break;
                default:
                    _image.sprite = blankIcon;
                    break;
            }
        }
    }
}
