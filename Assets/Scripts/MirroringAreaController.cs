using UnityEngine;
using UnityEngine.Tilemaps;

public class MirroringAreaController : MonoBehaviour
{
    private readonly Vector2Int[] _verticalMatrix = { 
        new(-1, -1), new(-1, 0), new(-1, 1),
        new( 1, -1), new( 1, 0), new( 1, 1)
    };

    private readonly Vector2Int[] _horizontalMatrix = {
        new(-1, -1), new(0, -1), new(1, -1),
        new(-1,  1), new(0,  1), new(1,  1)
    };

    [SerializeField]
    private Transform _tf;

    [SerializeField]
    private Tilemap _tilemap;

    [SerializeField]
    private LevelController _levelController;

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float snappedX = Mathf.Floor(mousePos.x) + 0.5f;
        float snappedY = Mathf.Floor(mousePos.y) + 0.5f;

        _tf.position = new Vector2(snappedX, snappedY);

        if (Input.GetMouseButtonDown(0) && _levelController.AttemptMirror())
        {
            MirrorHorizontally(mousePos);
        }

        if (Input.GetMouseButtonDown(1) && _levelController.AttemptMirror())
        {
            MirrorVertically(mousePos);
        }
    }

    private void MirrorHorizontally(Vector3 mousePos)
    {
        Vector3Int cellPos = _tilemap.WorldToCell(mousePos);
        
        for (int i = 0; i < 3; i++)
        {
            Vector2Int botPos = _horizontalMatrix[i];
            Vector2Int topPos = _horizontalMatrix[i+3];
            TileBase tileOnBottomRow = _tilemap.GetTile(new Vector3Int(cellPos.x + botPos.x, cellPos.y + botPos.y, 0));
            TileBase tileOnTopRow = _tilemap.GetTile(new Vector3Int(cellPos.x + topPos.x, cellPos.y + topPos.y, 0));
            _tilemap.SetTile(new Vector3Int(cellPos.x + botPos.x, cellPos.y + botPos.y, 0), tileOnTopRow);
            _tilemap.SetTile(new Vector3Int(cellPos.x + topPos.x, cellPos.y + topPos.y, 0), tileOnBottomRow);
        }
    }

    private void MirrorVertically(Vector3 mousePos)
    {
        Vector3Int cellPos = _tilemap.WorldToCell(mousePos);

        for (int i = 0; i < 3; i++)
        {
            Vector2Int leftPos = _verticalMatrix[i];
            Vector2Int rightPos = _verticalMatrix[i + 3];
            TileBase tileOnLeftColumn = _tilemap.GetTile(new Vector3Int(cellPos.x + leftPos.x, cellPos.y + leftPos.y, 0));
            TileBase tileOnRightColumn = _tilemap.GetTile(new Vector3Int(cellPos.x + rightPos.x, cellPos.y + rightPos.y, 0));
            _tilemap.SetTile(new Vector3Int(cellPos.x + leftPos.x, cellPos.y + leftPos.y, 0), tileOnRightColumn);
            _tilemap.SetTile(new Vector3Int(cellPos.x + rightPos.x, cellPos.y + rightPos.y, 0), tileOnLeftColumn);
        }
    }
}
