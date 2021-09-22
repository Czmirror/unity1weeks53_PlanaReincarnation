using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.Event
{
    public class Warp : MonoBehaviour
    {
        [SerializeField] private Vector3 warpPosition;
        [SerializeField] private GameObject destinationObject;

        public Vector3 CurrentWarpPosition
        {
            get { return warpPosition; }
        }

        public Vector3 destinationGameObjectPosition
        {
            get { return destinationObject.transform.position; }
        }
    }
}
