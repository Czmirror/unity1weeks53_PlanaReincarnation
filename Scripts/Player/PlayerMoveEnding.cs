using System;
using System.Collections;
using Scripts.Event;
using UniRx;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerMoveEnding : MonoBehaviour
    {
        private ReactiveProperty<bool> isEnding =  new ReactiveProperty<bool>(false);
        [SerializeField] public IObservable<bool> isEndingObservable => isEnding;

        /// <summary>フェード時のカラー</summary>
        [SerializeField] private Color fadeColor = Color.white;

        /// <summary>フェード中の透明度</summary>
        [SerializeField] private float fadeAlpha = 0;

        /// <summary>フェードアウトの時間</summary>
        [SerializeField] private float feedTime = 5;
        
        /// <summary>フェードアウトの時間</summary>
        [SerializeField] private float endingTime = 25;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<MoveEnding>() is var targetWarp && targetWarp != null)
            {
                StartCoroutine(MoveEnding(targetWarp));
            }
        }
        
        private void OnGUI()
        {
            this.fadeColor.a = this.fadeAlpha;
            GUI.color = this.fadeColor;
            GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            
        }
        
        private IEnumerator MoveEnding(MoveEnding other)
        {
            // エンディング遷移前にプレエンディングに移動
            transform.position = other.destinationGameObjectPosition;
            
            // 画面明転
            float time = 0;
            float interval = feedTime;
            while (time <= interval) {
                this.fadeAlpha = Mathf.Lerp (0f, 1f, time / interval);
                time += Time.deltaTime;
                yield return 0;
            }
            
            // エンディングへ移動
            transform.position = other.endingDestinationGameObjectPosition;
            
            // 画面戻し
            time = 0;
            while (time <= interval) {
                this.fadeAlpha = Mathf.Lerp (1f, 0f, time / interval);
                time += Time.deltaTime;
                yield return 0;
            }
            
            // 一定時間経過後、エンディングフラグ
            yield return new WaitForSeconds(endingTime);

            isEnding.Value = true;
        }
    }
}
