using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private int _amountOfMirroringsRemaining;

    [SerializeField]
    private int _maxWisps;

    [SerializeField]
    private int _wispsCollected;

    [SerializeField]
    private TMP_Text _amountText;

    [SerializeField]
    private TMP_Text _wispsText;

    [SerializeField]
    private Tilemap _tilemap;

    private void Start()
    {
        _maxWisps = 0;
        _wispsCollected = 0;
        BoundsInt bounds = _tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase tile = _tilemap.GetTile(pos);

                if (tile is CustomPropertyTile customPropertyTile)
                {
                    if (customPropertyTile.type == TileType.SCOREITEM)
                    {
                        _maxWisps++;
                    }
                }
            }
        }

        _wispsText.text = "Wisps: 0/" + _maxWisps;
    }

    public bool AttemptMirror()
    {
        if (_amountOfMirroringsRemaining == 0)
        {
            return false;
        }
        else
        {
            UpdateMirroringCount(-1);
            return true;
        }
    }

    public void CollectWisp(Vector3Int cellPos)
    {
        _tilemap.SetTile(cellPos, null);
        _wispsCollected++;

        _wispsText.text = "Wisps: " + _wispsCollected + "/" + _maxWisps;
    }

    public void CollectMirror(Vector3Int cellPos)
    {
        _tilemap.SetTile(cellPos, null);
        UpdateMirroringCount(1);
    }

    private void UpdateMirroringCount(int change)
    {
        _amountOfMirroringsRemaining += change;
        _amountText.text = "Mirrors: " + _amountOfMirroringsRemaining.ToString();
    }
}
