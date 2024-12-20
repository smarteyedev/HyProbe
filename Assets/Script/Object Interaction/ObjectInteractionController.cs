using Seville;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SR
{
    public class ObjectInteractionController : MonoBehaviour
    {
        public static ObjectInteractionController instance;
        [SerializeField] private CharacteristicObjectController _characteristicObjectController;
        [SerializeField] private Vector3 _defaultCharacteristicTransform;

        private void Awake()
        {
            instance = this;
        }

        public void HideObjectInteraction()
        {
            _characteristicObjectController.gameObject.SetActive(false);
        }

        public void SetCharacteristicNPC(ABaseObjectData data)
        {
            //_characteristicObjectController.transform.parent = this.transform;
            //_characteristicObjectController.transform.localPosition = _defaultCharacteristicTransform;
            //_characteristicObjectController.gameObject.SetActive(false);
            //_characteristicObjectController.gameObject.SetActive(true);

            //_characteristicObjectController.SetCharacteristicData(data);
        }

    }
}
