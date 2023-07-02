using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
 
[ScriptedImporter(1, "tsv")]
public class TSVImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        TextAsset textAsset = new TextAsset(File.ReadAllText(ctx.assetPath));
        ctx.AddObjectToAsset(Path.GetFileNameWithoutExtension(ctx.assetPath), textAsset);
        ctx.SetMainObject(textAsset);
        AssetDatabase.SaveAssets();
    }
}