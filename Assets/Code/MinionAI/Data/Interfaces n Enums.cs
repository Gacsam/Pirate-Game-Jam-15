using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMelee
{
    void Attack();
}

public interface IRanged
{
    public float AttackRange { get; set; }
    void Attack();
}

public interface IMoving
{
    public float MovementSpeed { get; set; }
    void MoveTowardsOppositeTower();
}

public interface IHealing
{
    public float HealPerSecond { get; set; }
    void HealAllyInFront(BaseObject healTarget);
}

public enum DamageType { Standard, Fire, Arsenic, Moon, Borax }
public enum UnitSide { Alchemy, Shadow }
public enum UnitType { Melee, Ranged, Both, Tower }