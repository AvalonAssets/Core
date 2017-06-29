using AvalonAssets.Core.Extension;
using NUnit.Framework;

namespace AvalonAssets.CoreTests.Extension
{
    [TestFixture]
    public class EnumExtensionTest
    {
        public void ValuesTest()
        {
            CollectionAssert.AreEqual(new[] {TestEnum.A, TestEnum.B, TestEnum.C},
                EnumExtension.Values<TestEnum>());
        }

        public void ShiftTest()
        {
            var enums = new[] {TestEnum.A, TestEnum.B, TestEnum.C};
            CollectionAssert.AreEqual(new[] {TestEnum.B, TestEnum.C, TestEnum.A},
                enums.Shift(1));
            CollectionAssert.AreEqual(new[] {TestEnum.C, TestEnum.A, TestEnum.B},
                enums.Shift(5));
        }

        private enum TestEnum
        {
            A,
            B,
            C
        }
    }
}