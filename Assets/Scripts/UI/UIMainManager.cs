using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UIMainManager : MonoBehaviour
    {
        public Text debugText;

        public string FormatLogString(params LogItem[] keyValues)
        {
            if (keyValues.Length < 1) return "";
            if (keyValues.Length < 2) return keyValues[0].ToString();
            
            string output = keyValues[0].ToString();
            for (int i = 1; i < keyValues.Length; i++)
            {
                output += $"\n{keyValues[i].ToString()}";
            }
            return output;
        }
        
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
