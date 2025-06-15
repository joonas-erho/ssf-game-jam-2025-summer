using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private GameObject _transitionImage;

    [SerializeField]
    private GameObject _instantiatedTransition;

    public int currentLevel;

    public bool isPlayedFromLevelSelect = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // CreateTransitionImage();
    }

    public void ChangeScene(int sceneNumber, bool isPlayedFromLevelSelect)
    {
        StartCoroutine(ChangeSceneAsync(sceneNumber, isPlayedFromLevelSelect));
    }

    public IEnumerator ChangeSceneAsync(int sceneNumber, bool isPlayedFromLevelSelect)
    {
        Debug.Log("We are changing scene in enumerator");
        currentLevel = sceneNumber;
        CreateTransitionImage();
        yield return new WaitForSeconds(1f);
        StartCoroutine(DestroyTransitionImage());
        SceneManager.LoadScene(sceneNumber);
    }

    void CreateTransitionImage()
    {
        _instantiatedTransition = Instantiate(_transitionImage);
        _instantiatedTransition.transform.SetParent(_canvas.transform);
    }

    IEnumerator DestroyTransitionImage()
    {
        Debug.Log("que?");
        _instantiatedTransition.GetComponent<Animator>().SetTrigger("TransitionOut");
        yield return new WaitForSeconds(2f);
        Destroy(_instantiatedTransition);
    }
}
