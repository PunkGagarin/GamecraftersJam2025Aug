using Jam.Scripts.Utils.Coroutine;
using Zenject;

namespace Jam.Scripts.SceneManagement
{
    public class SceneChanger
    {
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private CoroutineHelper _coroutineHelper;

        public void StartGameplay()
        {
            _coroutineHelper.RunCoroutine(_sceneLoader.LoadScene(SceneEnum.Gameplay));
        }

        public void StartMenu()
        {
            _coroutineHelper.RunCoroutine(_sceneLoader.LoadScene(SceneEnum.MainMenu));
        }
    }
}