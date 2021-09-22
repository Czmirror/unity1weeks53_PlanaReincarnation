using System;
using Scripts.Interface;
using UnityEngine;
using UniRx;

namespace Scripts.Resistance
{
    public class BaseResistance : MonoBehaviour, IResistance
    {
        /// <summary>耐性の現在レベル</summary>
        [SerializeField] private ReactiveProperty<int> _resistanceLevel = new ReactiveProperty<int>(0);
        [SerializeField] public IObservable<int> resistanceLevelObservable => _resistanceLevel;

        /// <summary>レベルの最大値</summary>
        [SerializeField] private int _resistanceMaxLevel = 5;

        /// <summary>耐性の属性</summary>
        [SerializeField] private ResistanceType _resistanceType;

        /// <summary>耐性の現在経験値</summary>
        [SerializeField] private ReactiveProperty<float> _resistanceExperience = new ReactiveProperty<float>(0);
        [SerializeField] public IObservable<float> resistanceExperienceObservable => _resistanceExperience;
        
        /// <summary>ダメージ時に得られる経験値</summary>
        [SerializeField] private float _addExperience = 0.1f;

        /// <summary>経験値の最低値</summary>
        [SerializeField] private float _resistanceMinExperience = 0;

        /// <summary>経験値の最大値</summary>
        [SerializeField] private float _resistanceMaxExperience = 1;

        /// <summary>外部から参照する耐性の現在レベル</summary>
        public int CurrentLevel
        {
            get { return _resistanceLevel.Value; }
        }

        /// <summary>外部から参照する耐性の最大レベル</summary>
        public int MaxLevel
        {
            get { return _resistanceMaxLevel; }
        }

        /// <summary>外部から参照する耐性の最大経験値</summary>
        public float MaxExperience
        {
            get { return _resistanceMaxExperience; }
        }
        
        /// <summary>外部から参照する耐性の最大経験値</summary>
        public float MinExperience
        {
            get { return _resistanceMinExperience; }
        }

        /// <summary>経験値の初期化</summary>
        private void resetExperience()
        {
            _resistanceExperience.Value = _resistanceMinExperience;
        }

        /// <summary>経験値取得処理</summary>
        public void GetExperience(ResistanceType resistanceType)
        {
            if (_resistanceType == resistanceType)
            {
                // 最大レベルの場合はそのままreturn
                if (_resistanceLevel.Value == _resistanceMaxLevel)
                {
                    return;
                }

                // 経験値取得
                _resistanceExperience.Value = _resistanceExperience.Value + _addExperience;

                // 耐性レベルアップ
                if (_resistanceExperience.Value >= _resistanceMaxExperience)
                {
                    LevelUp();
                    resetExperience();
                }
            }
        }

        /// <summary>耐性レベルアップ処理</summary>
        public void LevelUp()
        {
            // 最大レベルの場合はそのままreturn
            if (_resistanceLevel.Value >= _resistanceMaxLevel)
            {
                return;
            }

            _resistanceLevel.Value++;
        }
    }
}
