using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PathFinder))]
public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridSize = new Vector2(50, 50);
    public float nodeRadius = 0.5f;
    public Transform actor;
    public bool snapPosition = true;
    public bool drawGrid = true;

    Node[,] grid;
    float nodeDi;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDi = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDi);
        gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDi);

        if (actor == null) actor = transform.parent;

        CreateGrid();
    }

    private void Update()
    {
        if (snapPosition) 
            transform.position = new Vector2(Mathf.FloorToInt(actor.position.x / (float)(nodeRadius * 2)) * (nodeRadius * 2), Mathf.FloorToInt(actor.position.y / (float)(nodeRadius * 2)) * (nodeRadius * 2));
        else
            transform.localPosition = Vector2.zero;
        UpdateGrid();
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPosition(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x - transform.position.x + gridSize.x / 2) / gridSize.x;
        float percentY = (worldPosition.y - transform.position.y + gridSize.y / 2) / gridSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.up * gridSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDi + nodeRadius) + Vector3.up * (y * nodeDi + nodeRadius);
                grid[x, y] = new Node(false, worldPoint, x, y);
            }
        }
    }

    void UpdateGrid()
    {
        Vector3 bottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.up * gridSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDi + nodeRadius) + Vector3.up * (y * nodeDi + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y].walkable = walkable;
                grid[x, y].worldPosition = worldPoint;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, gridSize.y, 0.1f));

        if (drawGrid && grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                if (GetComponent<PathFinder>().path != null && GetComponent<PathFinder>().path.Contains(node))
                    Gizmos.color = Color.blue;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDi - 0.1f));
            }
        }
    }
}
