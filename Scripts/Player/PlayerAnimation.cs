using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using UniRx;
using UnityEngine.Serialization;

namespace Scripts.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        PlayableGraph graph;
        private AnimationClipPlayable currentClipPlayable;
        private AnimationPlayableOutput playableOutput;

        [SerializeField] AnimationClip animationClipIdle;
        [SerializeField] AnimationClip animationClipWalk;
        [SerializeField] AnimationClip animationClipJumpUp;
        [SerializeField] AnimationClip animationClipJumpDown;
        [SerializeField] AnimationClip animationClipHeal;
        [SerializeField] AnimationClip animationClipDamage;
        [SerializeField] AnimationClip animationClipPinch;
        [SerializeField] AnimationClip animationClipDown;

        [FormerlySerializedAs("playerVitalStatus")]
        public PlayerVitalStateReactiveProperty playerVitalStatus;

        [SerializeField] private PlayerVitalStatus _playerVitalStatus;
        [SerializeField] private PlayerAnimationStatus _playerAnimationStatus;

        void Awake()
        {
            graph = PlayableGraph.Create();
        }

        private void Start()
        {
            // outputを生成して、出力先を自身のAnimatorに設定
            playableOutput = AnimationPlayableOutput.Create(graph, "output", GetComponent<Animator>());

            PlayerAnimataionIdle();

            // 主人公の生命ステータス監視用
            if (!_playerVitalStatus)
            {
                _playerVitalStatus = GetComponent<PlayerVitalStatus>();
            }

            if (!_playerAnimationStatus)
            {
                _playerAnimationStatus = GetComponent<PlayerAnimationStatus>();
            }


            // 主人公やられアニメーション監視
            _playerVitalStatus.playerVitalState.Where(x =>
                    x == PlayerVitalState.Down
                )
                .Subscribe(_ => PlayerAnimationDown());

            _playerAnimationStatus.playerAnimationState.Where(x =>
                x == PlayerAnimationState.Idle
            ).Subscribe(_ => PlayerAnimataionIdle());
            
            _playerAnimationStatus.playerAnimationState.Where(x =>
                x == PlayerAnimationState.Walk
            ).Subscribe(_ => PlayerAnimationWalk());
            
            _playerAnimationStatus.playerAnimationState.Where(x =>
                x == PlayerAnimationState.JumpUp
            ).Subscribe(_ => PlayerAnimationJumpUp());
            
            _playerAnimationStatus.playerAnimationState.Where(x =>
                x == PlayerAnimationState.JumpDown
            ).Subscribe(_ => PlayerAnimationJumpDown());
            
            _playerAnimationStatus.playerAnimationState.Where(x =>
                x == PlayerAnimationState.Heal
            ).Subscribe(_ => PlayerAnimationHeal());
            
            _playerAnimationStatus.playerAnimationState.Where(x =>
                x == PlayerAnimationState.Damage
            ).Subscribe(_ => PlayerAnimationDamage());
            
            _playerAnimationStatus.playerAnimationState.Where(x =>
                x == PlayerAnimationState.Pinch
            ).Subscribe(_ => PlayerAnimationPinch());
            
            _playerAnimationStatus.playerAnimationState.Where(x =>
                x == PlayerAnimationState.Down
            ).Subscribe(_ => PlayerAnimationDown());
        }

        // アニメーション実行
        private void PlayAnimation()
        {
            // playableをoutputに流し込む
            playableOutput.SetSourcePlayable(currentClipPlayable);
            graph.Play();
        }

        // 主人公通常時アニメーション
        private void PlayerAnimataionIdle()
        {
            currentClipPlayable = AnimationClipPlayable.Create(graph, animationClipIdle);
            PlayAnimation();
        }

        // 主人公通歩行アニメーション
        private void PlayerAnimationWalk()
        {
            currentClipPlayable = AnimationClipPlayable.Create(graph, animationClipWalk);
            PlayAnimation();
        }

        // 主人公通ジャンプ上昇アニメーション
        private void PlayerAnimationJumpUp()
        {
            currentClipPlayable = AnimationClipPlayable.Create(graph, animationClipJumpUp);
            PlayAnimation();
        }

        // 主人公通ジャンプ下降アニメーション
        private void PlayerAnimationJumpDown()
        {
            currentClipPlayable = AnimationClipPlayable.Create(graph, animationClipJumpDown);
            PlayAnimation();
        }

        // 主人公やられアニメーション
        private void PlayerAnimationHeal()
        {
            currentClipPlayable = AnimationClipPlayable.Create(graph, animationClipHeal);
            PlayAnimation();
        }
        
        private void PlayerAnimationDamage()
        {
            currentClipPlayable = AnimationClipPlayable.Create(graph, animationClipDamage);
            PlayAnimation();
        }
        
        private void PlayerAnimationPinch()
        {
            currentClipPlayable = AnimationClipPlayable.Create(graph, animationClipPinch);
            PlayAnimation();
        }
        
        private void PlayerAnimationDown()
        {
            currentClipPlayable = AnimationClipPlayable.Create(graph, animationClipDown);
            PlayAnimation();
        }
    }
}
