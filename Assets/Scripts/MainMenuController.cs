using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameManager _gm;

    public void StartGame()
    {
        _gm.ChangeScene(1, true);
    }
}
