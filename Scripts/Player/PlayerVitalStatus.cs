using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Serialization;

namespace Scripts.Player
{
    // 状態の種類のenum
    public enum PlayerVitalState
    {
        Normal,
        Pinch,
        Down,
    }

    // 独自UniRxステータス
    [SerializeField]
    public class PlayerVitalStateReactiveProperty : ReactiveProperty<PlayerVitalState>
    {
        public PlayerVitalStateReactiveProperty()
        {
        }

        public PlayerVitalStateReactiveProperty(PlayerVitalState init) : base(init)
        {
        }
    }

    // プレイヤー状態
    public class PlayerVitalStatus : MonoBehaviour
    {
        [SerializeField] private PlayerVital _playerVital;
        [SerializeField] private GameObject gameStatusObject;

        [FormerlySerializedAs("pinchLife")] [SerializeField]
        private int pinchLife = 5;

        // エディタで把握するためのパラメーター
        [FormerlySerializedAs("currentPlayerVitalState")] [SerializeField]
        private PlayerVitalState currentPlayerVitalState;

        public PlayerVitalState CurrentVitalState
        {
            get { return playerVitalState.Value; }
        }

        [FormerlySerializedAs("playerVitalState")]
        public PlayerVitalStateReactiveProperty playerVitalState = new PlayerVitalStateReactiveProperty();

        private void Start()
        {
            if (!gameStatusObject)
            {
                gameStatusObject = GameObject.Find("Managers/GameStatus");
            }

            _playerVital.lifeObservable.DistinctUntilChanged().Subscribe(life => PlayerChangeStatus(life));
        }

        private void PlayerChangeStatus(int life)
        {
            if (life > pinchLife)
            {
                PlayerNormal();
                return;
            }

            if (life <= pinchLife && life > 0)
            {
                PlayerPinch();
                return;
            }

            if (life <= 0)
            {
                PlayerDown();
                return;
            }
        }

        private void PlayerNormal()
        {
            playerVitalState.Value = PlayerVitalState.Normal;
        }

        private void PlayerPinch()
        {
            playerVitalState.Value = PlayerVitalState.Pinch;
        }

        private void PlayerDown()
        {
            playerVitalState.Value = PlayerVitalState.Down;
        }
    }
}
