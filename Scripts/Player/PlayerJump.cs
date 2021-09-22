using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Scripts.Player
{
    public class PlayerJump : MonoBehaviour
    {
        [SerializeField] private float jumpPower = 300.0f; //ジャンプ力
        public ReactiveProperty<bool> isJump = new ReactiveProperty<bool>(false); //ジャンプフラグ

        private Rigidbody2D rigidbody2D;
        
        [SerializeField] private Vector3 latestPosition; // 速度計算用最終座標

        public ReactiveProperty<float> currentHeight = new ReactiveProperty<float>(0); // 現在の加速
        [SerializeField] public IObservable<float> currentHeightObservable => currentHeight;
        [SerializeField] private PlayerVitalStatus _playerVitalStatus;

        private void Start()
        {
            _playerVitalStatus = this.GetComponent<PlayerVitalStatus>();
            rigidbody2D = this.GetComponent<Rigidbody2D>();

            // ジャンプアクション
            this.UpdateAsObservable()
                .Where(_ =>
                    (
                        (Input.GetKeyUp("space"))
                    ) &&
                    _playerVitalStatus.playerVitalState.Value != PlayerVitalState.Down &&
                    isJump.Value == false
                )
                .Subscribe(_ => Jump());

            // ジャンプフラグのセット
            this.UpdateAsObservable()
                .Where(_ =>
                    rigidbody2D.velocity.y > 0 &&
                    isJump.Value == false
                )
                .Subscribe(_ => IsJumpReset(true));

            // ジャンプフラグのリセット
            this.UpdateAsObservable()
                .Where(_ =>
                    rigidbody2D.velocity.y == 0 &&
                    isJump.Value == true
                )
                .Subscribe(_ => IsJumpReset(false));
        }

        private void Update()
        {
            SetSpeed();
        }

        /**
         * プレイヤーの現在速度をセット
         */
        private void SetSpeed()
        {
            currentHeight.Value = rigidbody2D.velocity.y;
        }

        private void Jump()
        {
            rigidbody2D.AddForce(transform.up * jumpPower);
        }

        // ジャンプフラグの更新
        private void IsJumpReset(bool currentJump)
        {
            isJump.Value = currentJump;
        }
    }
}
