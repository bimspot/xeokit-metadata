using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xbim.Ifc;
using Xbim.Ifc2x3.Interfaces;

namespace Hierarchy {
  static class Program {
    private static void Main(string[] args) {
      
      if (args.Length < 2) {
        System
          .Console
          .WriteLine("Please specify the path to the IFC and the output json.");
        Environment.Exit(1);
      }
      
      var ifcPath = args[0];
      var jsonPath = args[1];
      
      using (var model = IfcStore.Open(ifcPath)) {
        var project = model.Instances.FirstOrDefault<IIfcProject>();
        
        var metaModel = new MetaModel();
        metaModel.Id = project.Name;
        metaModel.ProjectId = project.GlobalId;
          
        var metaObjects = CalculateHierarchy(project);
        metaModel.MetaObjects = metaObjects;

        WriteToJson(metaModel, jsonPath);
      }
    }

    static List<MetaObject> CalculateHierarchy(IIfcObjectDefinition 
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
        var children = CalculateHierarchy(item);
        metaObjects.AddRange(children);
      }

      return metaObjects;
    }

    static async void WriteToJson(MetaModel metaModel, string jsonPath) {
      var contractResolver = new DefaultContractResolver {
        NamingStrategy = new CamelCaseNamingStrategy()
      };
      var settings = new JsonSerializerSettings {
        ContractResolver = contractResolver,
        Formatting = Formatting.Indented
      };
      
      using (StreamWriter outputFile = new StreamWriter(jsonPath)) {
        var output = JsonConvert.SerializeObject(metaModel, settings);
        await outputFile.WriteAsync(output);
      }
    }
  }
}