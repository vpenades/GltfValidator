using NUnit.Framework;

namespace GltfValidator
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var path = NUnit.Framework.TestContext.CurrentContext.TestDirectory;

            path = System.IO.Path.Combine(path, "Resources\\Avocado\\Avocado.gltf");

            var report = ValidationReport.Validate(path);

            Assert.IsFalse(report.HasErrors);
        }
    }
}