using AlexBellLSD;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;

namespace UnitTestProject1
{
    [TestClass]
    public class TaskReturnedTest : CodeFixVerifier
    {
        [TestMethod]
        public void TestMethod1()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ReturnedTaskTest()
        {
            var test = @"
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestProgram
{
    internal class Haha
    {
        public async Task GarbageMan()
        {
            var url = ""http://forecast.weather.gov/MapClick.php?lat=42&lon=-75&FcstType=dwml"";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation(""User-Agent"", ""Stackoverflow/1.0"");

            //var xml = await client.GetStringAsync(url);
            var xml = client.GetStringAsync(url);

            if (xml != null)
            {
                //get bent
            }


        }
    }
}
";

            var expected = new DiagnosticResult
            {
                Id = NonGenericCollectionsAnalyzer.DiagnosticId,
                Message = "class 'Junky' inherits from a non-generic collection",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 7, 5)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }



        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new UnawaitedTaskAnalyzer();
        }
    }
}
