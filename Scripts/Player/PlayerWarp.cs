using System.Collections;
using Scripts.Event;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerWarp : MonoBehaviour
    {

        /// <summary>移動可能フラグ</summary>
        [SerializeField] private bool isEnter;
        /// <summary>移動可能になるインターバル</summary>
        [SerializeField] private float interval = 1.0f;
        
        /// <summary>ワープポイントに触れた際に指定された場所へ移動</summary>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Warp>() is var targetWarp && targetWarp != null && isEnter == false)
            {
                transform.position = targetWarp.destinationGameObjectPosition;
                
                StartCoroutine(Warp());
            }
        }

        /// <summary>移動処理後のコルーチン処理、移動可能フラグの制御</summary>
        private IEnumerator Warp()
        {
            isEnter = true;
            yield return new WaitForSeconds(interval);
            isEnter = false;
        }
    }
}
