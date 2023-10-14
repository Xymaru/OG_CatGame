using PawsAndClaws.Entities;
using UnityEngine;

namespace PawsAndClaws.Utils
{
    public static class GameUtils
    {
        /// <summary>
        /// Checks if the game object implements IGameEntity on any of it's components
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>The IGameEntity script if found</returns>
        public static IGameEntity GetIfHasIGameEntity(GameObject gameObject)
        {
            var gameEntity = gameObject.GetComponent<IGameEntity>();
            return gameEntity;
        }
        
        /// <summary>
        /// Checks if both game objects implement IGameEntity and are on opposite teams
        /// </summary>
        /// <param name="gameObject">Game object calling the function</param>
        /// <param name="other">Game object that will return the IGameEntity if it's on the other team</param>
        /// <returns>Null if either one of the game objects doesn't implement IGameEntity or they are on the same team.
        /// IGameEntity if they are on opposite teams</returns>
        public static IGameEntity GetIfIsEntityFromOtherTeam(GameObject gameObject, GameObject other)
        {
            var goInterface = GetIfHasIGameEntity(gameObject);
            var otherInterface = GetIfHasIGameEntity(other);

            if (goInterface == null || otherInterface == null)
                return null;
            
            return goInterface.Team != otherInterface.Team ? otherInterface : null;
        }
    }
   
}
