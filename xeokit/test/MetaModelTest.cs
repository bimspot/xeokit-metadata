using System;
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
        Assert.True(json.Equals(mock), "Data is not equal with required one.");
      }
      catch (Exception e) {
        Console.WriteLine(e);
        Assert.True(false,e.Message);
      }
    }
  }
}