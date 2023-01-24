// --------------------------------------------------------------------------------------------------------------
// <copyright file="Interpreter.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------

namespace RDKitTools.Skill
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary lang='Zh-CN'>
    /// 释义器，用于翻译原始字符串参数.
    /// </summary>
    public class Interpreter
    {
        /// <summary lang='Zh-CN'>
        /// 翻译结果符号位枚举.
        /// </summary>
        public enum InterpreterSign
        {
            /// <summary lang='Zh-CN'>
            /// 直接累加数值.
            /// </summary>
            Add = '+',

            /// <summary lang='Zh-CN'>
            /// 直接设置数值.
            /// </summary>
            Set = '=',
        }

        /// <summary lang='Zh-CN'>
        /// 获取数值.
        /// </summary>
        /// <param name="input">输入字符串.</param>
        /// <returns>数值.</returns>
        public static double GetNumbers(string input)
        {
            var numericChars = "0123456789,.".ToCharArray();
            return Convert.ToDouble(new string(input.Where(c => numericChars.Any(n => n == c)).ToArray()));
        }

        /// <summary lang='Zh-CN'>
        /// 原始字符串释义
        /// 规则1：输入字符串，输出 double.
        /// 规则1-1：如果检测到非法输入字符串，定位并报错.
        /// 示例1: 输入: "-3.4%", 输出: -0.034.
        /// </summary>
        /// <param name="rawParams">原始字符串参数 [- | + | = | ''][1-9].+[%] (e.g. 234 意味着 +234, =45% 意味着修改为 45%).</param>
        /// <returns>翻译结果.</returns>
        public static List<InterpreterResult> RawInterpreter(params string[] rawParams)
        {
            List<InterpreterResult> results = new List<InterpreterResult>();
            foreach (string str in rawParams)
            {
                InterpreterResult result = new () { Sign = InterpreterSign.Add, Mulitiply = false, Value = 1 };
                switch (str[0])
                {
                    case '+':
                        break;
                    case >= '0' and <= '9':
                        break;
                    case '-':
                        result.Value = -1;
                        break;
                    case '=':
                        result.Sign = InterpreterSign.Set;
                        break;
                    default:
                        break;
                }

                switch (str[^1])
                {
                    case '%':
                        result.Value *= 0.01 * GetNumbers(str);
                        result.Mulitiply = true;
                        break;
                    default:
                        result.Value *= GetNumbers(str);
                        break;
                }

                results.Add(result);
            }

            return results;
        }

        /// <summary lang='Zh-CN'>
        /// 翻译结果结构体.
        /// </summary>
        public struct InterpreterResult
        {
            /// <summary lang='Zh-CN'>
            /// 符号位.
            /// </summary>
            public InterpreterSign Sign;

            /// <summary lang='Zh-CN'>
            /// 数值.
            /// </summary>
            public double Value;

            /// <summary lang='Zh-CN'>
            /// 是否是乘数.
            /// </summary>
            public bool Mulitiply;

            /// <summary lang='Zh-CN'>
            /// 设置翻译结果结构体的值.
            /// </summary>
            /// <param name="sign">符号位.</param>
            /// <param name="value">数值.</param>
            public void SetValues(InterpreterSign sign, double value)
            {
                this.Sign = sign;
                this.Value = value;
            }
        }
    }
}
