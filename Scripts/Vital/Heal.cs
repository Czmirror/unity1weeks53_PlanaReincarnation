using UnityEngine;

namespace Scripts.Vital
{
    public class Heal : MonoBehaviour
    {
        /// <summary>回復値</summary>
        [SerializeField] private int addLife = 1;

        /// <summary>回復値のプロパティ</summary>
        public int currentAddLife
        {
            get { return addLife; }
        }
    }
}
