using System;
using UnityEngine;

namespace Core.Cards
{
    [Serializable]
    public struct CardData
    {
        [field: SerializeField]
        public Sprite PreviewImage;

        [field: SerializeField]
        public string CardName;
    }
}
