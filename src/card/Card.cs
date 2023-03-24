using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class Card
{
    /// <summary>
    ///   卡牌当前的物理攻击力.
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
    public List<String> CardLabel = new List<String>();
}