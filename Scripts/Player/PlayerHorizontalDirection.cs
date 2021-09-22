using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerHorizontalDirection : MonoBehaviour
    {
        [SerializeField] private float initialPlayerScale;
        [SerializeField] private PlayerVitalStatus _playerVitalStatus;
        
        void Start()
        {
            // プレイヤーの初期のScaleをセット
            initialPlayerScale = transform.localScale.x;
            SetDirection(this.GetCancellationTokenOnDestroy()).Forget();
        }

        async UniTask SetDirection(CancellationToken token)
        {
            await UniTask.WaitUntil(() => { return (Input.GetAxis("Horizontal") != 0); });
            var x = Input.GetAxis("Horizontal");

            Vector3 scale = transform.localScale;
            if (x > 0 && _playerVitalStatus.playerVitalState.Value != PlayerVitalState.Down)
            {
                // 右向き
                scale.x = initialPlayerScale;
            } else if (x < 0 && _playerVitalStatus.playerVitalState.Value != PlayerVitalState.Down)
            {
                // 左向き
                scale.x = initialPlayerScale * -1;
            }

            transform.localScale = scale;
            
            SetDirection(token).Forget();
        }
    }
}
