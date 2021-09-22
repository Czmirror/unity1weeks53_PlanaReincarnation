using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Scripts.Interface;
using Scripts.Event;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerTalk : MonoBehaviour, ITalkable
    {
        private ReactiveProperty<Talk> talkData = new ReactiveProperty<Talk>(null);
        [SerializeField] public IObservable<Talk> IsTalkDataObservable => talkData;

        public Talk currentTalk
        {
            get { return talkData.Value; }
        }

        private void Start()
        {
            var ct = this.GetCancellationTokenOnDestroy();
            SetTalk(ct).Forget();
        }

        /**
         * 会話エリアでTalkクラスを受け取り、セットする
         */
        async UniTask SetTalk(CancellationToken ct)
        {
            // Talkクラスを持つオブジェクトと接触した場合処理
            var target = await this.GetAsyncTriggerEnter2DTrigger().OnTriggerEnter2DAsync(ct);
            if (target.gameObject.GetComponent<Talk>() is var targetTalk && targetTalk != null)
            {
                talkData.Value = targetTalk;
            }
//            else
//            {
//                // 背景のistriggerとバッティングする場合があるため、該当しなかった場合は処理をリロード
//                SetTalk(ct).Forget();
//            }

            if (ct.IsCancellationRequested)
            {
                throw new OperationCanceledException(ct);
            }

            SetTalk(ct).Forget();
        }
    }
}
