
using UnityEngine;
using TMPro;

namespace Seville
{
    public class NotificationMessage : MonoBehaviour
    {
        public TextMeshProUGUI textMessage;

        private void Start()
        {
            Invoke(nameof(DestroyPopup), 2.5f);
        }

        public void DestroyPopup()
        {
            Destroy(this.gameObject);
        }
    }
}