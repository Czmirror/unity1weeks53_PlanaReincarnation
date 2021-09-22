using UnityEngine;

namespace Scripts.Event
{
    public class MoveEnding : MonoBehaviour
    {
        [SerializeField] private GameObject destinationObject;
        [SerializeField] private GameObject endingDestinationObject;
        
        public Vector3 destinationGameObjectPosition
        {
            get { return destinationObject.transform.position; }
        }
        
        public Vector3 endingDestinationGameObjectPosition
        {
            get { return endingDestinationObject.transform.position; }
        }
    }
}
