using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Newtonsoft.Json;

/// <summary>
///   游戏中的主要单位，可以是角色、敌人，甚至是陷阱机关
/// </summary>
public class EquippedCards
{
    /// <summary>
    ///   卡牌当前的物理攻击力
    /// </summary>
    private double PhysicalAttack;

    /// <summary>
    ///   卡牌当前的魔法攻击力
    /// </summary>
    private double MagicAttack;
    
    /// <summary>
    ///   卡牌效果的触发间隔 n次/秒
    /// </summary>
    private double AttackSpeed;

    /// <summary>
    ///   卡牌效果的生效范围
    /// </summary>
    private double AttackRange;
    
    /// <summary>
    ///   卡牌对单位造成的位移效果的强度
    /// </summary>
    private int Bounce;

    /// <summary>
    ///   卡牌对单位造成的硬直效果的强度
    /// </summary>
    private int Stun;
    /// <summary>
    ///   当前卡牌的标签，包括武器、伤害类型等
    /// </summary>
    public List<String> CardLabel = new List<int>();
}