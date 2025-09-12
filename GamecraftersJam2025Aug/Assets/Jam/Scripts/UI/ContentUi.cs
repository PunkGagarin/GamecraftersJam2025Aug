using UnityEngine;

namespace Jam.Scripts.UI
{
    public class ContentUi : MonoBehaviour
    {
        [SerializeField] protected GameObject content;

        public void Show()
        {
            content.SetActive(true);
        }
        
        public void Hide()
        {
            content.SetActive(false);
        }
    }
}