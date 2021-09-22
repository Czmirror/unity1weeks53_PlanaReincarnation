using UnityEngine;
using UniRx;


namespace Scripts.UI
{
    // 状態の種類のenum
    public enum TitleState
    {
        Title,
        PushedStartButton
    }

    // 独自UniRxステータス
    [SerializeField]
    public class TitleStateReactiveProperty : ReactiveProperty<TitleState>
    {
        public TitleStateReactiveProperty()
        {
        }

        public TitleStateReactiveProperty(TitleState init) : base(init)
        {
        }
    }

    public class TitleStatus : MonoBehaviour
    {
        [SerializeField] private GameObject buttonStartObject;
        [SerializeField] private ButtonStart _buttonStart;
        
        public TitleState currentState
        {
            get { return titleState.Value; }
        }

        public TitleStateReactiveProperty titleState = new TitleStateReactiveProperty();

        public void Title()
        {
            titleState.Value = TitleState.Title;
        }

        public void PushedStartButton()
        {
            titleState.Value = TitleState.PushedStartButton;
        }

        private void Start()
        {
            Title();

            _buttonStart.isPush.DistinctUntilChanged().Where(x => x).Subscribe(_ => PushedStartButton());
        }
    }
}
