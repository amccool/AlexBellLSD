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
    public class UnitTest : CodeFixVerifier
    {
        [TestMethod]
        public void TestMethod1()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ClassInheritance()
        {

            var test = @"
using System;
using System.Collections;

namespace TestProgram
{
    internal class Junky : ArrayList
    {
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

        [TestMethod]
        public void VariableDeclarationTest()
        {
            var test = @"
using System;
using System.Collections;

namespace TestProgram
{
    internal class BadVariableClass
    {
        public void TestMethod()
        {
            ArrayList badList = new ArrayList();
        }
    }
}
";
            var expected = new DiagnosticResult()
            {
                Id = NonGenericCollectionsAnalyzer.DiagnosticId,
                Message = "variable 'badList' inherits from a non-generic collection",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 11, 13)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }



        [TestMethod]
        public void FieldTest()
        {

            var test = @"
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace TestProgram
{
    class ArrayListsBeHere
    {
        public ArrayList _IhateTheseThings;

    }
}";

            var expected = new DiagnosticResult
            {
                Id = NonGenericCollectionsAnalyzer.DiagnosticId,
                Message = "field '_IhateTheseThings' is a non-generic collection",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 14, 9)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }



        [TestMethod]
        public void PropertiesTest()
        {

            var test = @"
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace TestProgram
{
    class ArrayListsBeHere
    {
        public ArrayList Stilhated { get; set; }
    }
}";

            var expected = new DiagnosticResult
            {
                Id = NonGenericCollectionsAnalyzer.DiagnosticId,
                Message = "property 'Stilhated' is a non-generic collection",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 14, 9)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }




        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new NonGenericCollectionsAnalyzer();
        }
    }
}
