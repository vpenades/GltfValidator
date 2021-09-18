using System.Linq;

using NUnit.Framework;

namespace GltfValidator
{
    public class Tests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void Test1()
        {
            var path = NUnit.Framework.TestContext.CurrentContext.TestDirectory;

            path = System.IO.Path.Combine(path, "Resources\\Avocado\\Avocado.gltf");

            var report = ValidationReport.Validate(path);

            TestContext.WriteLine(report.ToString());

            _Check(report.Issues);

            Assert.AreEqual(Severity.None, report.Severity);
        }

        [Test]
        public void Test2()
        {
            var path = NUnit.Framework.TestContext.CurrentContext.TestDirectory;

            path = System.IO.Path.Combine(path, "Resources\\invalid_uri_data.glb");

            var report = ValidationReport.Validate(path);           

            TestContext.WriteLine(report.ToString());

            _Check(report.Issues);

            Assert.AreEqual(Severity.Error, report.Severity);
            Assert.AreEqual(Severity.Error, report.Issues.Messages[0].Severity);
            Assert.AreEqual("INVALID_URI", report.Issues.Messages[0].Code);
            Assert.AreEqual(Severity.Information, report.Issues.Messages[1].Severity);
            Assert.AreEqual("UNUSED_OBJECT", report.Issues.Messages[1].Code);
        }


        private static void _Check(Issues issues)
        {
            Assert.AreEqual(issues.NumErrors, issues.Messages.Count(item => item.Severity == Severity.Error));
            Assert.AreEqual(issues.NumWarnings, issues.Messages.Count(item => item.Severity == Severity.Warning));
            Assert.AreEqual(issues.NumInfos, issues.Messages.Count(item => item.Severity == Severity.Information));
            Assert.AreEqual(issues.NumHints, issues.Messages.Count(item => item.Severity == Severity.Hint));
        }
    }
}