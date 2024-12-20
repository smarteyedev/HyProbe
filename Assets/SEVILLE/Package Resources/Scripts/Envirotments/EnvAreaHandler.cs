using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

namespace Seville
{
    public class EnvAreaHandler : MonoBehaviour
    {
        [Tooltip("Insert components contained in the area (ex: canvas and object interaction)")]
        public List<GameObject> areaObjsList;
        public Texture2D areaTexture;
        public bool isRestartOnExitArea = false;

        [Space]
        public float firstCamLookRotationValue;

        [Space]
        public AudioClip backsound;

        public void SetActiveObjsState(bool state)
        {
            foreach (var item in areaObjsList)
            {
                item.SetActive(state);
            }
        }
    }
}