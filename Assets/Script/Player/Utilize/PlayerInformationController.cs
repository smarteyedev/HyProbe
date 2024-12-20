using SR;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace SR
{
    public class PlayerInformationController : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<Collider, ABaseObjectInteraction> _cachedCollider;

        private ABaseObjectInteraction _activeData;

        private void OnEnable()
        {
            EventManager.OnUpdatePlayerInformation += OnSwtObjectInformation;
        }

        private void OnDisable()
        {
            EventManager.OnUpdatePlayerInformation -= OnSwtObjectInformation;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (_cachedCollider.TryGetValue(other, out ABaseObjectInteraction interaction))
            {
                _activeData = interaction;
                interaction.SetSizeObject(true);
            }
            else
            {
                // Jika collider tidak ditemukan dalam dictionary
                Debug.LogWarning("Collider tidak ditemukan dalam dictionary.");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_activeData != null)
            {
                _activeData.SetSizeObject(false);
            }
            _activeData = null;
        }

        public void OnSwtObjectInformation()
        {
            if (_activeData != null)
            {
                ObjectDataCasting(_activeData.GetObjectData(), _activeData.objectType);
                _activeData.gameObject.SetActive(false);
                _activeData = null;
            }
        }


        private void ObjectDataCasting(ABaseObjectData abaseData, ObjectType type)
        {
            if(type == ObjectType.CharacteristicObject){
                ObjectCharacteristicData data = abaseData as ObjectCharacteristicData;

                if (data != null)
                {
                    EventManager.onUpdateNPCStatusUI.Invoke(data.npcStatus);
                }
            }
        }
    }
}

