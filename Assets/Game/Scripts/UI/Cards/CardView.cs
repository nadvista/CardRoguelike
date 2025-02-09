using Core.Battle;
using Core.Cards;
using Core.ScoreCounting;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ui.Cards
{
    public class CardView : UIContainerElement<BaseCard>
    {
        [SerializeField]
        private Image imagePreview;

        [SerializeField]
        private Image imageCdFill;

        [SerializeField]
        private Image imageCantPlayIndicator;

        private List<CardUiComponent> _components = new List<CardUiComponent>();

        [Inject]
        private void Construct(ICardsCooldownProvider cooldownProvider, IBattleProvider battleProvider)
        {
            var cdComponent = new CardCdView(cooldownProvider, imageCdFill);
            var playView = new CardOnPlayView(battleProvider, imageCantPlayIndicator);

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
            foreach(var component in _components)
                component.SetCard(data);

            imagePreview.sprite = data.CardData.PreviewImage;
        }

        private void OnDestroy()
        {
            foreach (var component in _components)
                component.Dispose();
        }
    }
}
