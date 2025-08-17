using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Localization
{
    [CreateAssetMenu(fileName = "LanguageConfig", menuName = "Game Resources/Configs/Language")]
    public class LanguageConfig: ScriptableObject {
        [field: SerializeField] public LanguageType DefaultLanguage { get; private set; }
        [field:SerializeField] public List<TextAsset> LocalizationTextAssets { get; private set; }
    }
}