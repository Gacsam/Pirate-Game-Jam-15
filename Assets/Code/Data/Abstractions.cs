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
    protected float sliderOffset = 2.0f;
    protected Slider unitHealthSlider;

    // honestly dunno how to go about implementing poison .... im cooked
    protected bool poisoned = false;
    protected float damagePerSecond = 1f;


    // Add Box Collider if it doesn't have it
    protected void Awake()
    {
        if (this.GetComponent<Collider2D>() == null)
        {
            this.AddComponent<BoxCollider2D>();
        }
        thisUnitHealth = thisUnitMaxHealth;

        var sliderPrefab = Resources.Load<Slider>("Prefabs/Sliders/HealthBar");
        this.unitHealthSlider = Instantiate(sliderPrefab, Camera.main.GetComponentInChildren<Canvas>().transform.Find("Health UI"));
        Vector3 healthHeight = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        this.unitHealthSlider.transform.position = Camera.main.WorldToScreenPoint(healthHeight + (Vector3.down * sliderOffset));
        this.unitHealthSlider.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.Ceil(thisUnitHealth) + " / " + thisUnitMaxHealth;

        // If it's a tower, hook up to GameMan
        if (this.thisUnitType == UnitType.Tower)
        {
            if (this.thisUnitSide == UnitSide.Alchemy)
            {
                GameMan.Alchemy.Tower = this.GetComponent<BaseObject>();
            }
            else
            {
                GameMan.Shadow.Tower = this.GetComponent<BaseObject>();
            }
            // Make Tower bar slightly longer
            this.unitHealthSlider.GetComponent<RectTransform>().sizeDelta = this.unitHealthSlider.GetComponent<RectTransform>().sizeDelta + Vector2.right * 50;
        }
        else
        {
            this.unitHealthSlider.transform.localScale = Vector3.one * 0.5f;
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
    public float GetSpriteExtents()
    {
        return this.GetComponent<Collider2D>().bounds.extents.x;
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

        if (this.thisUnitHealth <= 0)
        {
            // Make sure unit is not set to Tower by accident
            if (this.thisUnitType != UnitType.Tower)
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
            this.unitHealthSlider.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.Ceil(thisUnitHealth) + " / " + thisUnitMaxHealth;
            return false;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Whether the unit was poisoned.</returns>
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
}
/// <summary>
/// Keep movement separated for things like turrets or defenders
/// </summary>
public abstract class BaseUnit : BaseObject
{
    protected float attackTimer = 0;
    public int unitCost = 30;
    public int baseDamage = 2;
    public float attackCooldown = 1;


    // Draw ray in editor to show distance
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Offset half a sprite towards enemy   
        var offset = GetEnemyTowerDirection() * this.GetSpriteExtents();
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
        // Continously update position just in case there's knockback, falling or anything
        Vector3 healthHeight = new Vector3(this.transform.position.x, GameMan.Alchemy.Tower.transform.position.y, this.transform.position.z);
        this.unitHealthSlider.transform.position = Camera.main.WorldToScreenPoint(healthHeight + (Vector3.down * sliderOffset));

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
        var spriteOffsets = GameMan.GetClosestEnemy(thisUnitSide).GetSpriteExtents() + this.GetSpriteExtents(); // we use sprite offsets to get true distance between units' edges
        return Mathf.Abs(GameMan.GetClosestEnemy(thisUnitSide).transform.position.x - this.transform.position.x) - spriteOffsets;
    }

    /// <summary>
    /// Modify health by the base damage of this unit
    /// </summary>
    /// <returns>Whether the unit has died</returns>
    protected bool DamageClosestEnemy()
    {
        return GameMan.GetClosestEnemy(thisUnitSide).ModifyHealth(-this.baseDamage);
    }

    protected void MockAttack()
    {
        // Timer represents animations n all        
        this.attackTimer += Time.deltaTime;
        // When the attack animation is done
        if (this.attackTimer > this.attackCooldown)
        {
            // Double check the enemy unit still exist
            if (GameMan.GetClosestEnemy(thisUnitSide) != null)
            {
                // Reset the timer and damage closest enemy unit
                this.attackTimer = 0;
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
    private bool isPushed = false;
    /// <summary>
    /// Attempt to move towards enemy tower based on movement speed
    /// </summary>
    public void MoveTowardsOppositeTower()
    {
        if (isPushed)
        {
            var rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity *= Vector2.one * 0.9f;
                if(Mathf.Abs(rb.velocity.x) <= 0.1f)
                {
                    rb.velocity = Vector2.zero;
                    isPushed = false;
                }
            }
        }
        else
        {
            // Force all health bars to be on Tower's health bar height
            var checkPath = UnitInPath();
            // Check if ally unit isn't blocking the path
            if (checkPath == null)
            {
                this.gameObject.transform.position += MovementSpeed * Time.deltaTime * GetEnemyTowerDirection();
            }
            else if (this is IHealing healer)
            {
                if (checkPath.gameObject.GetComponent<BaseObject>().thisUnitSide == thisUnitSide)
                {
                    healer.HealAllyInFront(checkPath.gameObject.GetComponent<BaseObject>());
                }
            }
        }
    }

    public void KnockbackEffect(Vector2 pushForce)
    {
        if (GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<Rigidbody2D>().AddForce(pushForce, ForceMode2D.Impulse);
            isPushed = true;    
        }
    }

    /// <summary>
    /// Draws a raycast towards the enemy tower direction.
    /// Meant to check if path is free, rather than having sprites pile on top of each other.
    /// </summary>
    /// <returns>Whether there is no collider in front of the unit within globalMeleeRange</returns>
    protected Collider2D UnitInPath()
    {
        // Multiply direction by horizontal offset to create a ray right at edge of character
        Vector3 offset = GetEnemyTowerDirection() * this.GetComponent<Collider2D>().bounds.extents.x;
        // Create a raycast of X steps where you can't move further
        RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, GetEnemyTowerDirection(), GameMan.globalMeleeRange, ~LayerMask.GetMask("Ignore Raycast"));
        // If there's nothing in front of us for X distance, say path blocked or nah
        return hit.collider;
    }
}

public abstract class BaseProjectile : MonoBehaviour
{
    [SerializeField]
    protected bool isPiercing = false;
    [SerializeField]
    protected float rotationSpeed = 300, flightSpeed = 2, damage = 1;
    protected Vector3 enemyTowerDirection;
    protected UnitSide allySide;

    public void Setup(Vector3 enemyDirection, UnitSide allySide)
    {
        this.enemyTowerDirection = enemyDirection;
        this.allySide = allySide;
    }

    // Update is called once per frame
    void Update()
    {
        if (this is IMoving move)
        {
            move.MoveTowardsOppositeTower();
        }
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