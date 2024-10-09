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
            var path = TestContext.CurrentContext.TestDirectory;
            path = System.IO.Path.Combine(path, "Resources/Avocado/Avocado.gltf");

            var report = ValidationReport.Validate(path);

            TestContext.WriteLine(report.ToString());

            _Check(report.Issues);

            Assert.That(report.Severity, Is.EqualTo(Severity.Hint));
        }

        [Test]
        public async System.Threading.Tasks.Task Test1Async()
        {
            var path = TestContext.CurrentContext.TestDirectory;
            path = System.IO.Path.Combine(path, "Resources/Avocado/Avocado.gltf");

            var report = await ValidationReport.ValidateAsync(path, System.Threading.CancellationToken.None);

            TestContext.WriteLine(report.ToString());

            _Check(report.Issues);

            Assert.That(report.Severity, Is.EqualTo(Severity.Hint));
        }

        [Test]
        public void Test2()
        {
            var path = TestContext.CurrentContext.TestDirectory;
            path = System.IO.Path.Combine(path, "Resources/invalid_uri_data.glb");

            var report = ValidationReport.Validate(path);           

            TestContext.WriteLine(report.ToString());

            _Check(report.Issues);

            Assert.That(report.Severity, Is.EqualTo(Severity.Error));

            Assert.That(report.Issues.Messages[1].Severity, Is.EqualTo(Severity.Information));
            Assert.That(report.Issues.Messages[1].Code, Is.EqualTo("INVALID_URI"));

            Assert.That(report.Issues.Messages[2].Severity, Is.EqualTo(Severity.Information));
            Assert.That(report.Issues.Messages[2].Code, Is.EqualTo("UNUSED_OBJECT"));
        }

        [Test]
        public void Test3()
        {
            var path = TestContext.CurrentContext.TestDirectory;
            path = System.IO.Path.Combine(path, "Resources/empty_objects.gltf");

            var report = ValidationReport.Validate(path);

            TestContext.WriteLine(report.ToString());

            _Check(report.Issues);

            Assert.That(report.Severity, Is.EqualTo(Severity.Error));
            Assert.That(report.Issues.NumErrors, Is.EqualTo(8));
        }

        private static void _Check(Issues issues)
        {
            Assert.That(issues.Messages.Count(item => item.Severity == Severity.Error), Is.EqualTo(issues.NumErrors));
            Assert.That(issues.Messages.Count(item => item.Severity == Severity.Warning), Is.EqualTo(issues.NumWarnings));
            Assert.That(issues.Messages.Count(item => item.Severity == Severity.Information), Is.EqualTo(issues.NumInfos));
            Assert.That(issues.Messages.Count(item => item.Severity == Severity.Hint), Is.EqualTo(issues.NumHints));
        }
    }
}