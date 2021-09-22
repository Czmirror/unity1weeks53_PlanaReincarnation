using Scripts.Resistance;
using Scripts.Vital;
using UnityEngine;
using Scripts.Interface;

namespace Scripts.Player
{
    public class PlayerResistance : MonoBehaviour
    {
        [SerializeField] private BaseResistance thornResistance;
        [SerializeField] private BaseResistance fireResistance;
        [SerializeField] private BaseResistance impactResistance;
        
        [SerializeField] private PlayerVitalStatus _playerVitalStatus;

        public int DamageResistance(Damage damage)
        {
            int registanceLevel = 0;
            var damageType = damage.currentResistanceType;
            switch (damageType)
            {
                case ResistanceType.Thorn:
                    return CalculationDamage(damage, thornResistance);
                    break;
                case ResistanceType.Fire:
                    return CalculationDamage(damage, fireResistance);
                    break;
                case ResistanceType.Impact:
                    return CalculationDamage(damage, impactResistance);
                    break;
            }

            // もしいずれの耐性にもヒットしなかった場合は0を返却
            return 0;

        }

        private int CalculationDamage(Damage damage, BaseResistance resistance)
        {
            int registanceLevel = resistance.CurrentLevel;
            
            // ダメージから耐性レベルを差し引く
            int deffensiveDamage = damage.currentSubtractLife - registanceLevel;
            if (deffensiveDamage < 0)
            {
                deffensiveDamage = 0;
            }

            // ダメージが１以上の場合、経験値取得
            if (deffensiveDamage > 0 && _playerVitalStatus.playerVitalState.Value != PlayerVitalState.Down)
            {
                resistance.GetExperience(damage.currentResistanceType);
            }

            return deffensiveDamage;
        }
    }
}
