using Core.Cards.Actions;
using Core.Params;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Cards
{
    [Serializable]
    public class BaseCard : ScriptableObject
    {
        [field: SerializeField]
        public CardData CardData { get; private set; }

        [field: SerializeField]
        public string Id { get; set; }

        [field: SerializeField]
        public Param ManaCost { get; set; }

        [field: SerializeField]
        public float CooldownSeconds { get; set; } = 1f;

        [field: SerializeReference]
        public List<CardAction> Actions { get; set; }
    }
}
