using Core.Params;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ui.Params
{
    public class ParamView : UIContainerElement<Param>
    {
        [SerializeField]
        private TextMeshProUGUI _paramNameLabel;

        [SerializeField]
        private TextMeshProUGUI _valueLabel;

        protected override void OnSetup(Param data)
        {
            _paramNameLabel.text = data.ParamName;
        }

        protected override void OnActivate()
        {
            _data.OnValueChange += OnParamChange;
            OnParamChange();
        }

        protected override void OnDeactivate()
        {
            if(_data != null)
                _data.OnValueChange -= OnParamChange;
        }

        private void OnParamChange()
        {
            _valueLabel.text = _data.ActualValue.ToString("F2");
        }
    }
}