using UnityEngine;
using UnityEditor;

public class TexturePostProcessor : AssetPostprocessor
{

    void OnPreprocessTexture()
    {

        if (assetPath.Contains("Modules/Icons/"))
        {
            TextureImporter importer = assetImporter as TextureImporter;
            importer.textureType = TextureImporterType.Sprite;            
            importer.isReadable = true;
            importer.filterMode = FilterMode.Point;
            importer.npotScale = TextureImporterNPOTScale.None;

            Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));
            if (asset)
            {
                EditorUtility.SetDirty(asset);
            }
            else
            {
                importer.textureType = TextureImporterType.Sprite;
            }
        }

    }
}