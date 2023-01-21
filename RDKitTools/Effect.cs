﻿// --------------------------------------------------------------------------------------------------------------
// <copyright file="Calculator.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------
using RDKitTools.Utils;

namespace Skill
{
    /// <summary lang='Zh-CN'>
    /// 基础Effect类，技能效果的最小单元.
    /// </summary>
    public class Effect : SmartEnum<Effect>
    {
        /// <summary lang='Zh-CN>
        /// 伤害效果.
        /// </summary>
        public static readonly Effect Damage = new (1, nameof(Damage));

        /// <summary lang='Zh-CN>
        /// 治疗效果.
        /// </summary>
        public static readonly Effect Heal = new (2, nameof(Heal));

        private Effect(int value, string name)
            : base(value, name)
        {
            // todo
        }
    }
}
