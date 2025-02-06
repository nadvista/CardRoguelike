using Core.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Actors
{
    [Serializable]
    public class GameActor
    {
        [field: SerializeField]
        public ActorData Data { get; private set; }

        [field: SerializeField]
        public Param HealthParam { get; private set; }

        [field: SerializeField]
        public List<Param> AllParams { get; private set; }

        public event Action OnDead;

        public virtual void Initialize()
        {
            Reset();

            HealthParam.OnValueChange -= OnHealthChange;

            HealthParam.OnValueChange += OnHealthChange;
        }

        public virtual void Reset()
        {
            foreach (var param in AllParams)
                param.Reset();

            HealthParam.Reset();
        }
        private void OnHealthChange()
        {
            if (HealthParam.ActualValue <= 0)
                OnDead?.Invoke();
        }
        public List<Param> GetParams(string name)
        {
            return AllParams.Where(e => e.ParamName == name).ToList();
        }
    }
}
