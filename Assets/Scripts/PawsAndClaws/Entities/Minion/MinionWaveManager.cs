using PawsAndClaws.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Entities.Minion
{
    public class MinionWaveManager : MonoBehaviour
    {
        [Header("Spawn")]
        [SerializeField] private float timeToNextWave = 30f;
        [SerializeField] private float timeToStartSpawn = 180f;
        [SerializeField] private float timeBetweenMinionSpawn = 0.2f;
        [SerializeField] private int minionsPerWave = 3;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Team team;

        private bool _canSpawn = false;
        private Coroutine _startCoroutine;
        private Coroutine _spawnCoroutine;


        [Header("Pool")]
        [SerializeField] private int poolSize = 10;
        [SerializeField] private GameObject minionPrefab;
        [SerializeField] private List<GameObject> minionList = new List<GameObject>();

        private void Awake()
        {
            AddMinionToPool(poolSize);
        }

        private void Start()
        {
            _startCoroutine ??= StartCoroutine(StartSpawnCoroutine());
        }

        private void Update()
        {
            if(_canSpawn)
            {
                _spawnCoroutine ??= StartCoroutine(SpawnMinionCoroutine());
            }
        }

        private IEnumerator StartSpawnCoroutine()
        {
            yield return new WaitForSeconds(timeToStartSpawn);
            _canSpawn = true;
        }

        private IEnumerator SpawnMinionCoroutine()
        {
            for(int i = 0; i < minionsPerWave; i++)
            {
                var minion = RequestMinion();
                Debug.Log("Minion Spawned");
                yield return new WaitForSeconds(timeBetweenMinionSpawn);
            }
            yield return new WaitForSeconds(timeToNextWave);
            _spawnCoroutine = null;
        }

        private void AddMinionToPool(int amount)
        {
            for(int i = 0; i < amount; i++) 
            {
                var minion = Instantiate(minionPrefab, transform);
                Utils.GameUtils.SetEntityTeam(ref minion, team);
                minionList.Add(minion);
                minion.SetActive(false);
                minion.transform.position = spawnPoint.position;
            }
        }

        public GameObject RequestMinion()
        {
            foreach(var minion in minionList)
            {
                if(!minion.activeSelf)
                {
                    minion.SetActive(true);
                    return minion;
                }
            }
            AddMinionToPool(poolSize / 2);
            return RequestMinion();
        }
    }
}
