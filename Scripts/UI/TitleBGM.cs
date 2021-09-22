using UnityEngine;
using UniRx;

namespace Scripts.UI
{
    public class TitleBGM : MonoBehaviour
    {
        [SerializeField] private UIAlpha bgmPlayUI; // 表示後に音楽を再生するUI
        [SerializeField] private AudioSource _audioSource;
        
        private void Start()
        {
            bgmPlayUI.isVisible.DistinctUntilChanged().Where(x => x).Subscribe(_ => _audioSource.Play());
        }
    }
}
