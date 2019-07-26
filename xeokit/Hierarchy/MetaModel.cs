using System.Collections.Generic;

namespace Hierarchy {
  public struct MetaModel {
    public string Id;
    public string ProjectId;
    public List<MetaObject> MetaObjects;
  }

  public struct MetaObject {
    public string Id;
    public string Name;
    public string Type;
    public string Parent;
  }
}