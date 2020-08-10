using Assets.Components.Level1.Interfaces;
using UnityEngine;

namespace Assets.Components.Level1.IsSomething
{
    public class BoxIsSomething
    {
        public FightingEnemy fightingEnemy;

        public bool IsEnemy()
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit))
            {
                if (raycastHit.collider.GetComponent<FightingEnemy>() != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool LockEnemy()
        {
            fightingEnemy = TraceEnemyUnderCursor();

            return fightingEnemy != null;
        }

        public float SqrDistanceToEnemy(Vector3 allyPosition)
        {
            FightingEnemy enemy = fightingEnemy ?? TraceEnemyUnderCursor();

            return enemy != null ? (enemy.gameObject.transform.position - allyPosition).sqrMagnitude : 0;
        }

        protected FightingEnemy TraceEnemyUnderCursor()
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit))
            {
                if (raycastHit.collider.TryGetComponent(out FightingEnemy isetAdvanceC))
                {
                    return isetAdvanceC;
                }
            }

            return null;
        }

        public bool HasLockedEnemy()
        {
            return fightingEnemy != null;
        }
    }
}