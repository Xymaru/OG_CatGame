using System;
using System.Collections;
using System.Collections.Generic;
using PawsAndClaws.Entities.Inhibitor;
using PawsAndClaws.Entities.Minion;
using PawsAndClaws.Entities.Nexus;
using PawsAndClaws.Entities.Tower;
using PawsAndClaws.Networking;
using PawsAndClaws.Player;
using UnityEngine;

namespace PawsAndClaws.Game
{
    public class GameManager : MonoBehaviour
    {
        public static Action<Team> OnTeamLose;
        public static Action OnMinionsStartSpawn;

        [Header("Database")] 
        [SerializeField] private CharacterDatabaseSO database;

        [SerializeField] private GameObject localPlayerPrefab;
        [SerializeField] private GameObject netPlayerPrefab;
        
        
        [Header("Minions")]
        [SerializeField] private float timeToStartSpawn = 180f;

        [SerializeField] private MinionWaveManager hamsterMinionWaveManager;
        [SerializeField] private MinionWaveManager catMinionWaveManager;
        
        [Header("Spawn points")] 
        [SerializeField] private List<Transform> hamsterSpawnPoint = new();
        [SerializeField] private List<Transform> catSpawnPoint = new();

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
        public static GameObject LocalPlayer;
        
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

            // Remove me
            {
                for (ushort i = 0; i < 2; i++)
                {
                    for (ushort j = 0; j < 3; j++)
                    {
                        if (NetworkData.Teams[i].members[j] != null)
                            NetworkData.Teams[i].members[j].character_id = j;
                    }
                }
            }

            StartMatch();
        }

        
        
        private void StartMatch()
        {
            OppositeTeamLayer = playerTeam == Team.Cat ? LayerMask.NameToLayer("Hamsters") : LayerMask.NameToLayer("Cats");
            _matchStarted = true;

            _startCoroutine ??= StartCoroutine(StartMinionTimer());
            
            SpawnPlayers();
        }
        
        private void SpawnPlayers()
        {
            for (var teamId = 0; teamId < 2; teamId++)
            {
                var teamInfo = NetworkData.Teams[teamId];
                if(teamInfo == null)
                    continue;
                
                for (var userId = 0; userId < 3; userId++)
                {
                    var userInfo = teamInfo.members[userId];
                    if (userInfo == null)
                        continue;

                    var characterData = database.GetCharacter(userInfo.team, userInfo.character_id);
                    var spawnPoint = userInfo.team == Team.Cat ? catSpawnPoint[userId] : hamsterSpawnPoint[userId];
                    
                    // Check if this is the local player
                    if (userInfo.client_id == NetworkData.NetSocket.PlayerI.client_id)
                    {
                        var player = ReplicationManager.Instance.CreateNetObject(localPlayerPrefab, spawnPoint.position);
                        var playerMan = player.GetComponentInChildren<LocalPlayerManager>();
                        playerMan.characterData = characterData;
                        playerMan.userName = userInfo.name;
                        playerMan.spawnPoint = spawnPoint;

                        LocalPlayer = playerMan.gameObject;

                        userInfo.player_obj = LocalPlayer;
                        
                        Debug.Log($"Spawning local player at [{spawnPoint.position}], with character: [{characterData.name}]");
                        
                        continue;
                    }
                    
                    // Spawn the network player
                    {
                        var player = ReplicationManager.Instance.CreateNetObject(netPlayerPrefab, spawnPoint.position);
                        var playerMan = player.GetComponentInChildren<NetworkPlayerManager>();
                        playerMan.characterData = characterData;
                        playerMan.userName = userInfo.name;
                        playerMan.spawnPoint = spawnPoint;

                        userInfo.player_obj = playerMan.gameObject;
                        
                        Debug.Log($"Spawning remote player at [{spawnPoint.position}], with character: [{characterData.name}]");
                    }
                }
            }
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

        public void LoseGame(Team losingTeam)
        {
            
        }
    }
}
