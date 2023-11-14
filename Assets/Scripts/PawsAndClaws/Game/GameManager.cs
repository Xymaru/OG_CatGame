using System;
using PawsAndClaws.Debugging;
using PawsAndClaws.Entities.Inhibitor;
using PawsAndClaws.Entities.Nexus;
using PawsAndClaws.Entities.Tower;
using PawsAndClaws.Player;
using UnityEngine;

namespace PawsAndClaws.Game
{
    public class GameManager : MonoBehaviour
    {
        public static Action<Team> OnTeamLose;
        
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
        
        public float MatchTime => _matchTime;
        public string MatchTimeString => TimeSpan.FromSeconds(_matchTime).ToString("mm':'ss");
        private float _matchTime;
        public bool MatchStarted => _matchStarted;
        private bool _matchStarted = false;

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

            var debugMan = gameObject.GetComponent<DebugManager>();
            debugMan.enabled = false;
            
            // Enable debugging info
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            debugMan.enabled = true;
#endif
        }

        private void Start()
        {
            OppositeTeamLayer = playerTeam == Team.Cat ? LayerMask.NameToLayer("Hamsters") : LayerMask.NameToLayer("Cats");
            
            StartMatch();
        }

        private void StartMatch()
        {
            _matchStarted = true;
        }

        private void Update()
        {
            if (!_matchStarted)
                return;
            
            _matchTime += Time.deltaTime;
        }
    }
}
