
using UnityEngine;

namespace Seville
{
    public static class SevilleStyleEditor
    {
        private static GUIStyle _greenButtonStyle = null;
        private static GUIStyle _blueButtonStyle = null;

        public static GUIStyle GreenButton
        {
            get
            {
                if (_greenButtonStyle == null)
                {
                    _greenButtonStyle = CreateButtonStyle(Color.green, Color.black, Color.white, Color.Lerp(Color.green, Color.black, 0.2f));
                }
                return _greenButtonStyle;
            }
        }

        public static GUIStyle BlueButton
        {
            get
            {
                if (_blueButtonStyle == null)
                {
                    _blueButtonStyle = CreateButtonStyle(Color.blue, Color.white, Color.black, Color.Lerp(Color.blue, Color.black, 0.2f));
                }
                return _blueButtonStyle;
            }
        }

        private static GUIStyle CreateButtonStyle(Color normalBgColor, Color normalTextColor, Color hoverTextColor, Color hoverBgColor)
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);

            style.normal.textColor = normalTextColor;
            style.normal.background = MakeTex(600, 1, normalBgColor);

            style.hover.textColor = hoverTextColor;
            style.hover.background = MakeTex(600, 1, hoverBgColor);

            return style;
        }

        private static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }
    }
}