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
        [SerializeField] private float timeBetweenMinionSpawn = 0.2f;
        [SerializeField] private int minionsPerWave = 3;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform checkPoint;
        [SerializeField] private Team team;

        private bool _canSpawn = false;
        private Coroutine _spawnCoroutine;

        [Header("Pool")]
        [SerializeField] private int poolSize = 10;
        [SerializeField] private GameObject minionPrefab;
        [SerializeField] private List<GameObject> minionList = new List<GameObject>();

        private List<Networking.NPMinionSequence> _packetsToSend = new();
        private object _packetsMutex = new();

        private void Start()
        {
            AddMinionToPool(poolSize);
        }

        private void Update()
        {
            if(_canSpawn)
            {
                _spawnCoroutine ??= StartCoroutine(SpawnMinionCoroutine());
            }

            lock (_packetsMutex)
            {
                foreach(Networking.NPMinionSequence p in _packetsToSend)
                {
                    Networking.ReplicationManager.Instance.SendPacketMinion(p);
                }

                _packetsToSend.Clear();
            }
        }

        public void StartSpawningMinions()
        {
            _canSpawn = true;
        }

        private IEnumerator SpawnMinionCoroutine()
        {
            for(int i = 0; i < minionsPerWave; i++)
            {
                var minion = RequestMinion();

                Networking.NPMinionSpawn packet = new();
                packet.team = team;

                lock (_packetsMutex)
                {
                    _packetsToSend.Add(packet);
                }

                yield return new WaitForSeconds(timeBetweenMinionSpawn);
            }
            yield return new WaitForSeconds(timeToNextWave);
            _spawnCoroutine = null;
        }

        private void AddMinionToPool(int amount)
        {
            for(var i = 0; i < amount; i++) 
            {
                //var minion = Instantiate(minionPrefab, spawnPoint);
                var minion = Networking.ReplicationManager.Instance.CreateNetObject(minionPrefab, spawnPoint.position);
                Utils.GameUtils.SetEntityTeam(ref minion, team);
                minion.GetComponent<MinionStateMachine>().checkPoint = checkPoint;
                var minionMan = minion.GetComponent<MinionController>();
                minionMan.IsAlive = false;
                minion.SetActive(false);
                minionList.Add(minion);
            }
        }

        public GameObject RequestMinion()
        {
            // Take available minion
            foreach(var minion in minionList)
            {
                if(!minion.activeSelf)
                {
                    minion.SetActive(true);
                    var man = minion.GetComponent<MinionController>();
                    man.IsAlive = true;
                    man.Initialize();
                    minion.transform.position = spawnPoint.position;
                    return minion;
                }
            }
            // If none available, add to pool and request again.
            AddMinionToPool(poolSize / 2);
            return RequestMinion();
        }
    }
}
