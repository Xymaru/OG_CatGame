using System;
using PawsAndClaws.Player;
using UnityEngine;

namespace PawsAndClaws
{
    public class GameManager : MonoBehaviour
    {
        public float matchTime;
        public Team playerTeam;

        public static GameManager Instance;

        public static LayerMask oppositeTeamLayer;

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
            oppositeTeamLayer = playerTeam == Team.Cat ? LayerMask.NameToLayer("Hamster") : LayerMask.NameToLayer("Cat");
        }

        public void StartMatch()
        {
            
        }
    }
}
