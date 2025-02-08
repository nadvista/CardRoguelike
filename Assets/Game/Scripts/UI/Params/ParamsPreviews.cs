using Core.Params;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(menuName ="Game/Ui/ParamsPreview")]
    public class ParamsPreviews : ScriptableObject
    {
        [field: SerializeField]
        public List<ParamPreviewData> ParamsPreviewData { get; private set; }

        public Sprite GetPreview(ParamType type)
        {
            return ParamsPreviewData.FirstOrDefault(e => e.ParamType == type).Sprite;
        }
    }
}
