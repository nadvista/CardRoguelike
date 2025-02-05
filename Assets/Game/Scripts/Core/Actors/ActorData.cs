using System;
using UnityEngine;

namespace Core.Actors
{
    [Serializable]
    public struct ActorData
    {
        [field: SerializeField]
        public Sprite Image { get; private set; }

        [field: SerializeField]
        public string Name { get; private set; }
    }
}
