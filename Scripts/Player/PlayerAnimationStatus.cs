using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Serialization;

namespace Scripts.Player
{
    // 状態の種類のenum
    public enum PlayerAnimationState
    {
        Idle,
        Walk,
        JumpUp,
        JumpDown,
        Heal,
        Damage,
        Pinch,
        Down,
    }

    // 独自UniRxステータス
    [SerializeField]
    public class PlayerAnimationStateReactiveProperty : ReactiveProperty<PlayerAnimationState>
    {
        public PlayerAnimationStateReactiveProperty()
        {
        }

        public PlayerAnimationStateReactiveProperty(PlayerAnimationState init) : base(init)
        {
        }
    }

    // プレイヤー状態
    public class PlayerAnimationStatus : MonoBehaviour
    {
        [SerializeField] private GameObject gameStatusObject;
        [SerializeField] private PlayerLocomotion _playerLocomotion;
        [SerializeField] private PlayerJump _playerJump;
        [SerializeField] private PlayerHeal _playerHeal;
        [SerializeField] private PlayerDamage _playerDamage;
        [SerializeField] private PlayerVitalStatus _playerVitalStatus;

        // エディタで把握するためのパラメーター
        [FormerlySerializedAs("currentPlayerAnimationState")] [SerializeField]
        private PlayerAnimationState currentPlayerAnimationState;

        public PlayerAnimationState CurrentAnimationState
        {
            get { return playerAnimationState.Value; }
        }

        [FormerlySerializedAs("playerAnimationState")]
        public PlayerAnimationStateReactiveProperty playerAnimationState = new PlayerAnimationStateReactiveProperty();

        private void Start()
        {
            if (!gameStatusObject)
            {
                gameStatusObject = GameObject.Find("Managers/GameStatus");
            }

            if (!_playerLocomotion)
            {
                _playerLocomotion = GetComponent<PlayerLocomotion>();
            }

            if (!_playerJump)
            {
                _playerJump = GetComponent<PlayerJump>();
            }

            // 静止
            this.UpdateAsObservable()
                .Where(_ =>
                    _playerVitalStatus.playerVitalState.Value != PlayerVitalState.Down && 
                        _playerLocomotion.currentSpeed.Value < 1 
                        && _playerJump.currentHeight.Value == 0
                )
                .Subscribe(_ => PlayerIdle());

            // 歩行
            this.UpdateAsObservable()
                .Where(_ =>
                    _playerVitalStatus.playerVitalState.Value != PlayerVitalState.Down && 
                    _playerLocomotion.currentSpeed.Value >= 1 
                    && _playerJump.currentHeight.Value == 0
                )
                .Subscribe(_ => PlayerWalk());

            // ジャンプアップ
            this.UpdateAsObservable()
                .Where(_ =>
                    _playerVitalStatus.playerVitalState.Value != PlayerVitalState.Down && 
                    _playerJump.currentHeight.Value > 0
                )
                .Subscribe(_ => PlayerJumpUp());

            // ジャンプダウン
            this.UpdateAsObservable()
                .Where(_ =>
                    _playerVitalStatus.playerVitalState.Value != PlayerVitalState.Down && 
                    _playerJump.currentHeight.Value < 0
                )
                .Subscribe(_ => PlayerJumpDown());

            // 回復
            this.UpdateAsObservable()
                .Where(_ =>
                    _playerVitalStatus.playerVitalState.Value != PlayerVitalState.Down && 
                    _playerHeal.isHeal.Value
                )
                .Subscribe(_ => PlayerHeal());
            
            // ダメージ
            this.UpdateAsObservable()
                .Where(_ =>
                    _playerVitalStatus.playerVitalState.Value != PlayerVitalState.Down && 
                    _playerDamage.isDamage.Value
                )
                .Subscribe(_ => PlayerDamage());
            
            // ダウン
            this.UpdateAsObservable()
                .Where(_ =>
                    _playerVitalStatus.playerVitalState.Value == PlayerVitalState.Down
                )
                .Subscribe(_ => PlayerDown());
            
//            _playerLocomotion.currentSpeedObservable.DistinctUntilChanged().Where(x => x < 0.1)
//                .Subscribe(_ => PlayerIdle());
//            _playerLocomotion.currentSpeedObservable.DistinctUntilChanged().Where(x => x > 1)
//                .Subscribe(_ => PlayerWalk());
//            _playerJump.currentHeightObservable.DistinctUntilChanged().Where(x => x > 0).Subscribe(_ => PlayerJumpUp());
//            _playerJump.currentHeightObservable.DistinctUntilChanged().Where(x => x < 0).Subscribe(_ => PlayerJumpDown());
//            _playerHeal.isHealObservable.DistinctUntilChanged().Subscribe(_ => PlayerHeal());
        }

        async UniTask StatusCheck(CancellationToken token)
        {
//            await UniTask.WaitUntil(() =>
//            {
//                _playerJump.currentHeight.Value < 0;
//                PlayerJumpDown();
//            }, cancellationToken: token);
//            
//            await UniTask.WaitUntil(() =>
//            {
//                _playerJump.currentHeight.Value > 0;
//                PlayerJumpDown();
//            }, cancellationToken: token);


            StatusCheck(token).Forget();
        }

        private void PlayerChangeStatus()
        {
            if (playerAnimationState.Value == PlayerAnimationState.Down)
            {
                PlayerDown();
                return;
            }

            // 通常
            PlayerIdle();

            // エディタで把握するためのパラメーター
            currentPlayerAnimationState = playerAnimationState.Value;
        }

        public void PlayerIdle()
        {
//            if (playerAnimationState.Value == PlayerAnimationState.Walk)
//            {
//                return;
//            }
//            if (playerAnimationState.Value == PlayerAnimationState.JumpUp)
//            {
//                return;
//            }
//            if (playerAnimationState.Value == PlayerAnimationState.JumpDown)
//            {
//                return;
//            }
//            if (playerAnimationState.Value == PlayerAnimationState.Heal)
//            {
//                return;
//            }

            if (_playerVitalStatus.playerVitalState.Value == PlayerVitalState.Pinch)
            {
                playerAnimationState.Value = PlayerAnimationState.Pinch;
            }
            else if (_playerHeal.isHeal.Value == true)
            {
                PlayerHeal();
            }
            else
            {
                playerAnimationState.Value = PlayerAnimationState.Idle;
            }
        }

        public void PlayerWalk()
        {
            playerAnimationState.Value = PlayerAnimationState.Walk;
        }

        public void PlayerJumpUp()
        {
            playerAnimationState.Value = PlayerAnimationState.JumpUp;
        }

        public void PlayerJumpDown()
        {
            // 稀にジャンプダウンのモーションが解除されないため、判定を行う。
            if (_playerJump.currentHeight.Value < 0)
            {
                playerAnimationState.Value = PlayerAnimationState.JumpDown;
                
            }
            else
            {
                PlayerIdle();
            }
        }

        public void PlayerHeal()
        {
            playerAnimationState.Value = PlayerAnimationState.Heal;
        }
        
        public void PlayerDamage()
        {
            playerAnimationState.Value = PlayerAnimationState.Damage;
        }
        
        public void PlayerPinch()
        {
            playerAnimationState.Value = PlayerAnimationState.Pinch;
        }

        public void PlayerDown()
        {
            playerAnimationState.Value = PlayerAnimationState.Down;
        }
    }
}
