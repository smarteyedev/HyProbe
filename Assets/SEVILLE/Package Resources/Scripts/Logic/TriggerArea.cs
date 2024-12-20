using System;
using UnityEngine;
using UnityEngine.Events;

namespace Seville
{
    public class TriggerArea : MonoBehaviour
    {
        public string targetObjectTag;

        public TriggerEvent triggerAreaEvent;

        [Header("Component Dependencies")]
        private MeshRenderer m_meshRenderer;

        void Awake()
        {
            m_meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            if (m_meshRenderer && m_meshRenderer.enabled) m_meshRenderer.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == targetObjectTag)
            {
                triggerAreaEvent.OnObjectEnter?.Invoke();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == targetObjectTag)
            {
                triggerAreaEvent.OnObjectStay?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == targetObjectTag)
            {
                triggerAreaEvent.OnObjectExit?.Invoke();
            }
        }
    }

    [Serializable]
    public class TriggerEvent
    {
        [Header("Trigger Event")]
        [Space(2f)]
        public UnityEvent OnObjectEnter;
        [Space(3f)]
        public UnityEvent OnObjectStay;
        [Space(3f)]
        public UnityEvent OnObjectExit;
    }
}