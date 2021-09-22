using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerVital : MonoBehaviour
    {
        [SerializeField] private int lifeInitialValue = 10;
        [SerializeField] private int maxLifeInitialValue = 10;
        [SerializeField] private ReactiveProperty<int> life = new ReactiveProperty<int>(10);
        [SerializeField] public IObservable<int> lifeObservable => life;
        [SerializeField] private ReactiveProperty<int> maxlife = new ReactiveProperty<int>(10);
        [SerializeField] public IObservable<int> maxLifeObservable => maxlife;
        [SerializeField] private PlayerHeal _playerHeal;
        [SerializeField] private PlayerDamage _playerDamage;
        [SerializeField] private int waitTime = 1; // 回復のインターバル

        [SerializeField] private PlayerResistance _playerResistance;

//        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
//        private CancellationTokenSource cts;
//        private CancellationToken ct;

        private bool isHeal = false; 
        private bool isDamage = false; 

        private void Start()
        {
            maxlife.Value = maxLifeInitialValue;
            life.Value = lifeInitialValue;

            // ライフフラグを自身の変数に格納
            _playerHeal.isHealObservable.Subscribe(x => isHeal = x);
            
            // ライフフラグが有効な場合、回復処理
            _playerHeal.isHealObservable.Where(x => x).Subscribe(x =>
            {
                var cts = new CancellationTokenSource();
                LifeUp(cts.Token);
            });
            
            // ダメージフラグを自身の変数に格納
            _playerDamage.isDamageObservable.Subscribe(x => isDamage = x);
            
            // ダメージフラグが有効な場合、ダメージ処理
            _playerDamage.isDamageObservable.Where(x => x).Subscribe(x =>
            {
                var cts = new CancellationTokenSource();
                LifeDown(cts.Token);
            });
        }

        private async UniTask LifeUp(CancellationToken token)
        {
            // 回復値を元にライフを回復
            while (isHeal && life.Value < maxlife.Value)
            {
                await UniTask.Delay((waitTime * 1000), cancellationToken: token);
                if (_playerHeal.currentHeal == null)
                {
                    break;
                }
                
                life.Value += _playerHeal.currentHeal.currentAddLife;

                if (life.Value >= maxlife.Value)
                {
                    life.Value = maxlife.Value;
                } 
            }
        }

        public async UniTask LifeDown(CancellationToken token)
        {
            while (isDamage)
            {
                // ダメージを取得
                var damage = _playerDamage.currentDamage;
                
                // 耐性値からダメージを差し引く
                var resistanceDamage = _playerResistance.DamageResistance(damage);

                // ライフから計算されたダメージを差し引く

                life.Value -= resistanceDamage;
                if (life.Value <= 0)
                {
                    life.Value = 0;
                }
            
                // ダメージインターバル
                await UniTask.Delay((waitTime * 1000), cancellationToken: token);
            }
            
        }
    }
}
