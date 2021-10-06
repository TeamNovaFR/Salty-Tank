using UnityEngine;
using SaltyTank.ChunkSystem;
using SaltyTank.PlayerSystem;
using SaltyTank.UISystem;
using SaltyTank.AchievementSystem;

namespace SaltyTank
{
    public class GameManager : MonoBehaviour
    {
        #region Public Vars

        public static GameManager instance;

        [Header("Game Configuration")]
        public float doorSpeed = 1f;
        public GameConfigSO gameConfig;
        public float musicPitchSmooth = 5f;
        public Vector3 basePlayerPosition = new Vector3(0f, 0f, -1.8f);

        [Header("References")]
        public Player player;
        public ChunkGeneration chunkGenerator;
        public AchievementManager achievement;
        public AudioSource musicSource;
        public AudioSource idleMusicSource;
        public UIManager ui;

        [Header("Debug Only")]
        public Chunk currentChunk;
        public bool isGameStarted;

        [Header("Stats")]
        public int numberOfDoorsPassed;
        public int totalDoorsPassed;
        public int totalCarSmashed;
        public int bux;

        #endregion


        #region Private Vars

        private bool isFirstStart = true;
        private float deltaTime = 0.0f;

        #endregion

        private void Awake()
        {
            Localization.Init();
            instance = this;
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
            player.OnPlayerEnterRoadAction += OnPlayerEnterRoad;
            SaveManager.RetrieveSave();
        }

        void OnPlayerEnterRoad(Road road)
        {
            Chunk roadChunk = road.roadChunk;
            if(currentChunk != roadChunk)
            {
                if(roadChunk == chunkGenerator.nextChunk)
                {
                    chunkGenerator.CreateNextChunk();
                    numberOfDoorsPassed++;
                    totalDoorsPassed++;
                    SaveManager.Save();
                    ui.SetScore();
                    achievement.CheckForUnlockedAchievement();

                    if (player.speed < gameConfig.maxPlayerSpeed)
                        player.speed += doorSpeed;
                }
            }

            currentChunk = chunkGenerator.currentChunk;
        }

        private void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

            if (!isGameStarted)
            {
                musicSource.pitch = Mathf.Lerp(musicSource.pitch, 0f, Time.deltaTime * musicPitchSmooth);
                idleMusicSource.volume = Mathf.Lerp(idleMusicSource.volume, 1f, Time.deltaTime * 5f);
            }else
            {
                idleMusicSource.volume = Mathf.Lerp(idleMusicSource.volume, 0f, Time.deltaTime * 5f);
            }
        }

        void OnGUI()
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
        }

        public void StartGame()
        {
            player.transform.position = basePlayerPosition;
            player.rb.isKinematic = false;
            if (isFirstStart)
            {
                isFirstStart = false;
            }else
            {
                chunkGenerator.Start();
            }

            player.speed = gameConfig.initialPlayerSpeed;

            numberOfDoorsPassed = 0;
            ui.SetScore();
            isGameStarted = true;
            musicSource.pitch = 1f;
            player.isDead = false;
            musicSource.Stop();
            musicSource.Play();
        }

        public void OnPlayerLoose()
        {
            SaveManager.Save();
            idleMusicSource.time = musicSource.time;
            isGameStarted = false;
            player.rb.isKinematic = true;
        }
    }
}