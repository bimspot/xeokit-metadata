using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xbim.Common.Step21;
using Xbim.Ifc;
using Xbim.Ifc2x3.Interfaces;

namespace Hierarchy {
  /// <summary>
  ///   The MetaObject is used to serialise the building elements within the IFC
  ///   model. It is a representation of a single element (e.g. IfcProject,
  ///   IfcStorey, IfcWindow, etc.).
  /// </summary>
  public struct MetaObject {
    /// <summary>
    ///   The GlobalId of the building element
    /// </summary>
    public string Id;

    /// <summary>
    ///   The Name of the building element
    /// </summary>
    public string Name;

    /// <summary>
    ///   The IFC type of the building element, e.g. 'IfcStandardWallCase'
    /// </summary>
    public string Type;

    /// <summary>
    ///   The GlobalId of the parent element if any.
    /// </summary>
    public string Parent;
  }

  /// <summary>
  ///   The MetaModel is used to serialise the building elements within the IFC
  ///   model. It is the representation of the complete JSON version of the
  ///   structure.
  /// </summary>
  public struct MetaModel {
    /// <summary>
    ///   The Id field is populated with the name of the project.
    /// </summary>
    public string Id;

    /// <summary>
    ///   The GlobalId of the project.
    /// </summary>
    public string ProjectId;

    /// <summary>
    ///   A list of all building elements as MetaObjects within the project.
    /// </summary>
    public List<MetaObject> MetaObjects;

    /// <summary>
    ///   The convenience initialiser creates and returns an instance of the
    ///   MetaModel by parsing the IFC at the provided path.
    /// </summary>
    /// <param name="ifcPath">A string path of the IFC path.</param>
    /// <returns>Returns the complete MetaModel of the IFC.</returns>
    /// <exception cref="ArgumentException">
    ///   Throws an exception if the provided IFC file is not using the 2x3
    ///   schema.
    /// </exception>
    public static MetaModel FromIfc(string ifcPath) {
      using (var model = IfcStore.Open(ifcPath)) {
        if (model.SchemaVersion != XbimSchemaVersion.Ifc2X3)
          throw new ArgumentException(
            "Currently only IFC 2x3 is supported");

        var project = model.Instances.FirstOrDefault<IIfcProject>();

        var metaModel = new MetaModel();
        metaModel.Id = project.Name;
        metaModel.ProjectId = project.GlobalId;

        var metaObjects = ExtractHierarchy(project);
        metaModel.MetaObjects = metaObjects;
        return metaModel;
      }
    }

    /// <summary>
    ///   The method selects all IIfcSpatialStructureElement-s recursively and
    ///   creates MetaObject of them.
    /// </summary>
    /// <param name="objectDefinition">
    ///   Accepts an IIfcObjectDefinition parameter, which related elements are
    ///   then iterated in search for additional structure elements.
    /// </param>
    /// <returns>
    ///   Returns a flattened list of all MetaObject-s related to the provided
    ///   IIfcObjectDefinition.
    /// </returns>
    private static List<MetaObject> ExtractHierarchy(IIfcObjectDefinition
      objectDefinition) {
      var metaObjects = new List<MetaObject>();

      var parentObject = new MetaObject {
        Id = objectDefinition.GlobalId,
        Name = objectDefinition.Name,
        Type = objectDefinition.GetType().Name
      };

      metaObjects.Add(parentObject);

      var spatialElement = objectDefinition as IIfcSpatialStructureElement;

      if (spatialElement != null) {
        var containedElements = spatialElement
          .ContainsElements
          .SelectMany(rel => rel.RelatedElements);

        foreach (var element in containedElements) {
          var mo = new MetaObject {
            Id = element.GlobalId,
            Name = element.Name,
            Type = element.GetType().Name,
            Parent = spatialElement.GlobalId
          };
          metaObjects.Add(mo);
        }
      }

      var relatedObjects = objectDefinition
        .IsDecomposedBy
        .SelectMany(r => r.RelatedObjects);

      foreach (var item in relatedObjects) {
        var children = ExtractHierarchy(item);
        metaObjects.AddRange(children);
      }

      return metaObjects;
    }

    /// <summary>
    ///   The async method serialises the MetaModel object to a JSON string,
    ///   then writes it out to the provided path.
    ///
    ///   During the serialisation, all property names are turned into lower
    ///   camel case.
    /// </summary>
    /// <param name="jsonPath">
    ///   The path of the output JSON file.
    /// </param>
    public async void ToJson(string jsonPath) {
      var contractResolver = new DefaultContractResolver {
        NamingStrategy = new CamelCaseNamingStrategy()
      };
      var settings = new JsonSerializerSettings {
        ContractResolver = contractResolver,
        Formatting = Formatting.Indented
      };

      using (var outputFile = new StreamWriter(jsonPath)) {
        var output = JsonConvert.SerializeObject(this, settings);
        await outputFile.WriteAsync(output);
      }
    }
  }
}