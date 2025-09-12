using UnityEngine;

namespace Jam.Scripts.UI
{
    public class ContentUi : MonoBehaviour
    {
        [SerializeField] protected GameObject content;

        public virtual void Show()
        {
            content.SetActive(true);
        }
        
        public virtual void Hide()
        {
            content.SetActive(false);
        }
    }
}