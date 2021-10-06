using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SaltyTank.UISystem;

namespace SaltyTank.AchievementSystem
{
    public class AchievementManager : MonoBehaviour
    {
        public static AchievementManager instance;

        public List<AchievementSO> unlockedAchievements = new List<AchievementSO>();
        [Header("Achievements are automatically loaded in Resources folder.")]
        public AchievementSO[] achievements;

        private GameManager manager;
        private UIManager ui;

        private void Awake()
        {
            achievements = Resources.LoadAll<AchievementSO>("Achievements");
            instance = this;
        }

        private void Start()
        {
            ui = UIManager.instance;
            manager = GameManager.instance;
        }

        public void CheckForUnlockedAchievement()
        {
            for(int i = 0; i < achievements.Length; i++)
            {
                AchievementSO achievement = achievements[i];
                if(!unlockedAchievements.Contains(achievement))
                {
                    bool unlocked = true;

                    if (manager.totalCarSmashed < achievement.carSmashed)
                        unlocked = false;

                    if (manager.numberOfDoorsPassed < achievement.doors)
                        unlocked = false;

                    if (manager.totalDoorsPassed < achievement.totalDoors)
                        unlocked = false;

                    if (unlocked)
                    {
                        UnlockAchievement(achievement);
                    }
                }
            }
        }

        void UnlockAchievement(AchievementSO achievement)
        {
            unlockedAchievements.Add(achievement);
            manager.bux += achievement.bux;
            ui.PopNewAchievement(achievement);
        }

        public bool IsAchievementUnlocked(AchievementSO achievement)
        {
            if (unlockedAchievements.Contains(achievement))
            {
                return true;
            }else {
                return false;
            }
        }
    }
}