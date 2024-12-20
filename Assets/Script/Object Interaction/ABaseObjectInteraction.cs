using Seville;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;

namespace SR
{
    public abstract class ABaseObjectInteraction : MonoBehaviour
    {
        [SerializeField] protected ABaseObjectData objectData;
        [SerializeField] protected GameObject _objectModel;
        [SerializeField] protected Collider _collider;
        public ObjectType objectType;
        [SerializeField] protected Rigidbody _objectRB;

        [Header("Material Animation")]
        [SerializeField] private List<Material> _listMaterials = new List<Material>();
        private int frameCount = 0;
        private readonly int DissolveHide = Shader.PropertyToID("_DissolveHide");
        private readonly int RandomGlitchAmount = Shader.PropertyToID("_RandomGlitchAmount");
        private readonly int RandomGlitchConstant = Shader.PropertyToID("_RandomGlitchConstant");
        private Tween rotationTween;

        [SerializeField] private Vector3 _defaultCharacteristicTransform;
        [SerializeField] private Transform _parent;

        public ABaseObjectData GetObjectData()
        {
            _objectRB.isKinematic = true;
            return objectData;
        }

        public void SpawnHologramEffect()
        {

        }

        public void SetSizeObject(bool condition)
        {
            if (condition)
            {
                rotationTween = _objectModel.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.5f);
                //transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360)
                //    .SetLoops(-1, LoopType.Incremental) // Loop tak terbatas
                //    .SetEase(Ease.Linear); // Rotasi linear tanpa perlambatan
            }
            else
            {
                _objectModel.transform.DOScale(Vector3.one, 0.5f);
                //rotationTween.Kill(); // Menghentikan tween
            }
        }

        public void OnExitGrabCondition()
        {
            StartCoroutine(ReturnPosition());
        }

        public IEnumerator ReturnPosition()
        {
            yield return new WaitForSeconds(5f);

            _objectRB.velocity = Vector3.zero;
            //_objectRB.isKinematic = true;
            this.transform.parent = _parent.transform;
            this.transform.localPosition = _defaultCharacteristicTransform;
        }
    }
}


public enum ObjectType
{
    CharacteristicObject,
    MessageObject,
}
