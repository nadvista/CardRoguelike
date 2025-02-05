using Core.Actors;
using TMPro;
using Ui.Params;
using UnityEngine;
using UnityEngine.UI;
namespace Ui.Actors
{
    public class ActorView : UIContainerElement<Actor>
    {
        [SerializeField]
        private TextMeshProUGUI nameLabel;

        [SerializeField]
        private Image imagePreview;

        [SerializeField]
        private ParamView mainParamView;

        [SerializeField]
        private ParamsContainerView paramsContainerView;

        protected override void OnSetup(Actor data)
        {
            nameLabel.text = data.Data.Name;
            imagePreview.sprite = data.Data.Image;

            mainParamView.Setup(data.HealthParam);
            mainParamView.Activate();

            paramsContainerView.Fill(data.AllParams);
        }
    }
}