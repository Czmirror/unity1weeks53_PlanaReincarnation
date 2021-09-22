using Scripts.Resistance;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class PresenterResistanceLevel : MonoBehaviour
    {
        [SerializeField] private BaseResistance _baseResistance;
        [SerializeField] private TMP_Text _tmpText;
        
        private void Start()
        {
            _baseResistance.resistanceLevelObservable.Subscribe(level => RefreshUI(level));
        }

        private void RefreshUI(int level)
        {
            string levelText = "" + level;
            _tmpText.text = levelText;
        }
    }
}
