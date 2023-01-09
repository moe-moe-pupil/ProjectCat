// --------------------------------------------------------------------------------------------------------------
// <copyright file="ModifyTests.cs" company="ProjectCat Technologies and contributors.">
// ��Դ�����ʹ���� GNU AFFERO GENERAL PUBLIC LICENSE version 3 ���֤��Լ��, ���������������ҵ������֤.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------
namespace Tests
{
    using Modify;

    /// <summary lang='Zh-CN'>
    /// ������.
    /// </summary>
    [TestFixture]
    public class ModifyTests
    {
        /// <summary lang='Zh-CN'>
        /// ��ʼ��.
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }

        /// <summary lang='Zh-CN'>
        /// ����modify����.
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