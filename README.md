# xeokit-metadata

The `xeokit-metadata` is a multi-platform command line tool for extracting
the structural hierarchy of the building elements within an `IFC` into the
[metadata format of the `xeokit-sdk`][0]. Currently only IFC 2x3 is supported.

## Usage

```
$ wget --quiet https://github.com/bimspot/xeokit-metadata/releases/download/0.0.1/xeokit-metadata-linux-x64.tar.gz
$ tar -zxvf xeokit-metadata-linux-x64.tar.gz
$ chmod +x xeokit-metadata-linux-x64/xeokit-metadata
$ ln -s /absolute/path/to/xeokit-metadata-linux-x64/xeokit-metadata /usr/local/bin/xeokit-metadata
```

[See the usage with `xeokit-sdk` here][0].

## JSON

See the JSON schema in the repo: `MetaModel.schema.json`.

Example:

```json
{
  "id": "Geldropseweg 47",
  "projectId": "344O7vICcwH8qAEnwJDjSU",
  "metaObjects": [
    {
      "id": "344O7vICcwH8qAEnwJDjSU",
      "name": "Geldropseweg 47",
      "type": "IfcProject",
      "parent": null
    },
    {
      "id": "1GJdSmuaI6JvfGvF8t8fMD",
      "name": "-01. Fundering",
      "type": "IfcBuildingStorey",
      "parent": null
    },
    {
      "id": "1aR5aRgqnAJ9NEC7sfD6qG",
      "name": "21_1 WAND NC",
      "type": "IfcWall",
      "parent": "1GJdSmuaI6JvfGvF8t8fMD"
    }
  ]
}
```

## Credits

Created by [BIMspot][1] for the [`xeokit-sdk`][2] using the
[`XbimEssentials`][3] and the [NewtonSoft JSON][4] libraries.

[0]: https://github.com/xeokit/xeokit-sdk/wiki/Viewing-BIM-Models-Offline
[1]: https://bimspot.io
[2]: https://xeokit.io
[3]: https://github.com/xBimTeam/XbimEssentials
[4]: https://www.newtonsoft.com/json
