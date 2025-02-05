using System.Collections.Generic;
using UnityEngine;

namespace Core.Actors
{
    [CreateAssetMenu(menuName ="Game/ActorsDatabase")]
    public class ActorsDatabase : ScriptableObject
    {
        [field:SerializeField]
        public List<Actor> PlayerList { get; private set; }

        [field: SerializeField]
        public List<Actor> EnemiesList { get; private set; }
    }
}
