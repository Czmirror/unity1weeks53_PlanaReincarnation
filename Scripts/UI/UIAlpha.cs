using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using Scripts.Player;

namespace Scripts.UI
{
    public class UIAlpha : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float alphaSpeed = 0.02f;
        [SerializeField] private float delayTime = 1;
        [SerializeField] private int alphaOperator = 1; // 透明度の加算・減産のオペレーター
        [SerializeField] private float minAlpha = 0; // 透明度の最低値
        [SerializeField] private float maxAlpha = 1; // 透明度の最高値

        [SerializeField] private float currentAlpha; // 現在の透明度

        public ReactiveProperty<bool> isVisible = new ReactiveProperty<bool>(false); // 表示フラグ
        [SerializeField] private PlayerMoveEnding _playerMoveEnding;
        
        // 透明度のパターン
        public enum AlphaType
        {
            Loop, // ループ
            Once // 一度のみ
        }

        [SerializeField] private AlphaType _alphaType = AlphaType.Loop;

        private async void Start()
        {
            // 画像非表示
            _image.color = new Color(1.0f, 1.0f, 1.0f, 0);

            if (_playerMoveEnding != null)
            {
                _playerMoveEnding.isEndingObservable.DistinctUntilChanged().Where(x => x).Subscribe(_ => Setup());
            }
            else
            {
                Setup();
            }
        }

        private async void Setup()
        {
            // 待機処理
            var token = this.GetCancellationTokenOnDestroy();
            isVisible.Value = await UIViewWait(token);

            AlphaMinMaxCheck();

            // 画像表示
            _image.color = new Color(1.0f, 1.0f, 1.0f, currentAlpha);

            // 透明もしくは表示完了時に、透明度の演算子に−１乗算を反対にする
            this.UpdateAsObservable()
                .Where(_ =>
                    _image.color.a <= minAlpha && _alphaType == AlphaType.Loop
                )
                .Subscribe(_ => alphaOperator = 1);
            this.UpdateAsObservable()
                .Where(_ =>
                    _image.color.a >= maxAlpha && _alphaType == AlphaType.Loop
                )
                .Subscribe(_ => alphaOperator = -1);

            // 透明度の変更
            currentAlpha = _image.color.a;
            this.UpdateAsObservable()
                .Where(_ =>
                    (alphaOperator == -1 && currentAlpha > minAlpha) ||
                    (alphaOperator == 1 && currentAlpha < maxAlpha)
                )
                .Subscribe(_ =>
                {
                    currentAlpha = _image.color.a + (alphaSpeed * alphaOperator);
                    AlphaMinMaxCheck();

                    _image.color = new Color(1.0f, 1.0f, 1.0f, currentAlpha);
                });
        }

        private async UniTask<bool> UIViewWait(CancellationToken token)
        {
            // 待機処理
            await UniTask.Delay((int) (delayTime * 1000), cancellationToken: token);

            return true;
        }

        private void AlphaMinMaxCheck()
        {
            if (currentAlpha < minAlpha)
            {
                currentAlpha = minAlpha;
            }

            if (currentAlpha > maxAlpha)
            {
                currentAlpha = maxAlpha;
            }
        }
    }
}
