using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using XeokitMetadata;

namespace test {
  public class MetaModelTest {

    private string ifcPath;
    private string mock;

    [SetUp]
    public void setup() {
      ifcPath = @"resources/unitTestCase.ifc";
     
      using (var r = new StreamReader(@"resources/metaModelMock.json")) {
        mock = r.ReadToEnd();
      }
    }

    [Test]
    public void metaModelTest() {
      try {
        var metaModel = MetaModel.fromIfc(ifcPath);
        var json = metaModel.serialize();
        Debug.Assert(json.Equals(mock));
      }
      catch (Exception e) {
        Console.WriteLine(e);
      }
    }
  }
}