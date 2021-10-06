using SaltyTank.PlayerSystem;
using UnityEngine;

namespace SaltyTank.ChunkSystem
{
    public class Path : MonoBehaviour
    {
        public Path nextNode;

        private void Start()
        {
            if(nextNode)
            {
                transform.LookAt(nextNode.transform.position);
            }
        }
    }
}