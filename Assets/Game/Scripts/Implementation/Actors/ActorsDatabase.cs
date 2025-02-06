using Core.Actors.Enemies;
using Core.Actors.Players;
using System.Collections.Generic;
using UnityEngine;

namespace Implementation.Actors
{
    [CreateAssetMenu(menuName = "Game/ActorsDatabase")]
    public class ActorsDatabase : ScriptableObject
    {
        [field: SerializeField]
        public List<GamePlayer> PlayerList { get; private set; }

        [field: SerializeField]
        public List<GameEnemy> EnemiesList { get; private set; }
    }
}
