using System.Collections;
using UnityEngine;

namespace Assets.Code.MinionAI
{
    public class StraightProjectile : BaseProjectile
    {
        [SerializeField]
        private bool isSpinning = false;
        void Update()
        {
            if(targetPosition == null){Destroy(gameObject);}
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, flightSpeed * Time.deltaTime);
            if (isSpinning)
            {
                transform.Rotate(new Vector3(0,0,rotationSpeed * Time.deltaTime));
            }
        }

        protected override void ProjectileImpact(GameObject objectThatWasHit)
        {
            if (objectThatWasHit.GetComponent<BaseObject>() != null)
            {
                var theUnitHit = objectThatWasHit.GetComponent<BaseObject>();
                if (theUnitHit.thisUnitSide != allySide)
                {
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
}