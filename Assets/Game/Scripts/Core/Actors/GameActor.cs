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
        #region Properties

        [field: SerializeField]
        public ActorData Data { get; private set; }

        [field: SerializeField]
        public Param HealthParam { get; private set; }

        [field: SerializeField]
        public List<Param> AllParams { get; private set; }

        #endregion

        #region Events

        public event Action OnDead;

        #endregion

        #region Public Methods

        public void Initialize()
        {
            HealthParam.OnValueChange += OnHealthChange;

            Reset();
            OnInitialize();
        }

        public void Reset()
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

        public IEnumerable<Param> GetParams(ParamType paramType)
        {
            return AllParams.Where(e => e.Type == paramType);
        }

        #endregion

        #region Protected Methods 

        protected virtual void OnInitialize() { }
        protected virtual void OnReset() { }
        private void OnHealthChange()
        {
            if (HealthParam.ActualValue <= 0)
                OnDead?.Invoke();
        }

        #endregion
    }
}
