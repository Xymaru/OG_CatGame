using System;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Player
{
    [CreateAssetMenu(menuName = "Objects/Character Database", fileName = "CharacterDatabase")]
    public class CharacterDatabaseSO : ScriptableObject
    {
        [SerializeField] private List<CharacterDataSO> cats;
        [SerializeField] private List<CharacterDataSO> hamsters;


        public CharacterDataSO GetCharacter(Team team, int id)
        {
            return team switch
            {
                Team.Cat => cats[id],
                Team.Hamster => hamsters[id],
                _ => throw new ArgumentOutOfRangeException(nameof(team), team, null)
            };
        }
    }
}
