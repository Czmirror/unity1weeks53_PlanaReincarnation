using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Scripts.Vital;
using UnityEngine;

namespace Scripts.BGM
{
    public class StageBGM : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerCore>() is var targetPlayer && targetPlayer != null)
            {
                _audioSource.Play();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerCore>() is var targetExitPlayer && targetExitPlayer != null)
            {
                _audioSource.Stop();
            }
        }
    }
}
