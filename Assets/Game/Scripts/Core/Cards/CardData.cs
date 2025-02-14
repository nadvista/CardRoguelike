using System;
using UnityEngine;

namespace Core.Cards
{
    [Serializable]
    public struct CardData
    {
        [field: SerializeField]
        public Sprite PreviewImage { get; private set; }

        [field: SerializeField]
        public string CardName { get; private set; }

        [field: SerializeField]
        [field: TextArea]
        public string Description { get; private set; }
    }
}
