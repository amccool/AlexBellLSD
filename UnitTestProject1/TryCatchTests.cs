using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlexBellLSD;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace UnitTestProject1
{
    [TestClass]
    public class TryCatchTests : CodeFixVerifier
    {
        [TestMethod]
        public void TryCatchNoThrowShouldGetWarningTest()
        {
            var test = @"
using System;
using System.Collections;

namespace TestProgram
{
    public class Junky
    {
        public void SillyMethod()
        {
            try
            {
                
            }
            catch(Exception ex)
            {
            }
        }
    }
}
";
            var expected = new DiagnosticResult
            {
                Id = NoRethrowAnalyzer.DiagnosticId,
                Message = "Try catch block with no rethrow",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 15, 13)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void TryCatchWithThrowShouldNotGetWarningTest()
        {
            var test = @"
using System;
using System.Collections;

namespace TestProgram
{
    public class Junky
    {
        public void SillyMethod()
        {
            try
            {
                
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
";

            VerifyCSharpDiagnostic(test);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new NoRethrowAnalyzer();
        }
    }
}
