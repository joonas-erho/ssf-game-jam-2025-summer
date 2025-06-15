using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private int _levelNumber;

    [SerializeField]
    private bool _isPlayedFromLevelSelect = false;

    [SerializeField]
    private string _tutorialTextContent;

    [SerializeField]
    private bool _hasMirroring;

    [SerializeField]
    private int _amountOfMirroringsRemaining;

    [SerializeField]
    private int _maxWisps;

    [SerializeField]
    private int _wispsCollected;

    [SerializeField]
    private TMP_Text _amountText;

    [SerializeField]
    private TMP_Text _levelText;

    [SerializeField]
    private TMP_Text _wispsText;

    [SerializeField]
    private GameObject _tutorialTextObject;

    [SerializeField]
    private TMP_Text _tutorialText;

    [SerializeField]
    private Tilemap _tilemap;

    [SerializeField]
    private GameObject _mirroringArea;

    [SerializeField]
    private GameManager _gm;

    private void Start()
    {
        _gm = FindAnyObjectByType<GameManager>();
        _isPlayedFromLevelSelect = _gm.isPlayedFromLevelSelect;
        _levelText.text = "Level " + _levelNumber;

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

        if (_maxWisps > 0)
        {
            _wispsText.text = "Shards: 0/" + _maxWisps;
        } else
        {
            _wispsText.text = "";
        }

        if (!_hasMirroring)
        {
            _amountText.text = "";
        } else
        {
            _mirroringArea.SetActive(true);
            _amountText.text = "Mirrors: " + _amountOfMirroringsRemaining.ToString();
        }

        if (_tutorialTextContent != "")
        {
            _tutorialText.text = _tutorialTextContent;
        } else
        {
            _tutorialTextObject.SetActive(false);
        }
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

        _wispsText.text = "Shards: " + _wispsCollected + "/" + _maxWisps;
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

    public void GoToNextLevel()
    {
        if (_levelNumber == 10)
        {
            _tutorialTextContent = "Thanks for playing! Full game coming to Steam in 2025.";
            _tutorialText.text = _tutorialTextContent;
            _tutorialTextObject.SetActive(true);
            return;
        }
        Debug.Log("Goal reached! Switching level.");
        _gm.ChangeScene(_levelNumber + 1, _isPlayedFromLevelSelect);
    }

    public void ReloadLevel()
    {
        Debug.Log("Reloading level due to death.");
        _gm.ChangeScene(_levelNumber, _isPlayedFromLevelSelect);
    }
}
