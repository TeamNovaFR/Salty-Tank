using UnityEngine;

namespace SaltyTank.PlayerSystem
{
    public class MakeWay : MonoBehaviour
    {
        public float makeWayForce = 100f;
        public float makeWayRadius = 5f;

        public AudioSource audioSource;
        public AudioClip[] impactClips;

        void OnTriggerEnter(Collider collider)
        {
            Rigidbody rb = collider.transform.GetComponent<Rigidbody>();

            if (rb)
            {
                rb.AddExplosionForce(makeWayForce, transform.position, makeWayRadius);
                audioSource.PlayOneShot(impactClips[Random.Range(0, impactClips.Length)]);
                GameManager.instance.totalCarSmashed++;
            }
        }
    }
}