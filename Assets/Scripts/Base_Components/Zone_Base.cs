using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Base_Components
{
    public abstract class Zone_Base : MonoBehaviour
    {
        public float MinSize = 1f;
        public float MaxSize = 3f;
        public float scaleTime = 1f;
        public float initialScale = 0.3f;
        
        private void Start()
        {
            float size = Random.Range(MinSize, MaxSize);
            transform.DOScale(Vector3.one * size, scaleTime).From(initialScale);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.attachedRigidbody.gameObject.TryGetComponent(out Unit_Base unit))
            {
                UnitInside(unit);
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.attachedRigidbody.gameObject.TryGetComponent(out Unit_Base unit))
            {
                UnitEnter(unit);
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.attachedRigidbody.gameObject.TryGetComponent(out Unit_Base unit))
            {
                UnitExit(unit);
            }
        }

        protected abstract void UnitInside(Unit_Base unit);
        protected abstract void UnitEnter(Unit_Base unit);
        protected abstract void UnitExit(Unit_Base unit);

    }
}