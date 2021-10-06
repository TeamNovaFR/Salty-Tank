using UnityEngine;
using SaltyTank.ChunkSystem;

namespace SaltyTank
{
    [CreateAssetMenu(fileName = "New Game Config", menuName = "SaltyTank/Game Config")]
    public class GameConfigSO : ScriptableObject
    {
        [Header("The initial player speed at the beginning of the game")]
        public float initialPlayerSpeed = 5f;

        [Header("The maximum player speed at the beginning of the game")]
        public float maxPlayerSpeed = 60f;

        [Space]
        
        [Header("Chunk Generation Configuration")]
        public ChunkGenerationConfig chunkGenerationConfig;
    }

    [System.Serializable]
    public class ChunkGenerationConfig
    {
        public ChunkConfig[] chunkConfigs;
    }

    [System.Serializable]
    public class ChunkConfig
    {
        [Header("The prefab of the chunk")]
        public Chunk prefab;

        [Header("Min. doors for unlocking this chunk")]
        public int minDoor;

        [Header("Max. doors for unlocking this chunk (0=no limits)")]
        public int maxDoor;

        [Header("Chance for unlocking this chunk")]
        public int chance = 100;
    }
}
