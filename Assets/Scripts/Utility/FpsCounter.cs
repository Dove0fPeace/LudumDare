using UnityEngine;
using UnityEngine.UI;

namespace Universal
{
    /// <summary>
    /// Fps counter service.
    /// </summary>
    sealed class FpsCounter : MonoBehaviour
    {
        public Text fpsText;
        private int frames = 0;
        private float lastTime = 0;

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        void Update()
        {
            if (Time.timeSinceLevelLoad > lastTime + 1)
            {
                fpsText.text = frames.ToString();
                frames = 1;
                lastTime = Time.timeSinceLevelLoad;
            }
            else
            {
                frames++;
            }
        }
    }
}