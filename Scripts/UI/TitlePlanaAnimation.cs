using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using UniRx;

namespace Scripts.UI
{
    public class TitlePlanaAnimation : MonoBehaviour
    {
        [SerializeField] Animator animator;
        int hashStatePlanaWink = Animator.StringToHash("PlanaWink");

        [SerializeField] private GameObject titleStateObject;

        public TitleStateReactiveProperty titleStatus;

        private void Start()
        {
            var gameStatus = titleStateObject.GetComponent<TitleStatus>();

            gameStatus.titleState
                .Where(x =>
                    x == TitleState.PushedStartButton)
                .Subscribe(_ => Wink());
        }

        private void Wink()
        {
            animator.Play(hashStatePlanaWink);
        }
    }
}
