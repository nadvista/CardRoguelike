using Core.Params;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(menuName ="Game/Ui/ParamsData")]
    public class ParamsDatas : ScriptableObject
    {
        [field: SerializeField]
        public List<ParamData> Datas { get; private set; }

        public Sprite GetPreview(ParamType type)
        {
            return GetParamData(type).Sprite;
        }
        public string GetName(ParamType type)
        {
            return GetParamData(type).Name;
        }

        public ParamData GetParamData(ParamType type)
        {
            return Datas.FirstOrDefault(e => e.ParamType == type);
        }
    }
}
