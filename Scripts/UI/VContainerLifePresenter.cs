using Scripts.Player;
using UniRx;
using UniRx.Triggers;
using VContainer.Unity;

namespace Scripts.UI
{
    public class VContainerLifePresenter : IPostInitializable
    {
        private int maxLifeValue;
        private readonly ViewLife _viewLife;
        private readonly PlayerVital _playerVital;

        public VContainerLifePresenter(ViewLife viewLife, PlayerVital playerVital)
        {
            _playerVital = playerVital;
            _viewLife = viewLife;
        }

        /// <summary>
        /// 初期化直後に呼ばれる
        /// </summary>
        public void PostInitialize()
        {
            _playerVital.maxLifeObservable.Subscribe(maxLife => maxLifeValue = maxLife);
            _playerVital.lifeObservable.Subscribe(life =>
            {
                float lifeRatio = (float) life / maxLifeValue;
                _viewLife.SetRatio(lifeRatio);
            });
        }
    }
}
