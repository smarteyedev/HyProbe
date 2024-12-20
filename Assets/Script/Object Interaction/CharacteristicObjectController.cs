using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SR
{
    public class CharacteristicObjectController : ABaseObjectInteraction
    {
        public void Start()
        {
            objectType = ObjectType.CharacteristicObject;
        }

        public void SetCharacteristicData(ABaseObjectData data)
        {
            _objectModel.transform.localScale = Vector3.one;
            _objectRB.velocity = Vector3.zero;
            objectData = data;
            StartCoroutine(DisableKinematic());
        }

        public IEnumerator DisableKinematic()
        {
            yield return new WaitForSeconds(1f);
            _objectRB.isKinematic = false;
        }
    }
}
