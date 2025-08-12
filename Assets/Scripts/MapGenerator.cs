using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap;

    // Массивы тайлов для каждой зоны (назначай их в инспекторе)
    public TileBase[] tilesZone1;
    public TileBase[] tilesZone2;
    public TileBase[] tilesZone3;
    public TileBase[] tilesZone4;

    public int tilesPerZone = 100;

    private Vector2[] zone1 = new Vector2[] { new Vector2(-250, 250), Vector2.zero, new Vector2(250, 250) };
    private Vector2[] zone2 = new Vector2[] { new Vector2(-250, 250), Vector2.zero, new Vector2(-250, -250) };
    private Vector2[] zone3 = new Vector2[] { new Vector2(-250, -250), Vector2.zero, new Vector2(250, -250) };
    private Vector2[] zone4 = new Vector2[] { new Vector2(250, -250), Vector2.zero, new Vector2(250, 250) };

    private HashSet<Vector3Int> occupiedTiles = new HashSet<Vector3Int>();

    void Start()
    {
        PlaceTilesInZone(zone1, tilesZone1);
        PlaceTilesInZone(zone2, tilesZone2);
        PlaceTilesInZone(zone3, tilesZone3);
        PlaceTilesInZone(zone4, tilesZone4);
    }

    void PlaceTilesInZone(Vector2[] triangle, TileBase[] tileSet)
    {
        if (tileSet == null || tileSet.Length == 0)
        {
            Debug.LogWarning("Tile set is empty for a zone");
            return;
        }

        int placedCount = 0;
        int attempts = 0;
        int maxAttempts = tilesPerZone * 10;

        while (placedCount < tilesPerZone && attempts < maxAttempts)
        {
            Vector2 point = RandomPointInTriangle(triangle[0], triangle[1], triangle[2]);
            Vector3Int tilePos = new Vector3Int(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y), 0);

            if (!occupiedTiles.Contains(tilePos))
            {
                // Выбираем случайный тайл из набора зоны
                TileBase tileToPlace = tileSet[Random.Range(0, tileSet.Length)];

                tilemap.SetTile(tilePos, tileToPlace);
                occupiedTiles.Add(tilePos);
                placedCount++;
            }

            attempts++;
        }
    }

    Vector2 RandomPointInTriangle(Vector2 A, Vector2 B, Vector2 C)
    {
        float u = Random.value;
        float v = Random.value;

        if (u + v > 1f)
        {
            u = 1f - u;
            v = 1f - v;
        }

        return A + u * (B - A) + v * (C - A);
    }



}
