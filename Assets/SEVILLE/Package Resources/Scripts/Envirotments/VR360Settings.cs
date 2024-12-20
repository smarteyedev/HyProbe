
using UnityEngine;

namespace Seville
{
    // [CreateAssetMenu(fileName = "VR360Settings", menuName = "Seville/VR2DSettings", order = 1)]
    public class VR360Settings : ScriptableObject
    {
        public int areaIndex = 0;
        public Color DefaultMaterial360Color = new Color(1, 1, 1, 0);

        public int GetCurrentAreaIndex()
        {
            return areaIndex;
        }

        public void SetCurrentAreaIndex(int newIndex) => areaIndex = newIndex;

        public void ResetAreaIndex() => areaIndex = 0;

        public Color GetDefaultMaterialColor()
        {
            return DefaultMaterial360Color;
        }
    }
}