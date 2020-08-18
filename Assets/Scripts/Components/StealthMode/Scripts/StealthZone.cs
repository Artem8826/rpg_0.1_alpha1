using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Components.StealthMode.Scripts
{
    public sealed class StealthZone : MonoBehaviour
    {
        #region Fields

        public event Action StealthZoneEnter;
        public event Action StealthZoneExit;

        public GameObject[] RecognizedObjects;

        public float StealthRadius = 3;
        public Color SphereColor = Color.black;

        private List<GameObject> _inStealthZone = new List<GameObject>();

        #endregion


        #region UnityMethods

        private void Start()
        {
            if (TryGetComponent(out SphereCollider objectCollider))
            {
                objectCollider.radius = StealthRadius;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = SphereColor;

            Gizmos.DrawWireSphere(transform.position, StealthRadius);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (RecognizedObjects.Any(obj => obj == other.gameObject))
            {
                _inStealthZone.Add(other.gameObject);

                StealthZoneEnter?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (RecognizedObjects.Any(obj => obj == other.gameObject))
            {
                _inStealthZone.Remove(other.gameObject);

                StealthZoneExit?.Invoke();
            }
        }

        #endregion


        #region Methods
        public bool IsInStealthMode(GameObject gameObject)
        {
            return _inStealthZone.Any(o => o == gameObject);
        } 
        #endregion
    }
}
