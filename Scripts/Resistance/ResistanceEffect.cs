using UniRx;
using UnityEngine;

namespace Scripts.Resistance
{
    public class ResistanceEffect : MonoBehaviour
    {
        [SerializeField] private AudioClip resistanceUp;
        [SerializeField] private BaseResistance _thromResistance;
        [SerializeField] private BaseResistance _fireResistance;
        [SerializeField] private AudioSource _audioSource;

        private void Start()
        {
            // 主人公耐性値レベルアップ
            _thromResistance.resistanceLevelObservable
                .DistinctUntilChanged()
                .Where(x => x > 0)
                .Subscribe(x => { PlayerResistanceUp(); });
            _fireResistance.resistanceLevelObservable
                .DistinctUntilChanged()
                .Where(x => x > 0)
                .Subscribe(_ => { PlayerResistanceUp(); });
        }

        private void PlayerResistanceUp()
        {
            _audioSource.clip = resistanceUp;
            _audioSource.Play();
        }
        
        
    }
}
