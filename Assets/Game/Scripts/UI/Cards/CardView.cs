using Core.Battle;
using Core.Cards;
using Core.Tools.Timer;
using TMPro;
using Ui.Params;
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
        private Image cantPlaySignalImage;

        [SerializeField]
        private TextMeshProUGUI cardNameLabel;

        [SerializeField]
        private ParamView cardCostParamView;

        [SerializeField]
        private Image cardPreviewImage;

        private IBattleProvider _battle;

        private CardPlaySignal _playSignal;
        private CardCooldownSignal _cdSignal; 

        [Inject]
        private void Construct(IBattleProvider battle, ICardsCooldownProvider cooldownProvider)
        {
            _battle = battle;

            _playSignal = new CardPlaySignal(cantPlaySignalImage, battle, cooldownProvider, null);
            _cdSignal = new CardCooldownSignal(null, blockImage, cooldownProvider);
        }

        private void Awake()
        {
            blockImage.fillAmount = 0;
            cantPlaySignalImage.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _playSignal.Dispose();
            _cdSignal.Dispose();
        }

        protected override void OnSetup(BaseCard data)
        {
            cardNameLabel.text = data.CardData.CardName;
            cardPreviewImage.sprite = data.CardData.PreviewImage;
            _playSignal.SetCard(data);
            _cdSignal.SetCard(data);
        }

        protected override void OnActivate()
        {
            Data.ManaCost.OnValueChange += OnCostChange;
            playButton.onClick.AddListener(OnPlayClicked);
            OnCostChange();
        }

        protected override void OnDeactivate()
        {
            if (Data != null)
            {
                Data.ManaCost.OnValueChange -= OnCostChange;
                playButton.onClick.RemoveListener(OnPlayClicked);
            }
        }

        private void OnPlayClicked()
        {
            if (Data == null)
                return;
            _battle.PlayBattleCard(Data);
        }

        private void OnCostChange()
        {
            cardCostParamView.Setup(Data.ManaCost);
        }
    }
}
