using UnityEngine;

namespace Assets.Controller
{
    [DisallowMultipleComponent]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private GameObject PlayerObject;
        private Transform playerTran;
        private Vector3 pos;

        private void Start()
        {
            playerTran = PlayerObject.transform;
            pos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        }

        private void LateUpdate()
        {
            pos.y = playerTran.position.y;
            if (pos.y < 0.0f) pos.y = 0.0f;

            transform.position = pos;
        }
    }

}
