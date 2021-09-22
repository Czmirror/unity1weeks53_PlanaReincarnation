using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

namespace Scripts.UI
{
    public class UIFillAmount : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float filloutSpeed = 0.02f;
        [SerializeField] private float delayTime = 1;

        public ReactiveProperty<bool> isVisible = new ReactiveProperty<bool>(false); // 表示フラグ

        private async void Start()
        {
            // 画像非表示
            _image.fillAmount = 0;
            
            // 待機処理
            var token = this.GetCancellationTokenOnDestroy();
            isVisible.Value = await UIViewWait(token);

            // FillOutが１ではない場合、FillOutを実施
            this.UpdateAsObservable()
                .Where(_ =>
                    _image.fillAmount < 1
                )
                .Subscribe(_ => FillOut());

            this.UpdateAsObservable()
                .Where(_ =>
                    isVisible.Value == false &&
                    _image.fillAmount == 1
                )
                .Subscribe(_ => isVisible.Value = true);
        }


        private async UniTask<bool> UIViewWait(CancellationToken token)
        {
            // 待機処理
            await UniTask.Delay((int) (delayTime * 1000), cancellationToken: token);

            return true;
        }

        private void FillOut()
        {
            _image.fillAmount += filloutSpeed;
        }
    }
}
