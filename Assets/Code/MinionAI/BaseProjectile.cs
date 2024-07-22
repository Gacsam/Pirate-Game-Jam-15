using System.Collections;
using UnityEngine;

namespace Assets.Code.MinionAI
{
    public class BaseProjectile : MonoBehaviour
    {
        [SerializeField]
        bool isPiercing = false;
        [SerializeField]
        float rotationSpeed = 300, flightSpeed = 2, damage = 1;
        Vector3 enemyDirection;
        UnitSide allySide;


        // Use this for initialization
        public void Setup(Vector3 enemyDirection, UnitSide enemySide, float damage, bool isPiercing)
        {
            this.enemyDirection = enemyDirection;
            this.allySide = enemySide;
            this.damage = damage;
            this.isPiercing = isPiercing;
        }

        public void Setup(Vector3 enemyDirection, UnitSide enemySide)
        {
            this.enemyDirection = enemyDirection;
            this.allySide = enemySide;
        }

        // Update is called once per frame
        void Update()
        {
            this.transform.position += enemyDirection * Time.deltaTime * flightSpeed;
            this.transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null)
            {
                var targetObject = collision.GetComponent<BaseObject>();
                if (targetObject != null)
                {
                    if (allySide != targetObject.thisUnitSide)
                    {
                        // - cause we want to modify health by a negative value
                        targetObject.ModifyHealth(-damage);
                        // If not piercing destroy self
                        if (!isPiercing)
                        {
                            Destroy(this.gameObject);
                        }
                        // If is piercing continue until tower? Probably OP should add a timer
                        else if(targetObject.thisUnitType == UnitType.Tower)
                            {
                            Destroy(this.gameObject);
                        }
                    }
                }
            }
        }
    }
}