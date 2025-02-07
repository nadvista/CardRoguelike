using Core.Battle;
using Core.Cards;
using Core.Tools.Timer;
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
        private Image blockImage;

        [SerializeField]
        private TextMeshProUGUI cardNameLabel;

        [SerializeField]
        private TextMeshProUGUI cardCostLabel;

        [SerializeField]
        private Image cardPreviewImage;

        private IBattleProvider _battle;
        private ICardsCooldownProvider _cooldownProvider;
        private GameTimer _blockTimer;

        [Inject]
        private void Construct(IBattleProvider battle, ICardsCooldownProvider cooldownProvider)
        {
            _battle = battle;
            _cooldownProvider = cooldownProvider;
        }
        private void Awake()
        {
            blockImage.fillAmount = 0;
            _cooldownProvider.OnBlockCard += OnBlockAnyCard;
            _cooldownProvider.OnUnblockCard += OnUnblockAnyCard;
        }
        private void OnDestroy()
        {
            _cooldownProvider.OnBlockCard -= OnBlockAnyCard;
            _cooldownProvider.OnUnblockCard -= OnUnblockAnyCard;
            if (_blockTimer != null)
            {
                _blockTimer.OnTick -= OnBlockTimerTick;
            }
        }

        private void OnBlockAnyCard(BaseCard card, GameTimer timer)
        {
            if (card != _data || _data == null)
                return;
            _blockTimer = timer;
            _blockTimer.OnTick += OnBlockTimerTick;
            blockImage.fillAmount = 1;
        }

        private void OnUnblockAnyCard(BaseCard card, GameTimer timer)
        {
            if (card != _data || _data == null)
                return;
            _blockTimer.OnTick -= OnBlockTimerTick;
            _blockTimer = null;
            blockImage.fillAmount = 0;
        }
        private void OnBlockTimerTick()
        {
            if(_data == null || _blockTimer == null)
                return;

            var fillAmount = (_data.PlayDelay - _blockTimer.CurrentTimeSeconds) / _data.PlayDelay;
            blockImage.fillAmount = fillAmount;
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
