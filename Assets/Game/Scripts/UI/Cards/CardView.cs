using Core.Battle;
using Core.Cards;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ui.Cards
{
    public class CardView : UIContainerElement<BaseCard>
    {
        [SerializeField]
        private Button playButton;

        [SerializeField]
        private TextMeshProUGUI cardNameLabel;

        [SerializeField]
        private TextMeshProUGUI cardCostLabel;

        [SerializeField]
        private Image cardPreviewImage;

        private BattleProvider _battle;

        [Inject]
        private void Construct(BattleProvider battle)
        {
            _battle = battle;
        }

        protected override void OnSetup(BaseCard data)
        {
            cardNameLabel.text = data.CardData.CardName;
            cardPreviewImage.sprite = data.CardData.PreviewImage;
        }

        protected override void OnActivate()
        {
            _data.ManaCost.OnValueChange += OnCostChange;
            playButton.onClick.AddListener(OnPlayClicked);
            OnCostChange();
        }

        protected override void OnDeactivate()
        {
            if (_data != null)
            {
                _data.ManaCost.OnValueChange -= OnCostChange;
                playButton.onClick.RemoveListener(OnPlayClicked);
            }
        }

        private void OnPlayClicked()
        {
            if (_data == null)
                return;
            _battle.PlayBattleCard(_data);
        }

        private void OnCostChange()
        {
            cardCostLabel.text = _data.ManaCost.ActualValue.ToString();
        }
    }
}
