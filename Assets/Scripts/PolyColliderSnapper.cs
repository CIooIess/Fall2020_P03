using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class PolyColliderSnapper : MonoBehaviour
{
    public PolygonCollider2D poly;
    public Grid grid;

    void Start()
    {
        if (poly == null)
            poly = GetComponent<PolygonCollider2D>();
    }

    public void Snap()
    {
        int counter = 0;
        Vector2[] snapPoints = poly.points;
        for (int i = 0; i < poly.points.Length; i++)
        {
            counter++;
            Vector2 point = snapPoints[i];
            snapPoints[i] = new Vector2(grid.GetCellCenterWorld(new Vector3Int(grid.WorldToCell(point).x, grid.WorldToCell(point).y, 0)).x, 
                                        grid.GetCellCenterWorld(new Vector3Int(grid.WorldToCell(point).x, grid.WorldToCell(point).y, 0)).y);
            poly.points = snapPoints;
        }
        print("Snapped " + counter + " points.");
    }
}