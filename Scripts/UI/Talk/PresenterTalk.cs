using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Talk
{
    public class PresenterTalk : MonoBehaviour
    {
        [SerializeField] private ReadJson _readJson;
        [SerializeField] private TMP_Text talkText;

        private void Start()
        {
            _readJson.talkObservable.DistinctUntilChanged().Subscribe(x =>
            {
                if (x != null)
                {
                    talkText.text = x;
                }
            });
        }
    }
}
