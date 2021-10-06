using UnityEngine;
using SaltyTank.CameraSystem;

namespace SaltyTank.ChunkSystem
{
    public class Chunk : MonoBehaviour
    {
        public Transform startPosition;
        public Transform endPosition;

        public Animator leftTarget,
            centerTarget,
            rightTarget;

        public Animator door;

        public bool randomTargetActivation = true;
        public bool isDoorOpened;

        public enum TargetPosition
        {
            Left,
            Center,
            Right,
            All
        }

        private void Start()
        {
            if(randomTargetActivation)
            {
                leftTarget.gameObject.SetActive(false);
                centerTarget.gameObject.SetActive(false);
                rightTarget.gameObject.SetActive(false);

                int rand = Random.Range(1, 4);

                if (rand == 1)
                    leftTarget.gameObject.SetActive(true);
                else if (rand == 2)
                    centerTarget.gameObject.SetActive(true);
                else
                    rightTarget.gameObject.SetActive(true);
            }
        }

        public void SetTarget(TargetPosition targetPosition, bool status)
        {
            // maybe this chunk doesn't have door to open?
            if (!door)
                return;

            switch(targetPosition)
            {
                case TargetPosition.Left:
                    if(leftTarget)
                    {
                        if (leftTarget.gameObject.activeSelf)
                        {
                            leftTarget.SetBool("isTrigger", status);
                            GameManager.instance.player.LookAt(leftTarget.transform);
                        }
                    }
                    break;
                case TargetPosition.Center:
                    if(centerTarget)
                    {
                        if (centerTarget.gameObject.activeSelf)
                        {
                            centerTarget.SetBool("isTrigger", status);
                            GameManager.instance.player.LookAt(centerTarget.transform);
                        }
                    }
                    break;
                case TargetPosition.Right:
                    if(rightTarget)
                    {
                        if (rightTarget.gameObject.activeSelf)
                        {
                            rightTarget.SetBool("isTrigger", status);
                            GameManager.instance.player.LookAt(rightTarget.transform);
                        }
                    }
                    break;
                case TargetPosition.All:
                    if(leftTarget)
                    {
                        if (leftTarget.gameObject.activeSelf)
                        {
                            leftTarget.SetBool("isTrigger", status);
                            GameManager.instance.player.LookAt(leftTarget.transform);
                        }
                    }
                    
                    if(centerTarget)
                    {
                        if (centerTarget.gameObject.activeSelf)
                        {
                            centerTarget.SetBool("isTrigger", status);
                            GameManager.instance.player.LookAt(centerTarget.transform);
                        }
                    }
                    
                    if(rightTarget)
                    {
                        if (rightTarget.gameObject.activeSelf)
                        {
                            rightTarget.SetBool("isTrigger", status);
                            GameManager.instance.player.LookAt(rightTarget.transform);
                        }
                    }
                    
                    break;
            }

            CheckForDoorState();
        }

        void CheckForDoorState()
        {
            bool doorState = true;

            if(leftTarget)
                if (leftTarget.gameObject.activeSelf && !leftTarget.GetBool("isTrigger"))
                doorState = false;

            if(centerTarget)
                if (centerTarget.gameObject.activeSelf && !centerTarget.GetBool("isTrigger"))
                doorState = false;

            if(rightTarget)
                if (rightTarget.gameObject.activeSelf && !rightTarget.GetBool("isTrigger"))
                doorState = false;

            isDoorOpened = doorState;

            if (doorState && !door.GetBool("isTrigger"))
            {
                CameraShake.instance.shakeDuration = 0.2f;
            }

            door.SetBool("isTrigger", doorState);
        }
    }
}