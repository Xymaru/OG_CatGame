using System;
using System.Collections;
using PawsAndClaws.Entities.Inhibitor;
using PawsAndClaws.Entities.Minion;
using PawsAndClaws.Entities.Nexus;
using PawsAndClaws.Entities.Tower;
using PawsAndClaws.Player;
using UnityEngine;

namespace PawsAndClaws.Game
{
    public class GameManager : MonoBehaviour
    {
        public static Action<Team> OnTeamLose;
        public static Action OnMinionsStartSpawn;
        
        [Header("Minions")]
        [SerializeField] private float timeToStartSpawn = 180f;

        [SerializeField] private MinionWaveManager hamsterMinionWaveManager;
        [SerializeField] private MinionWaveManager catMinionWaveManager;
        
        [Header("Spawn points")] 
        [SerializeField] private Transform hamsterSpawnPoint;
        [SerializeField] private Transform catSpawnPoint;

        [Header("Inhibitors")] 
        [SerializeField] private InhibitorController hamsterInhibitor;
        [SerializeField] private InhibitorController catInhibitor;

        [Header("Nexus")]
        [SerializeField] private NexusController hamsterNexus;
        [SerializeField] private NexusController catNexus;

        [Header("Towers")]
        [SerializeField] private TowerController hamsterInhibitorTower;
        [SerializeField] private TowerController hamsterNexusTowerUp;
        [SerializeField] private TowerController hamsterNexusTowerDown;
        [Space]
        [SerializeField] private TowerController catInhibitorTower;
        [SerializeField] private TowerController catNexusTowerUp;
        [SerializeField] private TowerController catNexusTowerDown;
        
        public Team playerTeam;
        public static GameManager Instance;
        public static LayerMask OppositeTeamLayer;
        
        private Coroutine _startCoroutine;
        
        public float MatchTime => _matchTime;
        public string MatchTimeString => TimeSpan.FromSeconds(_matchTime).ToString("mm':'ss");
        private float _matchTime;
        public static bool MatchStarted => _matchStarted;
        private static bool _matchStarted = false;
        
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            
            Instance = this;
            DontDestroyOnLoad(this);

            // Prevent mouse over events to be used by triggers
            Physics2D.queriesHitTriggers = false;

        }

        private void Start()
        {
            StartMatch();
        }

        private void StartMatch()
        {
            OppositeTeamLayer = playerTeam == Team.Cat ? LayerMask.NameToLayer("Hamsters") : LayerMask.NameToLayer("Cats");
            _matchStarted = true;

            _startCoroutine ??= StartCoroutine(StartMinionTimer());
        }

        private void Update()
        {
            if (!_matchStarted)
                return;
            
            // Update match time
            _matchTime += Time.deltaTime;
            
        }

        private IEnumerator StartMinionTimer()
        {
            yield return new WaitForSeconds(timeToStartSpawn);
            
            hamsterMinionWaveManager.StartSpawningMinions();
            catMinionWaveManager.StartSpawningMinions();
            
            OnMinionsStartSpawn?.Invoke();
            
            _startCoroutine = null;
        }
    }
}
