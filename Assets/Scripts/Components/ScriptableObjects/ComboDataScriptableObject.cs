using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Controller.Components.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/ComboDataScriptableObject")]
    public class ComboDataScriptableObject: ScriptableObject
    {
        public int DamageScore = 1;
        public List<ComboData> _comboDatas = new List<ComboData>();

        public ComboData GetData(ComboType type)
        {
            return _comboDatas.FirstOrDefault(data => data.Type == type);
        }
    }
}