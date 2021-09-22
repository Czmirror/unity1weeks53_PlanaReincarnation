using System.Collections;
using System.Collections.Generic;
using Scripts.Interface;
using Scripts.Player;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Scripts.Action.Jump
{
    public class InitialJump : MonoBehaviour, IJumpable
    {
        [SerializeField] private float jumpPower = 300.0f; //ジャンプ力
        public ReactiveProperty<bool> isJump = new ReactiveProperty<bool>(false); //ジャンプフラグ

        private Rigidbody2D rigidbody2D;

        private void Start()
        {
//            Setup();
        }

        public void Setup(PlayerVitalStatus playerVitalStatus, Rigidbody2D rigidbody2D)
        {
//            var playerStatus = gameObject.GetComponent<PlayerStatus>();
//            rigidbody2D = this.GetComponent<Rigidbody2D>();

            // ジャンプアクション
            this.UpdateAsObservable()
                .Where(_ =>
                    (
                        (Input.GetKeyUp("space"))
                    ) &&
                    playerVitalStatus.playerVitalState.Value != PlayerVitalState.Down &&
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

        public void Jump()
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
