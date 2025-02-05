using Core.Cards;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Desk
{
    [CreateAssetMenu(menuName ="Game/Desk")]
    public class CardsDesk : ScriptableObject
    {
        [field: SerializeField]
        public List<BaseCard> Cards { get; set; }
    }
}
