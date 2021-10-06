using SaltyTank.AchievementSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaltyTank
{
    public static class SaveManager
    {
        public static SaveGame saveGame = new SaveGame();

        public static void RetrieveSave()
        {
            if(System.IO.File.Exists(Application.persistentDataPath + "/save.json"))
            {
                string dataJson = System.IO.File.ReadAllText(Application.persistentDataPath + "/save.json");
                saveGame = JsonUtility.FromJson<SaveGame>(dataJson);

                GameManager gameManager = GameManager.instance;
                AchievementManager achievementManager = AchievementManager.instance;

                gameManager.bux = saveGame.bux;
                gameManager.totalCarSmashed = saveGame.totalCarSmashed;
                gameManager.totalDoorsPassed = saveGame.totalDoorsPassed;

                for(int i = 0; i < saveGame.achievementsUnlocked.Length; i++)
                {
                    string achievement = saveGame.achievementsUnlocked[i];

                    for(int x = 0; x < achievementManager.achievements.Length; x++)
                    {
                        AchievementSO _achievement = achievementManager.achievements[x];

                        if(achievement == _achievement.slug)
                        {
                            achievementManager.unlockedAchievements.Add(_achievement);
                            break;
                        }
                    }
                }
            }
        }

        public static void Save()
        {
            GameManager gameManager = GameManager.instance;
            AchievementManager achievementManager = AchievementManager.instance;

            // Achievement Saving
            List<string> achievements = new List<string>();

            for (int i = 0; i < achievementManager.unlockedAchievements.Count; i++)
            {
                achievements.Add(achievementManager.unlockedAchievements[i].slug);
            }

            saveGame.achievementsUnlocked = achievements.ToArray();

            // User infos save

            saveGame.bux = gameManager.bux;
            saveGame.totalDoorsPassed = gameManager.totalDoorsPassed;
            saveGame.totalCarSmashed = gameManager.totalCarSmashed;

            WriteToJson();
        }

        static void WriteToJson()
        {
            System.IO.File.WriteAllText(Application.persistentDataPath + "/save.json", JsonUtility.ToJson(saveGame));
        }
    }

    [System.Serializable]
    public struct SaveGame
    {
        public string[] achievementsUnlocked;
        public int bux;
        public int totalDoorsPassed;
        public int totalCarSmashed;
    }
}