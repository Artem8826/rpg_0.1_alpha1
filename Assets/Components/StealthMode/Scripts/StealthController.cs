using UnityEngine;

namespace Assets.Components.StealthMode.Scripts
{
    public class StealthController : MonoBehaviour
    {
        #region Fields

        public StealthZone[] StealthZones = new StealthZone[0];
        public GameObject[] StealthObjects = new GameObject[0]; 

        #endregion


        #region UnityMethods

        protected void OnEnable()
        {
            foreach (StealthZone stealthZone in StealthZones)
            {
                stealthZone.StealthZoneEnter += DStealthZoneEnter;
                stealthZone.StealthZoneExit += DStealthZoneExit;
            }
        }

        protected void Start()
        {
            if (StealthZones.Length == 0)
            {
                StealthZones = FindObjectsOfType<StealthZone>();
            }

            foreach (StealthZone stealthZone in StealthZones)
            {
                stealthZone.RecognizedObjects = StealthObjects;
            }

            OnEnable();
        }

        protected void Update()
        {

        }

        protected void OnDisable()
        {
            foreach (StealthZone stealthZone in StealthZones)
            {
                stealthZone.StealthZoneEnter -= DStealthZoneEnter;
                stealthZone.StealthZoneExit -= DStealthZoneExit;
            }
        }

        #endregion


        #region Methods

        protected virtual bool IsInStealthZone(GameObject gameObject)
        {
            foreach (StealthZone stealthZone in StealthZones)
            {
                if (stealthZone.IsInStealthMode(gameObject))
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual void DStealthZoneEnter()
        {
        }

        protected virtual void DStealthZoneExit()
        {
        } 

        #endregion
    }
}
