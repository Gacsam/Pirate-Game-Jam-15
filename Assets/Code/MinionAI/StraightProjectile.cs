using System.Collections;
using UnityEngine;

namespace Assets.Code.MinionAI
{
    public class StraightProjectile : BaseProjectile, IMoving
    {
        float IMoving.MovementSpeed { get { return flightSpeed; } set { flightSpeed = value; } }
        void IMoving.MoveTowardsOppositeTower()
        {
            transform.position += enemyTowerDirection * (flightSpeed * Time.deltaTime);
        }

        protected override void ProjectileImpact(GameObject objectThatWasHit)
        {
            if (objectThatWasHit.GetComponent<BaseObject>() != null)
            {
                var theUnitHit = objectThatWasHit.GetComponent<BaseObject>();
                // - cause we want to modify health by a negative value
                theUnitHit.ModifyHealth(-damage);
                // If not piercing destroy self
                if (!isPiercing)
                {
                    Destroy(this.gameObject);
                }
                // If is piercing continue until tower? Probably OP should add a timer
                else if (theUnitHit.thisUnitType == UnitType.Tower)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}