using UnityEditor;
using UnityEngine;

namespace MenuItem
{
    public class Terrain : MonoBehaviour
    {
        [UnityEditor.MenuItem("Terrain/Generate from Heightmap")]
        private static void GenerateTerrainHeightmap()
        {
            // get the selected texture
            var texture = Selection.activeObject as Texture2D;
            if (texture == null)
            {
                Debug.LogError("No texture selected");
                return;
            }

            // create a new heightmap
            var width = texture.width;
            var height = texture.height;
            var heightmap = new float[width, height];
            Debug.Log($"Width: {width}px\tHeight: {height}px");
            
            // fill the heightmap with the texture's grayscale values
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var color = texture.GetPixel(y, x);
                    heightmap[x, y] = color.grayscale;
                }
            }
            
            var terrainData = new TerrainData {
                heightmapResolution = width,
                size = new Vector3(width, 1, height)
            };
            terrainData.SetHeights(0, 0, heightmap);

            // save the terrain data asset
            var path = AssetDatabase.GetAssetPath(texture).Replace(".png", ".asset");
            AssetDatabase.CreateAsset(terrainData, path);
        }
    }
}