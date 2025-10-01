using Core.Battle;
using Core.Cards;
using System.Collections.Generic;
using Ui.Hint;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Ui.Cards
{
    public class CardView : UIContainerElement<BaseCard>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Image imagePreview;

        [SerializeField]
        private Image imageCdFill;

        [SerializeField]
        private Image imageCantPlayIndicator;

        private List<CardUiComponent> _components = new List<CardUiComponent>();

        private HintView _hints;

        [Inject]
        private void Construct(ICardsCooldownProvider cooldownProvider, IBattleCardsController battleCardsController, HintView hints)
        {
            var cdComponent = new CardCdView(cooldownProvider, imageCdFill);
            var playView = new CardOnPlayView(battleCardsController, imageCantPlayIndicator);
            _hints = hints;

            _components.Add(playView);
            _components.Add(cdComponent);
        }
        private void Start()
        {
            foreach (var component in _components)
                component.Start();
        }
        protected override void OnSetup(BaseCard data)
        {
            foreach (var component in _components)
                component.SetCard(data);

            imagePreview.sprite = data.CardData.PreviewImage;
        }

        private void OnDestroy()
        {
            foreach (var component in _components)
                component.Dispose();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Data == null)
                return;
            var fullTet = $"Стоимость: {Data.ManaCost.ActualValue}\n{Data.CardData.Description}";
            _hints.Show(fullTet);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _hints.Hide();
        }
    }
}
