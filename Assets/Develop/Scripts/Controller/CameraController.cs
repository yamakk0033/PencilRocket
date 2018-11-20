using UnityEngine;

namespace Assets.Controller
{
    [DisallowMultipleComponent]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        private Transform playerTran;
        private Vector3 position;

        private void Start()
        {
            playerTran = player.transform;
            position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        }

        private void LateUpdate()
        {
            position.y = playerTran.position.y;
            if (position.y < 0.0f) position.y = 0.0f;

            transform.position = position;
        }
    }

}
