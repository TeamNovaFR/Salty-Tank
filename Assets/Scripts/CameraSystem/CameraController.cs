using UnityEngine;

namespace SaltyTank.CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;
        public float smooth = 1f;

        private void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), smooth);
        }
    }
}
