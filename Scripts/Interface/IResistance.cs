using System;
using Scripts.Resistance;
using UniRx;

namespace Scripts.Interface
{
    public interface IResistance
    {
        void GetExperience(ResistanceType resistanceType);
        void LevelUp();
    }
}
