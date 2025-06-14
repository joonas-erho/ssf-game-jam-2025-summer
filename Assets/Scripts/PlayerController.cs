using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _verticalSpeed;

    [SerializeField]
    private float _jumpSpeed;

    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private Transform _tf;

    [SerializeField]
    private Tilemap _tilemap;

    [SerializeField]
    private LevelController _levelController;

    void Update()
    {
        // Move actual movement to FixedUpdate
        if (Input.GetKey(KeyCode.A))
        {
            _tf.position = new Vector2(_tf.position.x - _verticalSpeed, _tf.position.y);
        }
        if (Input.GetKey(KeyCode.D))
        {
            _tf.position = new Vector2(_tf.position.x + _verticalSpeed, _tf.position.y);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForceY(_jumpSpeed * 0.1f);
        }

        Vector3 playerWorldPos = transform.position;
        Vector3Int cellPos = _tilemap.WorldToCell(playerWorldPos);

        TileBase tile = _tilemap.GetTile(cellPos);

        if (tile is CustomPropertyTile customPropertyTile)
        {
            if (customPropertyTile.type == TileType.GOAL)
            {
                Debug.Log("Player stepped on the goal! Level complete!");
            }
            else if (customPropertyTile.type == TileType.SCOREITEM)
            {
                _levelController.CollectWisp(cellPos);
            }
            else if (customPropertyTile.type == TileType.ADDITIONALMIRROR)
            {
                _levelController.CollectMirror(cellPos);
            }
        }
    }
}
