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
    public class PlayerDamage : MonoBehaviour, IDamageable
    {
        /// <summary>ダメージ時のフラグReactiveProperty</summary>
        public ReactiveProperty<bool> isDamage = new ReactiveProperty<bool>(false);

        /// <summary>外部用のダメージフラグReactiveProperty</summary>
        [SerializeField]
        public IObservable<bool> isDamageObservable => isDamage;

        /// <summary>現在ダメージを与えているDamageクラス</summary>
        [SerializeField] private Damage _damage;

        /// <summary>現在ダメージを与えているクラスを取得するためのプロパティ</summary>
        public Damage currentDamage
        {
            get { return this._damage; }
        }

        /// <summary>ダメージオブジェクト接触時</summary>
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<Damage>() is var targetDamage && targetDamage != null)
            {
                _damage = targetDamage;
                Damage();
            }
        }

        /// <summary>ダメージオブジェクトから離れた時</summary>
        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<Damage>() is var targetExitDamage && targetExitDamage != null)
            {
                _damage = null;
                DamageLift();
            }
        }

        /// <summary>ダメージ</summary>
        public void Damage()
        {
            isDamage.Value = true;
        }

        /// <summary>ダメージ解除</summary>
        public void DamageLift()
        {
            isDamage.Value = false;
        }
    }
}
