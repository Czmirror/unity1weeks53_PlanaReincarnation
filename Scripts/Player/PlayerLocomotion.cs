using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;

namespace Scripts.Player
{
    public class PlayerLocomotion : MonoBehaviour
    {
        [SerializeField] private float moveSpeed; // 移動スピード
        [SerializeField] private Vector3 latestPosition; // 速度計算用最終座標

        public ReactiveProperty<float> currentSpeed = new ReactiveProperty<float>(0); // 現在の加速
        [SerializeField] public IObservable<float> currentSpeedObservable => currentSpeed;
        
        [SerializeField] private PlayerVitalStatus _playerVitalStatus;
        
        async UniTaskVoid Start()
        {
            Locomotion(this.GetCancellationTokenOnDestroy()).Forget();
        }

        /**
         * プレイヤーの現在速度をセット
         */
        private void SetSpeed()
        {
            var position = new Vector3(transform.position.x, 0, transform.position.z);
            var speed = ((position - latestPosition) / Time.deltaTime).magnitude;
            latestPosition = position;
            currentSpeed.Value = speed;
        }

        /**
         * 移動処理
         */
        async UniTask Locomotion(CancellationToken token)
        {
            SetSpeed();
            await UniTask.WaitUntil(() => { return (Input.GetAxis("Horizontal") != 0); });
            var x = Input.GetAxis("Horizontal") * moveSpeed;

            if (x != 0 && _playerVitalStatus.playerVitalState.Value != PlayerVitalState.Down)
            {
                var direction = new Vector3(x, 0, 0);
                transform.position += new Vector3(x * Time.deltaTime, 0, 0);
            }

            Locomotion(token).Forget();
        }
    }
}
