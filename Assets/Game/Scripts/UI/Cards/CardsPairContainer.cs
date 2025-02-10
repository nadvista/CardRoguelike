using Core.Cards;
using DG.Tweening;
using System;
using UnityEngine;

namespace Ui.Cards
{
    public class CardsPairContainer : MonoBehaviour
    {
        enum PairPosition
        {
            Normal, Switched
        }

        [SerializeField]
        private CardView mainCardView;

        [SerializeField]
        private CardView secondaryCardView;

        private Sequence _switchingSequence;

        private PairPosition _currentPosition = PairPosition.Normal;

        private Vector2 _mainScale;
        private Vector2 _secondaryScale;

        private Vector2 _mainPosition;
        private Vector2 _secondaryPosition;

        private RectTransform _mainRect;
        private RectTransform _secondaryRect;

        private void Awake()
        {
            _mainRect = mainCardView.GetComponent<RectTransform>();
            _secondaryRect = secondaryCardView.GetComponent<RectTransform>();

            _mainScale = _mainRect.sizeDelta;
            _secondaryScale = _secondaryRect.sizeDelta;

            _mainPosition = _mainRect.anchoredPosition;
            _secondaryPosition = _secondaryRect.anchoredPosition;
        }

        private void OnDestroy()
        {
            _switchingSequence.Kill();
        }

        public void SetupPair(BaseCard main, BaseCard secondary)
        {
            mainCardView.Setup(main);
            secondaryCardView.Setup(secondary);
        }

        public void Switch(float time)
        {
            if (_switchingSequence != null)
            {
                _switchingSequence.Kill();
            }

            //inverse position
            _currentPosition = _currentPosition == PairPosition.Normal? PairPosition.Switched: PairPosition.Normal;

            Vector3 newMainPos = Vector3.zero; 
            Vector3 newMainScale = Vector3.zero;
            Vector3 newSecondaryPos = Vector3.zero;
            Vector3 newSecondaryScale = Vector3.zero;

            switch (_currentPosition)
            {
                case PairPosition.Normal:
                    newMainPos = _mainPosition;
                    newSecondaryPos = _secondaryPosition;

                    newMainScale = _mainScale;
                    newSecondaryScale = _secondaryScale;
                    break;
                case PairPosition.Switched:
                    newMainPos = _secondaryPosition;
                    newSecondaryPos = _mainPosition;

                    newMainScale = _secondaryScale;
                    newSecondaryScale = _mainScale;
                    break;
            }

            _switchingSequence = DOTween.Sequence();
            _switchingSequence.Insert(0, _mainRect.DOAnchorPos(newMainPos, time));
            _switchingSequence.Insert(0, _mainRect.DOSizeDelta(newMainScale, time));

            _switchingSequence.Insert(0, _secondaryRect.DOAnchorPos(newSecondaryPos, time));
            _switchingSequence.Insert(0, _secondaryRect.DOSizeDelta(newSecondaryScale, time));

            _switchingSequence.OnComplete(() => {
                _switchingSequence = null;
            });
        }
    }
}
