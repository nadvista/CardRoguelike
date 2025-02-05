using System.Collections.Generic;
using UnityEngine;

namespace Core.Desk
{
    [CreateAssetMenu(menuName = "Game/DesksDatabase")]
    public class DesksDatabase : ScriptableObject
    {
        [field: SerializeField]
        public List<CardsDesk> Desks { get; set; }
    }
}
