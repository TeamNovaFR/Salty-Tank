using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SaltyTank.AchievementSystem;

namespace SaltyTank.UISystem
{
    public class UIManager : MonoBehaviour
    {
        #region Public Vars 

        public static UIManager instance;

        [Header("References")]
        public GameObject startButton;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI buxText;
        public AudioSource audioSource;

        [Header("Achievement UI")]
        public Transform achievementContainer;
        public AchievementPopUI achievementPopPrefab;
        public AudioClip achievementClip;

        #endregion

        #region Private Vars

        private GameManager manager;

        #endregion

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            manager = GameManager.instance;
        }

        public void SetScore()
        {
            scoreText.text = manager.numberOfDoorsPassed.ToString();
            scoreText.GetComponent<Animator>().SetTrigger("Trigger");
        }

        private void Update()
        {
            buxText.text = manager.bux.ToString();
            startButton.SetActive(!manager.isGameStarted);
        }

        public void PopNewAchievement(AchievementSO achievement)
        {
            audioSource.PlayOneShot(achievementClip);

            AchievementPopUI ach = Instantiate(achievementPopPrefab, achievementContainer);

            ach.title.text = "<size=25>"+ achievement.successName + "</size><size=20>\n<color=#6FC36F>+"+achievement.bux+" bux</color></size>";
        }

        public void PopNewAchievement(string title)
        {
            audioSource.PlayOneShot(achievementClip);

            AchievementPopUI ach = Instantiate(achievementPopPrefab, achievementContainer);

            ach.title.text = title;
        }
    }
}