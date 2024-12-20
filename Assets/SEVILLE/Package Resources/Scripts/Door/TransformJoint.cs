using UnityEngine;

namespace Seville
{
    public class TransformJoint : MonoBehaviour
    {
        public Transform connectedBody;

        private const float _minMass = 0.01f;
        private const float _maxForceDistance = 0.01f;
        private Vector3 _anchor, _connectedAnchor;
        Rigidbody _rigidbody;
        private float _baseMass = 1f;
        private float _appliedForce;

        [Header("Value")]
        [SerializeField] private float _baseForce = 0.25f;
        [SerializeField] private float _springForce = 1f;
        [SerializeField] private float _breakDistance = 1.5f;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            if (_rigidbody.mass > _minMass)
                _baseMass = _rigidbody.mass;

            SetupConnectedBodies();
        }

        void SetupConnectedBodies()
        {
            if (connectedBody)
            {
                _connectedAnchor = connectedBody.InverseTransformPoint(_rigidbody.position + Vector3.Scale(_rigidbody.rotation * _anchor, transform.lossyScale));
            }
        }

        void LateUpdate()
        {
            transform.position = _rigidbody.position;
        }

        void FixedUpdate()
        {
            UpdatePosition();
        }

        void UpdatePosition()
        {
            Vector3 worldSourceAnchor = _rigidbody.position + Vector3.Scale(_rigidbody.rotation * _anchor, transform.lossyScale);
            Vector3 worldDestAnchor = connectedBody.TransformPoint(_connectedAnchor);
            Vector3 positionDelta = worldDestAnchor - worldSourceAnchor;
            float offset = positionDelta.magnitude;

            if (offset > Mathf.Epsilon)
            {
                if (offset > _breakDistance)
                {
                    _rigidbody.position = worldDestAnchor;
                    transform.position = worldDestAnchor;
                }
                else
                {
                    _appliedForce = _baseForce + offset * _springForce;
                    _rigidbody.AddForce(positionDelta.normalized * _appliedForce, ForceMode.Impulse);
                }
            }
        }
    }
}