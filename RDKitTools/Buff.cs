// --------------------------------------------------------------------------------------------------------------
// <copyright file="Calculator.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------

namespace Skill
{
    /// <summary lang='Zh-CN'>
    /// Buff类，Effect类的聚合，负责分类和触发Effect.
    /// 规则1：_active、_passive分别为技能释放，被动的List<Effect>数组.
    /// </summary>
    public class Buff
    {
        /// <summary lang='Zh-CN'>
        /// Buff
        /// 规则1：_active、_passive分别为技能释放，被动的List<Effect>数组.
        /// </summary>
        /// <param name="rawParams">原始字符串参数 [- | + | = | ''][1-9].+[%] (e.g. 234 意味着 +234, =45% 意味着修改为 45%).</param>
        /// <returns>计算结果.</returns>
    }
}
