using System;
using System.Collections.Generic;
using System.Linq;
using Godot;


public partial class Effect
{
    /// <summary lang='zh-CN'>
    /// Gets or sets 物理攻击加成.
    /// </summary>
    public double PhysicalAttackPlus { get; set; }
    public double PhysicalAttackMultiply;
    public double MagicAttackPlus;
    public double MagicAttackMultiply;
    public double AttackSpeedPlus;
    public double AttackSpeedMultiply;
    public double AttackRangePlus;
    public double AttackRangeMultiply;
    public double BouncePlus;
    public double BounceMultiply;
    public double StunPlus;
    public double StunMultiply;

    // 单位属性效果
    public double RedHeartPlus;
    public double RedHeartMultiply;
    public double SoulHeartPlus;
    public double SoulHeartMultiply;
    public double PhysicalDefensePlus;
    public double PhysicalDefenseMultiply;
    public double MagicDefensePlus;
    public double MagicDefenseMultiply;
    public double MoveSpeedPlus;
    public double MoveSpeedMultiply;
    public double StunResistancePlus;
    public double StunResistanceMultiply;
    public double BounceResistancePlus;
    public double BounceResistanceMultiply;
    private double physicalAttackPlus;

    private enum SpecialEffect
    {
        Dash,
        DistanceDamage,
    }
}
