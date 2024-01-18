using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBtnHandler : MonoBehaviour
{
    [SerializeField] private string _nextSceneName;

    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnExit() 
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public void OnNextLevel()
    {
        SceneManager.LoadScene(_nextSceneName, LoadSceneMode.Single);
    }
}
