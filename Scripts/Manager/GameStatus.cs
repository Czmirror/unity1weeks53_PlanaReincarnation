using UnityEngine;
using UniRx;
using Scripts.Player;

namespace Scripts.Manager
{
    // 状態の種類のenum
    public enum GameState
    {
        Normal,
        GameOver,
        GameClear
    }

    // 独自UniRxステータス
    [SerializeField]
    public class GameStateReactiveProperty : ReactiveProperty<GameState>
    {
        public GameStateReactiveProperty()
        {
        }

        public GameStateReactiveProperty(GameState init) : base(init)
        {
        }
    }

    public class GameStatus : MonoBehaviour
    {
        public GameState currentState
        {
            get { return gameState.Value; }
        }

        public GameStateReactiveProperty gameState = new GameStateReactiveProperty();

        public void Normal()
        {
            gameState.Value = GameState.Normal;
        }

        public void GameOver()
        {
            gameState.Value = GameState.GameOver;
        }

        public void GameClear()
        {
            gameState.Value = GameState.GameClear;
        }


        [SerializeField] private GameObject player;
        [SerializeField] private GameObject tresureboxCount;

        private void Start()
        {
            Normal();

            if (!player)
            {
                player = GameObject.Find("Player");
            }

            var playerStatus = player.GetComponent<PlayerVitalStatus>();
            playerStatus.playerVitalState
                .Where(x =>
                    x == PlayerVitalState.Down
                )
                .Subscribe(_ => GameOver());

        }
    }
}
