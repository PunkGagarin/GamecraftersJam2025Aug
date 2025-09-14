using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts
{
    [RequireComponent(typeof(Graphic))]
    public class ToLocalize : MonoBehaviour
    {
        [SerializeField] private string _key;
        [SerializeField] private TMP_FontAsset _fontAsset;
        private RectTransform _rectTransform;
        private TextMeshProUGUI _tmpText;

        [Inject] private LanguageService _languageService;
        [Inject] private LocalizationTool _localizationTool;

        public void SetKey(string keyValue)
        {
            _key = keyValue;
            SwitchText();
        }

        private void Awake()
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();

            _tmpText = GetComponent<TextMeshProUGUI>();
            _languageService.OnSwitchLanguage += SwitchText;
        }

        private void Start() => SwitchText();

        private void OnDestroy() =>
            _languageService.OnSwitchLanguage -= SwitchText;

        private void SwitchText()
        {
            string tmpTextText = _localizationTool.GetText(_key).Replace("\\n", "\n");
            if(tmpTextText == "Undefined") return;
            _tmpText.text = tmpTextText;

            if (_fontAsset != null) _tmpText.font = _fontAsset;

            if (_tmpText.gameObject.transform.childCount > 0)
            {
                var children = _tmpText.gameObject.GetComponentsInChildren<TMP_SubMeshUI>();
                if (children != null)
                {
                    foreach (var child in children)
                        Destroy(child.gameObject);
                }
            }
        }
    }
}