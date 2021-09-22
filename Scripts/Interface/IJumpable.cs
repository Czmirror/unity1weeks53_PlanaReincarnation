using Scripts.Player;
using UnityEngine;

namespace Scripts.Interface
{
    public interface IJumpable
    {
        void Setup(PlayerVitalStatus playerVitalStatus, Rigidbody2D rigidbody2D);
        void Jump();
    }
}
