using System;
using System.IO;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Scripts.Player;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

namespace Scripts.UI.Talk
{
    public class ReadJson : MonoBehaviour
    {
        [SerializeField] private string jsonPath;
        [SerializeField] private TalkDataList _talkDataList;

        [SerializeField] private PlayerTalk _playerTalk;
        [SerializeField] private PlayerVitalStatus _playerVitalStatus;
        [SerializeField] private Event.Talk downTalk;

        [SerializeField] private RectTransform _talkWindow;
        [SerializeField] private float _talkWindowCuiinPosition = 120;
        [SerializeField] private float _talkWindowCuiinSpeed = 0.25f;

        public ReactiveProperty<string> characterImage = new ReactiveProperty<string>();
        [SerializeField] public IObservable<string> characterImageObservable => characterImage;
        public ReactiveProperty<string> talk = new ReactiveProperty<string>();
        [SerializeField] public IObservable<string> talkObservable => talk;

        private CancellationToken ct;

        /**
         * jsonパスをセットする
         */
        private void SetPath(string path)
        {
            jsonPath = Application.dataPath + "/" + path;
        }

        /**
         * json読み込み
         */
        private TalkDataList LoadJson()
        {
            StreamReader reader = new StreamReader(jsonPath);
            string jsonData = reader.ReadToEnd();
            reader.Close();

            return JsonUtility.FromJson<TalkDataList>(jsonData);
        }

        /**
         * 
         */
        private TalkDataList ReadJsonText(string text)
        {
            return JsonUtility.FromJson<TalkDataList>(text);
        }

        private async UniTask Start()
        {
            // Talkオブジェクトに接触し、Talkをプレイヤーが取得したら実施
            _playerTalk.IsTalkDataObservable.DistinctUntilChanged().Where(x => x).Subscribe(x =>
            {
                ct = this.GetCancellationTokenOnDestroy();
                TalkSet(ct, x).Forget();
            });

            // プレイヤーダウン時
            _playerVitalStatus.playerVitalState.DistinctUntilChanged()
                .Where(x => x == PlayerVitalState.Down)
                .Subscribe(
                    _ =>
                    {
                        ct = this.GetCancellationTokenOnDestroy();
                        TalkSet(ct, downTalk).Forget();
                    }
                );
        }

        private async UniTask TalkSet(CancellationToken ct, Event.Talk talkFile)
        {
            // json 読み込み
            _talkDataList = ReadJsonText(talkFile.currentTalkText);

            // talk window表示
            await _talkWindow.DOLocalMoveY(_talkWindowCuiinPosition, _talkWindowCuiinSpeed).SetEase(Ease.Linear);
            _talkWindow.DOLocalMoveY(_talkWindowCuiinPosition, _talkWindowCuiinSpeed).SetEase(Ease.Linear);

            // talk 表示
            foreach (var talkData in _talkDataList.TalkDatas)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                characterImage.Value = talkData.characterImage;
                talk.Value = talkData.talk;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(2));

            // talk window非表示
            _talkWindow.DOLocalMoveY((_talkWindowCuiinPosition * -1), _talkWindowCuiinSpeed).SetEase(Ease.Linear);

            characterImage.Value = "";
            talk.Value = "";

            if (ct.IsCancellationRequested)
            {
                throw new OperationCanceledException(ct);
            }
        }
    }
}
