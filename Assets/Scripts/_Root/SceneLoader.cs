using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    [SerializeField] Image _transitionImage;
    [SerializeField] float _fadeTime = 2.5f;
    
    private const string GamePlay = "1(GamePlay)";
    private const string MainMenu = "0(MainMenu)";
    
    private Color _color;

    IEnumerator LoadCoroutine(string sceneName) 
    {
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        if (loadingOperation != null)
        {
            loadingOperation.allowSceneActivation = false;

            _transitionImage.gameObject.SetActive(true);
            
            while (_color.a < 1f)
            {
                _color.a = Mathf.Clamp01(_color.a + Time.unscaledDeltaTime / _fadeTime);
                _transitionImage.color = _color;

                yield return null;
            }


            yield return new WaitUntil(() => loadingOperation.progress >= 0.9f);

            loadingOperation.allowSceneActivation = true;
        }
        
        while (_color.a > 0f) {
            _color.a = Mathf.Clamp01(_color.a - Time.unscaledDeltaTime / _fadeTime);
            _transitionImage.color = _color;

            yield return null;
        }
        
        _transitionImage.gameObject.SetActive(false);
    }

    public void LoadMainScene() {
        
        StopAllCoroutines();  
        StartCoroutine(LoadCoroutine(GamePlay));
    }

    public void LoadMenuScene() {
        StopAllCoroutines();
        StartCoroutine(LoadCoroutine(MainMenu));
    }
}
