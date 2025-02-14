using Core.Params;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ui.Params
{
    public class ParamView : UIContainerElement<Param>
    {
        [SerializeField]
        private Image _paramPreview;

        [SerializeField]
        private TextMeshProUGUI _label;

        [SerializeField]
        private TextMeshProUGUI _valueLabel;

        private ParamsDatas _datas;

        [Inject]
        private void Construct(ParamsDatas previews)
        {
            _datas = previews;
        }

        protected override void OnSetup(Param data)
        {
            _paramPreview.sprite = _datas.GetPreview(data.Type);
            _label.text = _datas.GetName(data.Type);
        }

        protected override void OnActivate()
        {
            Data.OnValueChange += OnParamChange;
            OnParamChange();
        }

        protected override void OnDeactivate()
        {
            if(Data != null)
                Data.OnValueChange -= OnParamChange;
        }

        private void OnParamChange()
        {
            _valueLabel.text = Data.ActualValue.ToString("F0");
        }
    }
}