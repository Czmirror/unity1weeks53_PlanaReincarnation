using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using UniRx;

namespace Scripts.UI
{
    public class EndingPlanaAnimation : MonoBehaviour
    {
        [SerializeField] Animator animator;
        int hashStatePlanaEnding = Animator.StringToHash("PlanaEndingAnimation");

        [SerializeField] private GameObject titleStateObject;
        [SerializeField] private UIAlpha planaEndingUI;
        private void Start()
        {
            planaEndingUI.isVisible.Where(x => x).Subscribe(_ => Ending());
        }

        private void Ending()
        {
            animator.Play(hashStatePlanaEnding);
        }
    }
}
