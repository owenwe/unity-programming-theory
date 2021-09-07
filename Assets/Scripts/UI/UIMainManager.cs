using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UIMainManager : MonoBehaviour
    {
        public Text debugText;

        public void Log(string output)
        {
            debugText.text = output;
        }

        public void ReturnToTitle()
        {
            SceneManager.LoadScene(0);
        }
    }
}
