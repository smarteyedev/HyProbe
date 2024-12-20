using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Seville
{
    public class EnvNavigation : MonoBehaviour
    {
        public int targetNextSceneOrArea;
        public void OnClickChangeAreaVR360()
        {
            EnvironmentManager.Instance.StartAreaByIndex(targetNextSceneOrArea);
        }

        public void OnClickChangeScene()
        {
            SceneManager.LoadScene(targetNextSceneOrArea);
        }
    }
}