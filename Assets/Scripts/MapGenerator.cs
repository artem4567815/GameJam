using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap passableTilemap; // для проходимых объектов
    public Tilemap impassableTilemap; // для непроходимых объектов
    public TileBase[] passableTiles;
    public TileBase[] impassableTiles;
    public int passableCount = 50;
    public int impassableCount = 50;

    public TileBase[] ImpassabletilesZone1;
    public TileBase[] ImpassabletilesZone2;
    public TileBase[] ImpassabletilesZone3;
    public TileBase[] ImpassabletilesZone4;

    public TileBase[] PassabletilesZone1;
    public TileBase[] PassabletilesZone2;
    public TileBase[] PassabletilesZone3;
    public TileBase[] PassabletilesZone4;

    public int tilesPerZone = 100;

    private Vector2[] zone1 = new Vector2[] { new Vector2(-250, 250), Vector2.zero, new Vector2(250, 250) };
    private Vector2[] zone2 = new Vector2[] { new Vector2(-250, 250), Vector2.zero, new Vector2(-250, -250) };
    private Vector2[] zone3 = new Vector2[] { new Vector2(-250, -250), Vector2.zero, new Vector2(250, -250) };
    private Vector2[] zone4 = new Vector2[] { new Vector2(250, -250), Vector2.zero, new Vector2(250, 250) };

    private HashSet<Vector3Int> occupiedTiles = new HashSet<Vector3Int>();

    void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += HandlePlayerDeath;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= HandlePlayerDeath;
    }

    void HandlePlayerDeath()
    {
        passableTilemap.ClearAllTiles();
        impassableTilemap.ClearAllTiles();
        occupiedTiles.Clear();
        Start();
    }


    void Start()
    {
        PlaceTilesInZone(zone1, PassabletilesZone1, passableTilemap);
        PlaceTilesInZone(zone2, PassabletilesZone2, passableTilemap);
        PlaceTilesInZone(zone3, PassabletilesZone3, passableTilemap);
        PlaceTilesInZone(zone4, PassabletilesZone4, passableTilemap);
        PlaceTilesInZone(zone1, ImpassabletilesZone1, impassableTilemap);
        PlaceTilesInZone(zone2, ImpassabletilesZone2, impassableTilemap);
        PlaceTilesInZone(zone3, ImpassabletilesZone3, impassableTilemap);
        PlaceTilesInZone(zone4, ImpassabletilesZone4, impassableTilemap);

        PlaceRandomObjects(passableTiles, passableCount, passableTilemap);
        PlaceRandomObjects(impassableTiles, impassableCount, impassableTilemap);

    }

    void PlaceTilesInZone(Vector2[] triangle, TileBase[] tileSet, Tilemap zoneTilemap)
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

            // Не размещать тайлы в квадрате x: -20..20, y: -20..20
            if (Mathf.Abs(tilePos.x) > 20 || Mathf.Abs(tilePos.y) > 20)
            {
                if (!occupiedTiles.Contains(tilePos))
                {
                    TileBase tileToPlace = tileSet[Random.Range(0, tileSet.Length)];
                    zoneTilemap.SetTile(tilePos, tileToPlace);
                    occupiedTiles.Add(tilePos);
                    placedCount++;
                }
            }
            attempts++;
        }
    }

    void PlaceRandomObjects(TileBase[] prefabs, int count, Tilemap targetTilemap)
    {
        int placed = 0;
        int maxAttempts = count * 10;
        int attempts = 0;
        while (placed < count && attempts < maxAttempts)
        {
            float x = Random.Range(-250f, 250f);
            float y = Random.Range(-250f, 250f);
            Vector3Int tilePos = new Vector3Int(Mathf.RoundToInt(x), Mathf.RoundToInt(y), 0);

            bool allowPlace = true;

            if (count == 2000)
            {
                if (Mathf.Abs(tilePos.x) <= 5 && Mathf.Abs(tilePos.y) <= 5)
                    allowPlace = false;
            }

            if (allowPlace && !occupiedTiles.Contains(tilePos))
            {
                TileBase tileToPlace = null;
                if (prefabs.Length > 0)
                {
                    tileToPlace = prefabs[Random.Range(0, prefabs.Length)];
                }
                if (tileToPlace != null)
                {
                    targetTilemap.SetTile(tilePos, tileToPlace);
                    occupiedTiles.Add(tilePos);
                    placed++;
                }
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
