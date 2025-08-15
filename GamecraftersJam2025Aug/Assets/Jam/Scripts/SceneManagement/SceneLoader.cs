using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jam.Scripts.SceneManagement
{
    public class SceneLoader
    {
        private AsyncOperation _asyncOperation;
        
        public IEnumerator LoadScene(SceneEnum scene)
        {
            SceneManager.LoadScene(SceneEnum.Loading.ToString());

            yield return null;
            
            _asyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

            while (!_asyncOperation.isDone)
            {
                yield return null;
            }
        }

        public float GetLoadingProcess() => 
            _asyncOperation?.progress ?? 0f;
    }
}
