using UnityEngine;

namespace SaltyTank.AchievementSystem
{
    [CreateAssetMenu(fileName = "New Achievement", menuName = "SaltyTank/Achievement")]
    public class AchievementSO : ScriptableObject
    {
        public string slug;

        public string successName {
            get
            {
                return Localization.Translate(langKey + "_title")
                    .Replace("%d", doors.ToString())
                    .Replace("%td", totalDoors.ToString())
                    .Replace("%c", carSmashed.ToString());
            }
        }

        public string langKey;

        public string successDesc
        {
            get
            {
                return Localization.Translate(langKey + "_desc")
                    .Replace("%d", doors.ToString())
                    .Replace("%td", totalDoors.ToString())
                    .Replace("%c", carSmashed.ToString());
            }
        }

        [Header("Icon of the success")]
        public Sprite icon;

        [Header("Doors to pass")]
        public int doors;

        [Header("Total Doors to pass")]
        public int totalDoors;

        [Header("Cars to smash")]
        public int carSmashed;

        [Header("Bux rewarded")]
        public int bux;
    }
}