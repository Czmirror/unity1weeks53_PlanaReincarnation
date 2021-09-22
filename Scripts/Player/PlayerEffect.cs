using Scripts.Resistance;
using UnityEngine;
using UniRx;

namespace Scripts.Player
{
    public class PlayerEffect : MonoBehaviour
    {
        [SerializeField] private AudioClip lifeUp;
        [SerializeField] private AudioClip lifeMax;
        [SerializeField] private AudioClip damageVoice;
        [SerializeField] private AudioClip downVoice;
        [SerializeField] private AudioClip resistanceUp;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private PlayerVital _playerVital;
        [SerializeField] private PlayerVitalStatus _playerVitalStatus;
        [SerializeField] private PlayerHeal _playerHeal;
        [SerializeField] private PlayerDamage _playerDamage;

        [SerializeField] private int maxLife;

        private void Start()
        {
            // 判定用主人公最大ライフ設定
            _playerVital.maxLifeObservable.Subscribe(x => maxLife = x);

            // 主人公回復
            _playerVital.lifeObservable
                .Where(x => x != maxLife)
                .Where(_ => _playerHeal.isHeal.Value)
                .Subscribe(_ => { PlayerEffectLifeUp(); });

            // 主人公全回復
            _playerVital.lifeObservable
                .Where(x => x == maxLife)
                .Where(_ => _playerHeal.isHeal.Value)
                .Subscribe(_ => { PlayerEffectLifeMaxUp(); });

            // プレイヤーダメージ
            _playerVital.lifeObservable
                .Where(_ => _playerDamage.isDamage.Value)
                .Where(_ => _playerVitalStatus.playerVitalState.Value != PlayerVitalState.Down)
                .Subscribe(_ => { PlayerEffectDamage(); });

            // 主人公やられ
            _playerVitalStatus.playerVitalState
                .DistinctUntilChanged()
                .Where(x =>
                    x == PlayerVitalState.Down
                )
                .Subscribe(_ => { PlayerEffectDown(); });
        }

        private void PlayerEffectLifeUp()
        {
            _audioSource.clip = lifeUp;
            _audioSource.Play();
        }

        private void PlayerEffectLifeMaxUp()
        {
            _audioSource.clip = lifeMax;
            _audioSource.Play();
        }

        private void PlayerEffectDamage()
        {
            _audioSource.clip = damageVoice;
            _audioSource.Play();
        }

        private void PlayerEffectDown()
        {
            _audioSource.clip = downVoice;
            _audioSource.Play();
        }
    }
}
