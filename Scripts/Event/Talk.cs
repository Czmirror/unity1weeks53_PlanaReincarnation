using UnityEngine;

namespace Scripts.Event
{
    public class Talk : MonoBehaviour
    {
        [SerializeField] private string talkFile;
        [SerializeField] private string talkText;

        public string currentTalkFile
        {
            get { return talkFile; }
        }

        public string currentTalkText
        {
            get { return talkText; }
        }
    }
}
