// --------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectileSpawner.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotUtilities;

public partial class ProjectileSpawner : Node2D
{
    public static readonly float BoundarySize = 2000000000f;

    public List<Projectile> Projectiles { get; set; } = new();

    public Rect2 Boundary { get; set; } = new(new Vector2(-BoundarySize, -BoundarySize), 2 * BoundarySize, 2 * BoundarySize);

    [Export]
    private float imageChangeOffset = 0.2f;

    [Export]
    private Image[] _frames { get; set; }

    [Node]
    private Area2D _sharedArea;

    public override void _ExitTree()
    {
        base._ExitTree();
        Projectiles.ForEach(projectile =>
        {
            PhysicsServer2D.FreeRid(projectile.ShapeId);
        });
        Projectiles.Clear();
    }

    public override void _Ready()
    {
        this.WireNodes();
    }

    public void RemoveAllInvaild()
    {
        foreach (var projectile in Projectiles.FindAll(projectile => !Boundary.HasPoint(projectile.CurrentPos)
                || projectile.Lifetime <= 0))
        {
            PhysicsServer2D.FreeRid(projectile.ShapeId);
            Projectiles.Remove(projectile);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        GD.Print(Projectiles.Count);
        RemoveAllInvaild();
        foreach (var item in Projectiles.Select((projectile, index) => (projectile, index)))
        {
            Vector2 offset = item.projectile.VectorMove.Normalized() * (float)item.projectile.Speed * (float)delta;
            item.projectile.CurrentPos += offset;
            var usedTransform = new Transform2D(0, item.projectile.CurrentPos);
            PhysicsServer2D.AreaSetShapeTransform(_sharedArea.GetRid(), item.index, usedTransform);
            item.projectile.Lifetime -= delta;
            item.projectile.AnimationLifetime += delta;
        }

        QueueRedraw();
    }

    public override void _Draw()
    {
        base._Draw();
        var offset = _frames[0].GetSize() / 2;
        foreach (var item in Projectiles.Select((projectile, index) => (projectile, index)))
        {
            if (item.projectile.AnimationLifetime >= imageChangeOffset)
            {
                item.projectile.ImageOffset += 1;
                item.projectile.ImageOffset %= _frames.Length;
                item.projectile.AnimationLifetime = 0;
            }

            ImageTexture imageTexture = new();
            imageTexture.SetImage(_frames[item.projectile.ImageOffset]);
            DrawTexture(
                imageTexture,
                item.projectile.CurrentPos - offset);
        }
    }

    public void SpawnBullet(Vector2 vectorMove, double lifeTime, Vector2 currentPos, double speed = 200)
    {
        Projectile projectile = new() { VectorMove = vectorMove, Speed = speed, CurrentPos = currentPos, Lifetime = lifeTime };
        ConfigProjCollision(projectile);
        Projectiles.Add(projectile);
    }

    public void ConfigProjCollision(Projectile proj)
    {
        var usedTransform = new Transform2D(0, proj.CurrentPos);
        var circleShape = PhysicsServer2D.CircleShapeCreate();
        PhysicsServer2D.ShapeSetData(circleShape, 8f);
        PhysicsServer2D.AreaSetMonitorable(circleShape, false);
        PhysicsServer2D.AreaSetCollisionLayer(circleShape, 0);
        PhysicsServer2D.AreaAddShape(_sharedArea.GetRid(), circleShape, usedTransform);
        proj.ShapeId = circleShape;
    }
}
