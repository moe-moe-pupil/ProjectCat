// --------------------------------------------------------------------------------------------------------------
// <copyright file="Unit.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------

using Godot;

/// <summary lang='zh-CN'>
///     游戏中的主要单位，可以是角色、敌人，甚至是陷阱机关.
/// </summary>
public partial class Unit : CharacterBody2D
{
    public RDKitTools.Unit.Unit Status { get; private set; }

    public float Gravity { get; private set; } = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
    }
}