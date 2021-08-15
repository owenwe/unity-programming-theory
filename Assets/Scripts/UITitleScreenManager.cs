using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITitleScreenManager : MonoBehaviour
{
    public void SelectUp()
    {
        if (TitleMainManager.Instance.CurrentSelectionIndex() > 0)
        {
            TitleMainManager.Instance.ChangeSelection(true);
        }
    }

    public void SelectDown()
    {
        if (TitleMainManager.Instance.CurrentSelectionIndex() != (TitleMainManager.Instance.SelectionsLength() - 1))
        {
            TitleMainManager.Instance.ChangeSelection(false);
        }
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
