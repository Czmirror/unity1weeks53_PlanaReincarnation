using Scripts.Resistance;
using UnityEngine;

namespace Scripts.Vital
{
    public class Damage : MonoBehaviour
    {
        /// <summary>差し引かれるダメージ値</summary>
        [SerializeField] private int subtractLife;
        /// <summary>ダメージの種類</summary>
        [SerializeField] private ResistanceType _resistanceType;

        /// <summary>ダメージの種類を取得</summary>
        public ResistanceType currentResistanceType
        {
            get { return _resistanceType; }
        }
        
        /// <summary>ダメージ値を取得</summary>
        public int currentSubtractLife
        {
            get { return subtractLife; }
        }
    }
}
