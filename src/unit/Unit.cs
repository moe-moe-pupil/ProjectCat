using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Newtonsoft.Json;

/// <summary>
///   游戏中的主要单位，可以是角色、敌人，甚至是陷阱机关
/// </summary>
public class Unit
{
    /// <summary>
    ///   单位的基础生命值
    /// </summary>
    public double RedHeart;
    /// <summary>
    ///   单位的额外生命值
    /// </summary>
    public double SoulHeart;
     /// <summary>
    ///   单位对物理攻击的抗性
    /// </summary>
    public int PhysicalDefense;
     /// <summary>
    ///   单位对魔法攻击的抗性
    /// </summary>
    public int MagicDefense;
    /// <summary>
    ///   单位的移动速度
    /// </summary>
    public int MoveSpeed;
    /// <summary>
    ///   单位的眩晕抗性
    /// </summary>
    private int StunResistance;
    /// <summary>
    ///   单位的击退抗性
    /// </summary>
    private int BounceResistance;
    /// <summary>
    ///   单位目前装备的卡牌的编号
    /// </summary>
    public List<int> EquipmentBar = new List<int>();
}