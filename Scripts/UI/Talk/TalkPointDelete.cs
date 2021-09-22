using UnityEngine;
using Scripts.Player;

namespace Scripts.UI.Talk
{
    public class TalkPointDelete : MonoBehaviour
    {

        // プレイヤーに触れたらトークポイントを削除
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerTalk>() is var targetTalk && targetTalk != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
