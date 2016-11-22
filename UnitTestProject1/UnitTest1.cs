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
                Message = String.Format("Type name '{0}' contains lowercase letters","TypeName"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 6, 13)
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

        public ArrayList Stilhated { get; set; }

        public ArrayListsBeHere()
        {
            ArrayList x = new ArrayList();
        }


        private void AnotherDisaster()
        {
            ArrayList thesesuck = new ArrayList();
        }
    }
}";

            var expected = new DiagnosticResult
            {
                Id = NonGenericCollectionsAnalyzer.DiagnosticId,
                Message = string.Format("Type name '{0}' contains lowercase letters", "TypeName"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 6, 13)
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
        public ArrayList _IhateTheseThings;

        public ArrayList Stilhated { get; set; }

        public ArrayListsBeHere()
        {
            ArrayList x = new ArrayList();
        }


        private void AnotherDisaster()
        {
            ArrayList thesesuck = new ArrayList();
        }
    }
}";

            var expected = new DiagnosticResult
            {
                Id = NonGenericCollectionsAnalyzer.DiagnosticId,
                Message = string.Format("Type name '{0}' contains lowercase letters", "TypeName"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 6, 13)
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
