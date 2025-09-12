using System.Collections.Generic;
using Jam.Scripts.Localization;
using UnityEngine;

namespace Jam.Scripts
{
    [CreateAssetMenu(fileName = "LanguageConfig", menuName = "Game Resources/Configs/Language")]
    public class LanguageConfig: ScriptableObject {
        [field: SerializeField] public LanguageType DefaultLanguage { get; private set; }
        [field:SerializeField] public List<TextAsset> LocalizationTextAssets { get; private set; }
    }
}