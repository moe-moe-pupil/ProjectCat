// --------------------------------------------------------------------------------------------------------------
// <copyright file="Projectile.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------

using Godot;

public class Projectile
{
    public Rid ShapeId { get; set; }

    public Vector2 VectorMove { get; set; }

    public Vector2 CurrentPos { get; set; }

    public double Lifetime { get; set; } = 0;

    public double AnimationLifetime { get; set; }

    public int ImageOffset { get; set; } = 0;

    public string Layer { get; set; } = "front";

    public double Speed { get; set; }
}
