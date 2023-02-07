using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Newtonsoft;
using GodotUtilities;

/// <summary lang='zh-CN'>
///     游戏中的主要单位，可以是角色、敌人，甚至是陷阱机关
/// </summary>
public partial class Unit : CharacterBody2D
{
    /// <summary lang='zh-CN'>
    ///     基础生命值
    /// </summary>
    public double RedHeart;

    /// <summary lang='zh-CN'>
    ///     额外生命值
    /// </summary>
    public double SoulHeart;

    /// <summary lang='zh-CN'>
    ///     物理抗性
    /// </summary>
    public int PhysicalDefense;

    /// <summary lang='zh-CN'>
    ///     魔法抗性
    /// </summary>
    public int MagicDefense;

    /// <summary lang='zh-CN'>
    ///     移动速度
    /// </summary>
    public int MoveSpeed = 5;

    /// <summary lang='zh-CN'>
    ///     眩晕抗性
    /// </summary>
    public int StunResistance;

    /// <summary lang='zh-CN'>
    ///     击退抗性
    /// </summary>
    public int BounceResistance;

    /// <summary lang='zh-CN'>
    ///     目前装备的卡牌编号
    /// </summary>
    public List<int> EquipmentBar = new List<int>();
    public float JumpVelocity = 5;
    public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    public GlobalScene Global;

    // Called when the node enters the scene tree for the first time.
    /// <inheritdoc/>
    public override void _Ready()
    {
        this.Global = this.GetNode<GlobalScene>("/root/GlobalScene");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    /// <inheritdoc/>
    public override void _PhysicsProcess(double delta)
    {
        if (this.Name == this.Global.Session.Username)
        {
            Vector2 velocity = this.Velocity;
            if (!this.IsOnFloor())
            {
                velocity.y -= this.Gravity * (float)delta;
            }

            if (Input.IsActionJustPressed("ui_accept") && this.IsOnFloor())
            {
                velocity.y = this.JumpVelocity;
            }

            this.Velocity = velocity;
            this.MoveAndSlide();
        }
    }
}