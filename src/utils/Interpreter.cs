// --------------------------------------------------------------------------------------------------------------
// <copyright file="Interpreter.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------

namespace Modify
{
    /// <summary lang='Zh-CN'>
    /// 释义器，用于翻译原始字符串参数.
    /// </summary>
    internal class Interpreter
    {
        /// <summary lang='Zh-CN'>
        /// 原始字符串释义
        /// 规则1：输入字符串，输出 double.
        /// 规则1-1：如果检测到非法输入字符串，定位并报错.
        /// 示例1: 输入: "-3.4%", 输出: -0.034.
        /// </summary>
        /// <param name="rawParams">原始字符串参数 [- | + | = | ''][1-9].+[%] (e.g. 234 意味着 +234, =45% 意味着修改为 45%).</param>
        /// <returns>翻译结果.</returns>
        public static double RawInterpreter(params string[] rawParams)
        {
            double result = 0;

            // todo
            return result;
        }
    }
}
