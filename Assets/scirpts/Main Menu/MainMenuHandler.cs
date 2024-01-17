using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    AssetBundle _assetBundle;

    private void Start()
    {
        //_assetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/scenes");
        
    }
    public void OnStartClick()
    {
        //var path = _assetBundle.GetAllScenePaths().Where(x => x.Contains("level 1")).First();
        SceneManager.LoadScene("level 1", LoadSceneMode.Single);
    }
}
