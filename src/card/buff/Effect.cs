using System;
using System.Collections.Generic;
using System.Linq;
using Godot;


    public partial class Effect
    {
        //卡牌属性效果
        public double PhysicalAttackPlus, PhysicalAttackMultiply, MagicAttackPlus, MagicAttackMultiply, AttackSpeedPlus, AttackSpeedMultiply, AttackRangePlus, AttackRangeMultiply, BouncePlus, BounceMultiply, StunPlus, StunMultiply;
    
        //单位属性效果
        public double RedHeartPlus, RedHeartMultiply, SoulHeartPlus, SoulHeartMultiply, PhysicalDefensePlus,PhysicalDefenseMultiply,MagicDefensePlus,MagicDefenseMultiply,MoveSpeedPlus,MoveSpeedMultiply,StunResistancePlus,StunResistanceMultiply,BounceResistancePlus,BounceResistanceMultiply;

        enum SpecialEffect
        {
            Dash, DistanceDamage
    }
}

