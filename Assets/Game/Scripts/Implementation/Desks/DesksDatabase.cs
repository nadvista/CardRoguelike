using Core.Desk;
using System.Collections.Generic;
using UnityEngine;

namespace Implementation.Desks
{
    [CreateAssetMenu(menuName = "Game/DesksDatabase")]
    public class DesksDatabase : ScriptableObject
    {
        [field: SerializeField]
        public List<CardsDesk> Desks { get; set; }
    }
}
