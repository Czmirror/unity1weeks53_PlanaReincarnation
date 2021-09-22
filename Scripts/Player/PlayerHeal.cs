using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Scripts.Interface;
using Scripts.Vital;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerHeal: MonoBehaviour, IHealable
    {
        public ReactiveProperty<bool> isHeal =  new ReactiveProperty<bool>(false);
        [SerializeField] public IObservable<bool> isHealObservable => isHeal;

        [SerializeField] private Heal _heal; // 現在のHealクラスを格納

        public Heal currentHeal
        {
            get{
                return this._heal;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Heal>() is var targetHeal && targetHeal != null)
            {
                _heal = targetHeal;
                Heal();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Heal>() is var targetExitHeal && targetExitHeal != null)
            {
                _heal = null;
                HealLift();
            }
        }

        /**
         * 回復中
         */
        public void Heal()
        {
            isHeal.Value = true;
        }

        /**
         * 回復解除
         */
        public void HealLift()
        {
            isHeal.Value = false;
        }
        
    }
}
