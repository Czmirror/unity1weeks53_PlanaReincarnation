using System.Collections;
using Scripts.UI.Talk;
using UniRx;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerRespawn : MonoBehaviour
    {
        /// <summary>リスポーンポジション</summary>
        [SerializeField] private Vector3 resapwnPosition;
        /// <summary>プレイヤーの生存状態</summary>
        [SerializeField] private PlayerVitalStatus _playerVitalStatus;
        /// <summary>プレイヤーの回復状態</summary>
        [SerializeField] private PlayerHeal _playerHeal;

        /// <summary>フェード時のカラー</summary>
        [SerializeField] private Color fadeColor = Color.black;
        /// <summary>フェード中の透明度</summary>
        [SerializeField] private float fadeAlpha = 0;
        /// <summary>リスポーンの時間</summary>
        [SerializeField] private float respawnTime = 5;

        private void Start()
        {
            SetRespawn();

            // 回復の泉に触れるとリスポーン地点として更新
            _playerHeal.isHealObservable.Where(x => x).Subscribe(_ => { SetRespawn(); });
            
            // プレイヤーがダウンした場合リスポーン処理
            _playerVitalStatus.playerVitalState.Where(x =>
                    x == PlayerVitalState.Down
                )
                .Subscribe(_ =>
                {
                    StartCoroutine (FadeRespawn ());
                });
        }
        
        private void OnGUI()
        {
            this.fadeColor.a = this.fadeAlpha;
            GUI.color = this.fadeColor;
            GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            
        }

        // リスポーン地点の更新
        private void SetRespawn()
        {
            resapwnPosition = transform.position;
        }
        
        // リスポーン地点への移動
        private void moveRespawn()
        {
            transform.position = resapwnPosition;
        }

        private IEnumerator FadeRespawn()
        {
            // 画面暗転
            float time = 0;
            float interval = respawnTime;
            while (time <= interval) {
                this.fadeAlpha = Mathf.Lerp (0f, 1f, time / interval);
                time += Time.deltaTime;
                yield return 0;
            }
            
            // リスポーン位置へ移動
            moveRespawn();
            
            // 画面戻し
            time = 0;
            while (time <= interval) {
                this.fadeAlpha = Mathf.Lerp (1f, 0f, time / interval);
                time += Time.deltaTime;
                yield return 0;
            }
        }
    }
}
