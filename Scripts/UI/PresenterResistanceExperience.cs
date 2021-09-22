using Scripts.Resistance;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class PresenterResistanceExperience : MonoBehaviour
    {
        [SerializeField] private BaseResistance _baseResistance;
        [SerializeField] private Image _image;

        private void Start()
        {
            _baseResistance.resistanceExperienceObservable.Subscribe(experience => RefreshUI(experience));
        }

        private void RefreshUI(float experience)
        {
            float uiRatio = (float) experience / _baseResistance.MaxExperience;
            _image.fillAmount = uiRatio;
        }
    }
}
