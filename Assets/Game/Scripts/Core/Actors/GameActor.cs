using Core.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Core.Actors
{
    [Serializable]
    public class GameActor : IDisposable, IInitializable
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
            HealthParam.OnValueChange += OnHealthChange;

            Reset();
            OnInitialize();
        }

        public virtual void Reset()
        {
            foreach (var param in AllParams)
                param.Reset();

            HealthParam.Reset();

            OnReset();
        }

        public void Dispose()
        {
            HealthParam.OnValueChange -= OnHealthChange;
        }

        public List<Param> GetParams(string name)
        {
            return AllParams.Where(e => e.ParamName == name).ToList();
        }

        protected virtual void OnInitialize() { }
        protected virtual void OnReset() { }
        private void OnHealthChange()
        {
            if (HealthParam.ActualValue <= 0)
                OnDead?.Invoke();
        }
    }
}
