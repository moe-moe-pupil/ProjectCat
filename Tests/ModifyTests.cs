// --------------------------------------------------------------------------------------------------------------
// <copyright file="ModifyTests.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------
namespace Tests
{
    using Modify;

    /// <summary lang='Zh-CN'>
    /// 测试类.
    /// </summary>
    [TestFixture]
    public class ModifyTests
    {
        /// <summary lang='Zh-CN'>
        /// 初始化.
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }

        /// <summary lang='Zh-CN'>
        /// 基础modify测试.
        /// </summary>
        [Test]
        public void BasicModifyCalc()
        {
            string[] rawString = { "2", "3", "=999", "1.1" };
            Assert.That(Calculator.Calc(rawString), Is.EqualTo(1000.1));
            string[] rawString2 = { "2", "3", "-3", "1" };
            Assert.That(Calculator.Calc(rawString2), Is.EqualTo(3.0));
            string[] rawString3 = { "2", "3", "100%", "1" };
            Assert.That(Calculator.Calc(rawString3), Is.EqualTo(12.0));
            string[] rawString4 = { "2", "-3", "100%", "=1" };
            Assert.That(Calculator.Calc(rawString4), Is.EqualTo(2.0));
        }
    }
}