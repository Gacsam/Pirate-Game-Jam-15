using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// Abstract class/variables basically means we want these methods in every inherited script down the line, or something
// Allows us to call the functions all the way from down here without having to write it or run some checks twice etc 

/// <summary>
///  Represents a basic destructible object, use this to extend tower etc
/// </summary>
public abstract class BaseObject : MonoBehaviour
{
    public UnitSide thisUnitSide = 0;
    public UnitType thisUnitType = 0;
    protected float thisUnitHealth = 10;
    [SerializeField]
    protected float thisUnitMaxHealth = 10;
    [SerializeField]
    protected Vector3 sliderOffset = Vector3.up;
    protected Slider unitHealthSlider;

    // honestly dunno how to go about implementing poison .... im cooked
    protected bool poisoned = false;
    protected float damagePerSecond = 1f;


    // Add Box Collider if it doesn't have it
    protected void Awake()
    {
        if (GetComponent<Collider2D>() == null)
        {
            this.AddComponent<BoxCollider2D>();
        }
        thisUnitHealth = thisUnitMaxHealth;

        var sliderPrefab = Resources.Load<Slider>("Prefabs/Sliders/HealthBar");
        if (Camera.main.GetComponentInChildren<Canvas>() == null) { throw new System.NotImplementedException(); } // parent UI_Canvas to Camera
        unitHealthSlider = Instantiate(sliderPrefab, Camera.main.GetComponentInChildren<Canvas>().transform.Find("Health UI"));

        Vector3 healthHeight = new(transform.position.x, transform.position.y, transform.position.z);
        
        unitHealthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.position + sliderOffset);
        unitHealthSlider.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.Ceil(thisUnitHealth) + " / " + thisUnitMaxHealth;

        // If it's a tower, hook up to GameMan
        if (thisUnitType == UnitType.Tower)
        {
            if (thisUnitSide == UnitSide.Alchemy)
            {
                GameMan.Alchemy.Tower = GetComponent<BaseObject>();
            }
            else
            {
                GameMan.Shadow.Tower = GetComponent<BaseObject>();
            }
            // Make Tower bar slightly longer
            unitHealthSlider.GetComponent<RectTransform>().sizeDelta = unitHealthSlider.GetComponent<RectTransform>().sizeDelta + Vector2.right * 50;
        }
        else
        {
            unitHealthSlider.transform.localScale = Vector3.one * 0.5f;
        }
    }

    public float GetInjuryPoints()
    {
        return thisUnitMaxHealth - thisUnitHealth;
    }

    protected Vector3 GetEnemyTowerDirection()
    {
        // Small if/else statement to check for player direction
        return thisUnitSide == UnitSide.Alchemy ? Vector3.right : Vector3.left;
    }

    /// <summary>
    /// Gets the sprite's X extents, equal to half the size.
    /// </summary>
    /// <returns>This specific unit's Collider2D.extents.x</returns>
    public Vector3 GetSpriteExtents()
    {
        return GetComponent<SpriteRenderer>().sprite.bounds.extents;
    }
    /// <summary>
    /// Take damage function, will allow for modifications based on damage type dealt
    /// </summary>
    /// <param name="modifier"></param>
    /// <param name="typeOfDamage"></param>
    /// <returns>Whether the unit died</returns>
    public bool ModifyHealth(float modifier)
    {

        // apply damage
        thisUnitHealth += modifier;
        thisUnitHealth = Mathf.Clamp(thisUnitHealth, 0, thisUnitMaxHealth);

        if (thisUnitHealth <= 0)
        {
            // Make sure unit is not set to Tower by accident
            if (thisUnitType != UnitType.Tower)
            {
                if (thisUnitSide == UnitSide.Alchemy)
                {
                    GameMan.Alchemy.UnitDied();
                }
                else
                {
                    GameMan.Shadow.UnitDied();
                }

                // if it's an enemy and not a tower
                if (thisUnitSide == UnitSide.Shadow && (thisUnitType != UnitType.Tower))
                {
                    ItemDrop.DropItem();
                }
            }
            else
            {
                Debug.Log("LUKE, I AM YOUR TOWER");
            }
            // Lets individual classes deal with destruction in their own way
            Destroy(unitHealthSlider.gameObject);
            HandleDestruction();
            return true;
        }
        else
        {
            unitHealthSlider.value = thisUnitHealth / thisUnitMaxHealth;
            unitHealthSlider.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.Ceil(thisUnitHealth) + " / " + thisUnitMaxHealth;
            return false;
        }
    }
    /// <summary>
    /// <returns>Whether the unit was poisoned.</returns>
    /// </summary>
    public bool IsPoisoned(){return poisoned;}

    public bool ApplyPoison()
    {
        if (true)
        {   // Can put in some checks for immunity later on
            poisoned = true;
        }
        return poisoned;
    }
    /// <summary>
    /// Called by TakeDamage if the unit's health reaches 0.
    /// </summary>
    protected abstract void HandleDestruction();

    /// </summary>
    /// Update healthbar relative to UI
    /// </summary>
    
    public void UpdateHealthSlider(){
        Vector3 healthHeight = new (transform.position.x, GameMan.Alchemy.Tower.transform.position.y, transform.position.z);

        // tower's health slider should be pushed to the centre a bit since camera does not go past half of the tower
        if(thisUnitType==UnitType.Tower){
            if(thisUnitSide==UnitSide.Alchemy){healthHeight.x += 1f;}
            else{healthHeight.x -= 1f;}
        }
        unitHealthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.position + sliderOffset);
    }

}
/// <summary>
/// Keep movement separated for things like turrets or defenders
/// </summary>
public abstract class BaseUnit : BaseObject
{
    protected float attackTimer = 0;
    public int baseDamage = 2;
    public float attackCooldown = 1;


    // Draw ray in editor to show distance
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Offset half a sprite towards enemy   
        var offset = GetEnemyTowerDirection() * GetSpriteExtents().x;
        offset.y = 0;
        offset += Vector3.up * GetSpriteExtents().y;
        if (this is IRanged r)
        {
            Gizmos.DrawRay(transform.position + offset, GetEnemyTowerDirection() * r.AttackRange);
        }
        else
        {
            Gizmos.DrawRay(transform.position + offset, GetEnemyTowerDirection() * GameMan.globalMeleeRange);
        }
    }

    public void Update()
    {         
        // Continuously update position just in case there's knockback, falling or anything
        unitHealthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.position + sliderOffset);

        // If GameMan exists and if we have a target which should always be true
        if (GameMan.Instance != null && GameMan.GetClosestEnemy(thisUnitSide) != null)
        {
            var enemyDistance = GetClosestTargetDistance();
            // If the unit has a IRanged interface and the enemy is within our attack range
            if (this is IRanged rangedAttack && rangedAttack.AttackRange >= enemyDistance)
            {
                // First check if he's not in melee range
                // If true AND the unit can melee, do melee
                if (GameMan.globalMeleeRange > enemyDistance && this is IMelee meleeAttack)
                {
                    meleeAttack.Attack();
                    Debug.Log("Enemy within range, forced to engage melee.");
                }
                else
                {
                    rangedAttack.Attack();
                    Debug.Log("Enemy within range, engaging ranged.");
                }
            }
            // Else if it has a melee interface and enemy is within melee range
            else if (this is IMelee meleeAttack && GameMan.globalMeleeRange > enemyDistance)
            {
                meleeAttack.Attack();
                Debug.Log("Enemy within range, engaging melee.");
            }
            // Otherwise assume enemy is just not within range
            else if (this is IMoving movingChar)
            {
                movingChar.MoveTowardsOppositeTower();
                Debug.Log("Enemy not within range. Advancing.");
            }
            else
            {
                Debug.Log("Enemy not within range. Turret(?) waiting.");
            }
        }

        if(poisoned){
            ModifyHealth(-damagePerSecond*Time.deltaTime);
        }
    }

    /// <summary>
    /// Get distance to closest enemy unit or tower
    /// </summary>
    /// <returns>Closest enemy unit or tower</returns>
    public float GetClosestTargetDistance()
    {
        Bounds myBounds = this.GetComponent<Renderer>().bounds;
        Bounds enemyBounds = GameMan.GetClosestEnemy(thisUnitSide).GetComponent<Renderer>().bounds;
        if (enemyBounds.max.x < myBounds.min.x)
        {
            return myBounds.min.x - enemyBounds.max.x;
        }
        else if (myBounds.max.x < enemyBounds.min.x)
        {
            return enemyBounds.min.x - myBounds.max.x;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// Modify health by the base damage of this unit
    /// </summary>
    /// <returns>Whether the unit has died</returns>
    protected bool DamageClosestEnemy()
    {
        return GameMan.GetClosestEnemy(thisUnitSide).ModifyHealth(-baseDamage);
    }

    protected void MockAttack()
    {
        // Timer represents animations n all        
        attackTimer += Time.deltaTime;
        // When the attack animation is done
        if (attackTimer > attackCooldown)
        {
            // Double check the enemy unit still exist
            if (GameMan.GetClosestEnemy(thisUnitSide) != null)
            {
                // Reset the timer and damage closest enemy unit
                attackTimer = 0;
                DamageClosestEnemy();

                // just realised youve done it .... bruh moment where i waste my time. Thats what i get for forgetting your discord message lol, you literally SAID YOUVE DONE IT
                // // special effects (borax)
                // if(thisUnitDamageType == DamageType.Borax){
                //     GameMan.HealCloseAllies(thisUnitSide,gameObject.GetComponent<BaseObject>());
                // }


            }
        }
    }
}

public abstract class BaseMovingUnit : BaseUnit, IMoving
{
    float movementSpeed = 1;
    public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }
    /// <summary>
    /// Attempt to move towards enemy tower based on movement speed
    /// </summary>
    public void MoveTowardsOppositeTower()
    {
        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            if (rb.velocity != Vector2.zero)
            {
                StartCoroutine(ResetRB());
            }
        }

        // Force all health bars to be on Tower's health bar height
        var checkPath = UnitInPath();
        // Check if ally unit isn't blocking the path
        if (checkPath == null)
        {
            gameObject.transform.position += MovementSpeed * Time.deltaTime * GetEnemyTowerDirection();
        }
        else if (this is IHealing healer)
        {
            if (checkPath.gameObject.GetComponent<BaseObject>().thisUnitSide == thisUnitSide)
            {
                healer.HealAllyInFront(checkPath.gameObject.GetComponent<BaseObject>());
            }
        }
    }

    public void KnockbackEffect(Vector2 pushForce)
    {
        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.AddForce(pushForce, ForceMode2D.Impulse);
        }
    }

    private IEnumerator ResetRB()
    {
        yield return new WaitForSeconds(0.1f);
        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.velocity = Vector2.zero;
        }
    }

    /// <summary>
    /// Draws a raycast towards the enemy tower direction.
    /// Meant to check if path is free, rather than having sprites pile on top of each other.
    /// </summary>
    /// <returns>Whether there is no collider in front of the unit within globalMeleeRange</returns>
    protected Collider2D UnitInPath()
    {
        var offset = GetEnemyTowerDirection() * GetSpriteExtents().x;
        offset.y = 0;
        offset += Vector3.up * GetSpriteExtents().y;
        // Create a raycast of X steps where you can't move further
        RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, GetEnemyTowerDirection(), GameMan.globalMeleeRange, ~LayerMask.GetMask("Ignore Raycast", "Ground"));
        // If there's nothing in front of us for X distance, say path blocked or nah
        if (hit)
        {
            if (hit.transform.gameObject.GetComponent<BaseMovingUnit>() != null) return hit.collider;
        }
        return null;
    }
}

public abstract class BaseProjectile : MonoBehaviour
{
    [SerializeField]
    protected bool isPiercing = false;
    [SerializeField]
    protected float rotationSpeed = 300, flightSpeed = 2, damage = 1;
    protected Vector3 targetPosition;
    protected UnitSide allySide;

    public void Setup(Vector3 enemyDirection, UnitSide allySide)
    {
        targetPosition = enemyDirection;
        this.allySide = allySide;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Make sure collision occurred
        if (collision != null)
        {
            ProjectileImpact(collision.gameObject);
        }
    }

    protected abstract void ProjectileImpact(GameObject enemyThatWasHit);
}