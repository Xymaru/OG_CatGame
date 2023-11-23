using UnityEngine;

namespace PawsAndClaws.Entities
{
    public class Particle : MonoBehaviour
    {
        public void Delete()
        {
            Destroy(gameObject);
        }
    }
}
