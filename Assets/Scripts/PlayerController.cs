using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;

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

    [SerializeField]
    private GroundChecker _groundChecker;

    [SerializeField]
    private Animator _animator;

    private float _moveInput;

    private bool _isDead = false;

    public AudioSource deathAudio;
    public AudioSource jumpAudio;
    public AudioSource collectShardAudio;
    public AudioSource collectMirrorAudio;
    public AudioSource completeLevelAudio;
    public AudioSource useMirrorAudio;

    void Update()
    {
        if (_isDead) return;

        if (_tf.position.y < -10)
        {
            StartCoroutine(Death());
        }

        _moveInput = Input.GetAxisRaw("Horizontal");
        if (_moveInput > 0)
        {
            _tf.localScale = new Vector3(1f, 1f, 1f);
        } else if (_moveInput < 0)
        {
            _tf.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _groundChecker.isGrounded)
        {
            jumpAudio.Play();
            _rb.AddForceY(_jumpSpeed * 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            deathAudio.Play();
            StartCoroutine(Death());
        }

        Vector3 playerWorldPos = transform.position;
        Vector3Int cellPos = _tilemap.WorldToCell(playerWorldPos);

        TileBase tile = _tilemap.GetTile(cellPos);

        if (tile is CustomPropertyTile customPropertyTile)
        {
            if (customPropertyTile.type == TileType.GOAL)
            {
                _isDead = true;
                completeLevelAudio.Play();
                StartCoroutine(NextLevel());
            }
            else if (customPropertyTile.type == TileType.SCOREITEM)
            {
                collectShardAudio.Play();
                _levelController.CollectWisp(cellPos);
            }
            else if (customPropertyTile.type == TileType.ADDITIONALMIRROR)
            {
                collectMirrorAudio.Play();
                _levelController.CollectMirror(cellPos);
            }
        }

        _animator.SetFloat("VelocityX", Mathf.Abs(_rb.linearVelocityX));
        _animator.SetFloat("VelocityY", _rb.linearVelocityY);
    }

    private void FixedUpdate()
    {
        if (_isDead)
        {
            _rb.linearVelocityX = 0f;
            _rb.linearVelocityY = 0f;
        }
        _rb.AddForce(new Vector2(_moveInput * _moveSpeed, 0f));
    }

    public void TriggerDeath()
    {
        deathAudio.Play();
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        _animator.SetTrigger("Die");
        _isDead = true;
        yield return new WaitForSeconds(1f);
        _levelController.ReloadLevel();
    }

    private IEnumerator NextLevel()
    {
        _animator.SetTrigger("Die");
        _isDead = true;
        yield return new WaitForSeconds(1f);
        _levelController.GoToNextLevel();
    }
}
