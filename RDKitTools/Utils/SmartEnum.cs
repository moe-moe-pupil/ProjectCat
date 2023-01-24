// --------------------------------------------------------------------------------------------------------------
// <copyright file="SmartEnum.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------

namespace RDKitTools.Utils
{
    /// <summary lang="Zh-CN">
    /// 智能枚举类.
    /// </summary>
    /// <typeparam name="TEnum">枚举类型.</typeparam>
    public abstract class SmartEnum<TEnum> : IEquatable<SmartEnum<TEnum>>
    where TEnum : SmartEnum<TEnum>
    {
        /// <summary lang="Zh-CN">
        /// Initializes a new instance of the <see cref="SmartEnum{TEnum}"/> class.
        /// 初始化实例 <see cref="SmartEnum{TEnum}"/> .
        /// </summary>
        /// <param name="value">枚举值.</param>
        /// <param name="name">枚举名称.</param>
        protected SmartEnum(int value, string name)
        {
            Value = value;
            Name = name;
        }

        /// <summary>
        /// Gets enum's value.
        /// </summary>
        public int Value { get; protected init; }

        /// <summary>
        /// Gets enum's key.
        /// </summary>
        public string Name { get; protected init; } = string.Empty;

        /// <summary lang="Zh-CN">
        /// 用Value生成一个实例.
        /// </summary>
        /// <param name="value">enum's value.</param>
        /// <returns>智能枚举.</returns>
        public static TEnum? FromValue(int value)
        {
            return default;
        }

        /// <summary lang="Zh-CN">
        /// 用Name生成一个实例.
        /// </summary>
        /// <param name="name">enum's Name.</param>
        /// <returns>智能枚举.</returns>
        public static TEnum? FromName(string name)
        {
            return default;
        }

        /// <summary lang="Zh-CN">
        /// 比较它与其他smartEnum是否相同.
        /// </summary>
        /// <param name="other">其他smartEnum类对象.</param>
        /// <returns>是否相同.</returns>
        public bool Equals(SmartEnum<TEnum>? other)
        {
            if (other is null)
            {
                return false;
            }

            return GetType() == other.GetType() &&
                Value == other.Value;
        }

        /// <summary lang="Zh-CN">
        /// 比较它与其他object是否相同.
        /// </summary>
        /// <param name="obj">其他object.</param>
        /// <returns>是否相同.</returns>
        public override bool Equals(object? obj)
        {
            return obj is SmartEnum<TEnum> other &&
                Equals(other);
        }

        /// <summary>
        /// GetHashCode.
        /// </summary>
        /// <returns>value's hash code</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
