using UnityEngine;

namespace Assets.Components.Level1.CodeOnly.TimeLocker
{
    public sealed class TimeLocker
    {
        private float _secTimer;
        private float _updateSecTimer;

        public void SetTimer(float timer)
        {
            _secTimer = timer;
        }

        public void GoTimer()
        {
            _updateSecTimer -= Time.deltaTime;
        }

        public bool IsTimeToGo()
        {
            if (_updateSecTimer < 0)
            {
                _updateSecTimer = _secTimer;

                return true;
            }

            return false;
        }
    }
}
