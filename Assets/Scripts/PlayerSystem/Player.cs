using UnityEngine;
using SaltyTank.ChunkSystem;
using UnityEngine.Events;

namespace SaltyTank.PlayerSystem
{
    public class Player : MonoBehaviour
    {
        #region Public Vars

        [Header("Player Configuration")]
        public Rigidbody rb;
        public float speed = 5f;
        public float speedFactor = 1f;
        public float motorTorque = 100f;
        public Transform turret;
        public AudioSource idleSource;
        public Transform centerOfMass;
        public float rotationY = -180f;

        public float maxTimeOutOfRoad = 0.05f;

        [Header("Input Configuration")]
        public bool detectSwipeOnlyAfterRelease = false;
        public float swipeThreshold = 20f;

        [Header("Debug Only")]
        public bool isOnRoad;
        public bool isDead = true;
        public bool isGrounded;
        public bool devMode;

        [Header("Events")]
        public UnityAction<Road> OnPlayerEnterRoadAction;

        #endregion

        #region Private Vars 

        private GameManager manager;

        private Vector2 fingerDown;
        private Vector2 fingerUp;
        private Vector3 newPosition;
        private int devModeCounter;

        #endregion

        private void Start()
        {
            manager = GameManager.instance;
            OnPlayerEnterRoadAction += OnPlayerEnterRoad;
            rb.centerOfMass = centerOfMass.localPosition;
        }

        private void FixedUpdate()
        {
            if(!isDead)
            {
                if(newPosition != Vector3.zero)
                {
                    if(rotationY == 180)
                        rb.MovePosition(new Vector3(0f, transform.position.x, transform.position.z));
                    else
                        rb.MovePosition(new Vector3(newPosition.x, transform.position.y, transform.position.z));

                    newPosition = Vector3.zero;
                }else
                {
                    Vector3 euler = transform.eulerAngles;
                    euler.y = rotationY;
                    if (euler.x > 180) euler.x = euler.x - 360;
                    euler.x = Mathf.Clamp(euler.x, 0f, 30f);
                    transform.eulerAngles = euler;

                    rb.MovePosition(transform.position + transform.forward * (speed * Time.fixedDeltaTime));
                }
            }
        }

        void OnCollisionStay(Collision collisionInfo)
        {
            isGrounded = true;
        }

        void OnCollisionExit(Collision collisionInfo)
        {
            isGrounded = false;
        }

        private void Update()
        {
            if(!isDead)
            {
                if (!idleSource.isPlaying)
                    idleSource.Play();
            }else
            {
                if (idleSource.isPlaying)
                    idleSource.Stop();
            }

            // Debug Only
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                manager.currentChunk.SetTarget(Chunk.TargetPosition.Left, true);
            }else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                manager.currentChunk.SetTarget(Chunk.TargetPosition.Center, true);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                manager.currentChunk.SetTarget(Chunk.TargetPosition.Right, true);
            }else if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                OnSwipeDown();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                isDead = !isDead;
            }

            // Mobile Inputs
            if(!isDead)
                InputsLoop();
        }
        
        public void LookAt(Transform t)
        {
            Vector3 lookPos = t.position - turret.position;
            lookPos.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookPos);
            turret.rotation = rotation;
            turret.localEulerAngles = new Vector3(0, turret.localEulerAngles.y, 0);
        }

        void OnPlayerFall()
        {
            isDead = true;
            manager.OnPlayerLoose();
        }

        void OnPlayerExitRoad(Road road)
        {
        }

        void OnPlayerEnterRoad(Road road)
        {
            if(devMode)
            {
                manager.currentChunk.SetTarget(Chunk.TargetPosition.Center, true);
                manager.currentChunk.SetTarget(Chunk.TargetPosition.Right, true);
                manager.currentChunk.SetTarget(Chunk.TargetPosition.Left, true);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            switch(other.tag)
            {
                case "Road":
                    OnPlayerEnterRoadAction.Invoke(other.GetComponent<Road>());
                    break;
                case "Path":
                    rotationY = other.transform.eulerAngles.y;
                    newPosition = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
                    break;
                case "Fall":
                    OnPlayerFall();
                    break;
            }

        }

        private void OnTriggerStay(Collider other)
        {
            switch (other.tag)
            {
                case "Road":
                    isOnRoad = true;
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Road"))
            {
                isOnRoad = false;
                OnPlayerExitRoad(other.GetComponent<Road>());
            }
        }

        #region Mobile Inputs

        void InputsLoop()
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fingerUp = touch.position;
                    fingerDown = touch.position;
                }

                //Detects Swipe while finger is still moving
                if (touch.phase == TouchPhase.Moved)
                {
                    if (!detectSwipeOnlyAfterRelease)
                    {
                        fingerDown = touch.position;
                        CheckSwipe();
                    }
                }

                //Detects swipe after finger is released
                if (touch.phase == TouchPhase.Ended)
                {
                    fingerDown = touch.position;
                    CheckSwipe();
                }
            }
        }

        void CheckSwipe()
        {
            //Check if Vertical swipe
            if (verticalMove() > swipeThreshold && verticalMove() > horizontalValMove())
            {
                //Debug.Log("Vertical");
                if (fingerDown.y - fingerUp.y > 0)//up swipe
                {
                    OnSwipeUp();
                }
                else if (fingerDown.y - fingerUp.y < 0)//Down swipe
                {
                    OnSwipeDown();
                }
                fingerUp = fingerDown;
            }

            //Check if Horizontal swipe
            else if (horizontalValMove() > swipeThreshold && horizontalValMove() > verticalMove())
            {
                //Debug.Log("Horizontal");
                if (fingerDown.x - fingerUp.x > 0)//Right swipe
                {
                    OnSwipeRight();
                }
                else if (fingerDown.x - fingerUp.x < 0)//Left swipe
                {
                    OnSwipeLeft();
                }
                fingerUp = fingerDown;
            }
        }

        float verticalMove()
        {
            return Mathf.Abs(fingerDown.y - fingerUp.y);
        }

        float horizontalValMove()
        {
            return Mathf.Abs(fingerDown.x - fingerUp.x);
        }

        void OnSwipeUp()
        {
            manager.currentChunk.SetTarget(Chunk.TargetPosition.Center, true);
        }

        void OnSwipeDown()
        {
            devModeCounter++;

            if(devModeCounter == 10)
            {
                devMode = true;
                manager.ui.PopNewAchievement("Mode développeur activé !");
            }

            if(devModeCounter == 20)
            {
                Time.timeScale = 5f;
                manager.ui.PopNewAchievement("Temps accéléré !");
            }

            if(devModeCounter == 30)
            {
                manager.ui.PopNewAchievement("Mode développeur désactivé !");
                devMode = false;
                Time.timeScale = 1f;
                devModeCounter = 0;
            }
        }

        void OnSwipeLeft()
        {
            manager.currentChunk.SetTarget(Chunk.TargetPosition.Left, true);
        }

        void OnSwipeRight()
        {
            manager.currentChunk.SetTarget(Chunk.TargetPosition.Right, true);
        }

        #endregion
    }
}