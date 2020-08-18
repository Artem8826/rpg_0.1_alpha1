using Assets.Components.StealthMode.Scripts;
using UnityEngine;

namespace Assets
{
    public class StealthBorderController : StealthController
    {
        public bool IsInStealthZoneBorder(GameObject gameObject)
        {
            return IsInStealthZone(gameObject);
        }
    }
}
