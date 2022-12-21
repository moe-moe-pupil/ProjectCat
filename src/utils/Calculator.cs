﻿// --------------------------------------------------------------------------------------------------------------
// <copyright file="Calculator.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------

namespace Modify
{
    /// <summary lang='Zh-CN'>
    /// 基础数值计算器，用于处理所有数值计算.
    /// </summary>
    internal class Calculator
    {
        /// <summary lang='Zh-CN'>
        /// 数值计算
        /// 规则1：所有数值严格按照层级排序后再计算数值
        /// 规则1-1：顺序为，加法(加法叠加) -> 非全能乘法(加法叠加) -> 全能乘法(乘法叠加)
        /// 示例1-1:
        /// (((待计算值N + 加法参数a1 + a2) * (非全能乘法参数m1 + m2)) * 全能乘法参数gm1) * 全能乘法参数gm2 = 输出
        /// 规则1-2: 如果相同层级则先按照优先级计算顺序，若相同则按照时间印记计算顺序,先进先触发.
        /// </summary>
        /// <param name="rawParams">原始字符串参数 [- | + | = | ''][1-9].+[%] (e.g. 234 意味着 +234, =45% 意味着修改为 45%).</param>
        /// <returns>计算结果.</returns>
        public static double Calc(params string[] rawParams)
        {
            double result = 0;

            // todo
            return result;
        }
    }
}