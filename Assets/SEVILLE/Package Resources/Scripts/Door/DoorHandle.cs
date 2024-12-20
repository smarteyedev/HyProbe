using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace Seville
{
    public class DoorHandle : XRBaseInteractable
    {
        private const float _switchZone = 0.1f;
        struct TrackedRotation
        {
            float _baseAngle;
            float _currentOffset;
            float _accumulatedAngle;
            public float totalOffset => _accumulatedAngle + _currentOffset;
            public void Reset()
            {
                _baseAngle = 0.0f;
                _currentOffset = 0.0f;
                _accumulatedAngle = 0.0f;
            }

            public void SetBaseFromVector(Vector3 direction)
            {
                _accumulatedAngle += _currentOffset;
                _baseAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                _currentOffset = 0.0f;
            }

            public void SetTargetFromVector(Vector3 direction)
            {
                var targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                _currentOffset = ShortestAngleDistance(_baseAngle, targetAngle, 360.0f);
            }
        }

        [Serializable] public class ValueChangeEvent : UnityEvent<float> { }

        [SerializeField] private Transform _handle = null;

        [SerializeField][Range(0.0f, 1.0f)] private float _value = 0.5f;

        [Header("Handle Configuration")]
        [SerializeField] bool _clampedMotion = true;
        [SerializeField] float _maxAngle = 0f;
        [SerializeField] float _minAngle = -90.0f;
        [SerializeField] float _positionTrackedRadius = 0.1f;
        [SerializeField] float _twistSensitivity = 2f;

        [SerializeField]
        ValueChangeEvent _onValueChange = new ValueChangeEvent();

        IXRSelectInteractor _interactor;
        bool _positionDriven = false;
        bool _upVectorDriven = false;

        TrackedRotation _positionAngles = new TrackedRotation();
        TrackedRotation _upVectorAngles = new TrackedRotation();
        float _baseKnobRotation = 0.0f;

        void Start()
        {
            SetValue(_value);
            SetKnobRotation(ValueToRotation());
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            selectEntered.AddListener(StartGrab);
            selectExited.AddListener(EndGrab);
        }

        protected override void OnDisable()
        {
            selectEntered.RemoveListener(StartGrab);
            selectExited.RemoveListener(EndGrab);
            base.OnDisable();
        }

        void StartGrab(SelectEnterEventArgs args)
        {
            _interactor = args.interactorObject;
            _positionAngles.Reset();
            _upVectorAngles.Reset();
            UpdateBaseKnobRotation();
        }

        void EndGrab(SelectExitEventArgs args)
        {
            _interactor = null;
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic && isSelected)
            {
                UpdateRotation();
            }
        }

        void UpdateRotation()
        {
            var interactorTransform = _interactor.GetAttachTransform(this);
            var localOffset = transform.InverseTransformVector(interactorTransform.position - _handle.position);
            localOffset.z = 0.0f;
            float radiusOffset = localOffset.magnitude;
            localOffset.Normalize();

            var localUp = transform.InverseTransformDirection(interactorTransform.up);
            localUp.z = 0.0f;
            localUp.Normalize();

            bool positionDriven = radiusOffset >= _positionTrackedRadius;
            bool upVectorDriven = localUp.z > 0.707f;

            if (positionDriven && !_positionDriven)
            {
                _positionAngles.SetBaseFromVector(localOffset);
            }
            _positionDriven = positionDriven;

            if (upVectorDriven && !_upVectorDriven)
            {
                _upVectorAngles.SetBaseFromVector(localUp);
            }
            _upVectorDriven = upVectorDriven;

            if (_positionDriven)
                _positionAngles.SetTargetFromVector(localOffset);

            if (_upVectorDriven)
                _upVectorAngles.SetTargetFromVector(localUp);

            var knobRotation = _baseKnobRotation - (_upVectorAngles.totalOffset + _positionAngles.totalOffset) * _twistSensitivity;

            if (_clampedMotion)
                knobRotation = Mathf.Clamp(knobRotation, _minAngle, _maxAngle);

            SetKnobRotation(knobRotation);

            var knobValue = Mathf.InverseLerp(_minAngle, _maxAngle, knobRotation);
            SetValue(knobValue);
        }

        void SetKnobRotation(float angle)
        {
            if (_handle != null)
                _handle.localEulerAngles = new Vector3(0.0f, 0.0f, angle);

            // Debug.Log(angle);
        }

        void SetValue(float value)
        {
            if (_clampedMotion)
                value = Mathf.Clamp01(value);

            _value = value;
            _onValueChange.Invoke(_value);
        }

        float ValueToRotation()
        {
            return _clampedMotion ? Mathf.Lerp(_minAngle, _maxAngle, _value) : Mathf.LerpUnclamped(_minAngle, _maxAngle, _value);
        }

        void UpdateBaseKnobRotation()
        {
            _baseKnobRotation = Mathf.LerpUnclamped(_minAngle, _maxAngle, _value);
        }

        static float ShortestAngleDistance(float start, float end, float max)
        {
            var angleDelta = end - start;
            var angleSign = Mathf.Sign(angleDelta);

            angleDelta = Math.Abs(angleDelta) % max;
            if (angleDelta > (max * 0.5f))
                angleDelta = -(max - angleDelta);

            return angleDelta * angleSign;
        }

        void OnValidate()
        {
            if (_clampedMotion)
                _value = Mathf.Clamp01(_value);

            if (_minAngle > _maxAngle)
                _minAngle = _maxAngle;

            SetKnobRotation(ValueToRotation());
        }
    }
}