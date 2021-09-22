using UnityEngine;

namespace Scripts.UI
{
    public class EndingUIActive : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObject;
        private void Start()
        {
            _gameObject.SetActive(true);
        }
    }
}
