using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SaltyTank.ChunkSystem
{
    public class ChunkGeneration : MonoBehaviour
    {
        #region Public Vars

        [Header("Chunk Configuration")]
        public Chunk chunkPrefab;
        public Transform chunkContainer;
        public int maxNextChunk = 3;

        [Header("Debug Only")]
        public List<Chunk> nextChunks = new List<Chunk>();
        public Chunk nextChunk;
        public Chunk currentChunk;
        public Chunk previousChunk;

        #endregion

        #region Private Vars 

        private GameManager manager;

        #endregion

        void Init()
        {
            manager = GameManager.instance;

            ClearAllChunks();
        }

        void ClearAllChunks()
        {
            foreach(Transform child in chunkContainer)
            {
                Destroy(child.gameObject);
            }

            previousChunk = null;
            nextChunk = null;
            currentChunk = null;
            nextChunks = new List<Chunk>();
        }

        public void Start()
        {
            Init();
            SetNextChunkPrefab();

            currentChunk = Instantiate(chunkPrefab, chunkContainer);

            if (previousChunk)
                previousChunk.name = "Previous Chunk";
            currentChunk.name = "Current Chunk";

            for (int i = 0; i < maxNextChunk; i++)
            {
                Chunk nextChunk = Instantiate(chunkPrefab, chunkContainer);
                nextChunks.Add(nextChunk);
                nextChunk.name = "Next Chunk " + i;

                if (i > 0)
                {
                    nextChunk.transform.position = new Vector3(nextChunks[i - 1].endPosition.position.x, 0f, nextChunks[i - 1].endPosition.position.z);
                } else
                {
                    nextChunk.transform.position = new Vector3(currentChunk.endPosition.position.x, 0, currentChunk.endPosition.position.z);
                    this.nextChunk = nextChunk;
                }
            }
        }

        void SetNextChunkPrefab()
        {
            GameConfigSO gameConfig = manager.gameConfig;

            List<ChunkConfig> chunkConfigList = new List<ChunkConfig>();

            for(int i = 0; i < gameConfig.chunkGenerationConfig.chunkConfigs.Length; i++)
            {
                ChunkConfig config = gameConfig.chunkGenerationConfig.chunkConfigs[i];

                if(config.maxDoor == 0 && config.minDoor <= manager.numberOfDoorsPassed)
                {
                    chunkConfigList.Add(config);
                }else if(config.maxDoor >= manager.numberOfDoorsPassed && config.minDoor <= manager.numberOfDoorsPassed)
                {
                    chunkConfigList.Add(config);
                }
            }

            if (chunkConfigList.Count == 1)
                chunkPrefab = chunkConfigList[0].prefab;
            else
                chunkPrefab = SelectChunk(chunkConfigList.ToArray());
        }

        public Chunk SelectChunk(ChunkConfig[] chunkConfigs)
        {
            // Calculate the summa of all chunks
            int poolSize = 0;
            for (int i = 0; i < chunkConfigs.Length; i++)
            {
                poolSize += chunkConfigs[i].chance;
            }

            // Get a random integer from 0 to PoolSize.
            int randomNumber = Random.Range(0, poolSize) + 1;

            // Detect the item, which corresponds to current random number.
            int accumulatedProbability = 0;
            for (int i = 0; i < chunkConfigs.Length; i++)
            {
                accumulatedProbability += chunkConfigs[i].chance;
                if (randomNumber <= accumulatedProbability)
                    return chunkConfigs[i].prefab;
            }
            return null;
        }

        public void CreateNextChunk()
        {
            if (previousChunk)
            {
                Destroy(previousChunk.gameObject);
            }

            previousChunk = currentChunk;
            currentChunk = nextChunk;

            // Regenerate new nextChunk
            Chunk _nextChunk = Instantiate(chunkPrefab, chunkContainer);
            SetNextChunkPrefab();
            _nextChunk.transform.position = new Vector3(nextChunks[maxNextChunk - 1].endPosition.position.x, 0f, nextChunks[maxNextChunk - 1].endPosition.position.z);
            nextChunks.Add(_nextChunk);

            // Delete current chunk from the next chunk list
            nextChunks.RemoveAt(0);

            for (int i = 0; i < nextChunks.Count; i++)
            {
                nextChunks[i].name = "Next Chunk " + i;
            }

            nextChunk = nextChunks[0];

            previousChunk.name = "Previous Chunk";
            currentChunk.name = "Current Chunk";
            nextChunk.name = "Next Chunk";
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                CreateNextChunk();
            }
        }
    }
}