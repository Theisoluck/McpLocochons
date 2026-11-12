using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform player;

    public bool IsGameOver { get; private set; }
    private float bestDistance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (player != null && !IsGameOver)
        {
            bestDistance = Mathf.Max(bestDistance, Mathf.Abs(player.position.x));
        }

        var keyboard = Keyboard.current;
        if (IsGameOver && keyboard != null && keyboard.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void GameOver()
    {
        IsGameOver = true;
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 22;
        GUI.Label(new Rect(20, 20, 400, 40), "Distancia: " + Mathf.FloorToInt(bestDistance));
        if (IsGameOver)
        {
            GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 40, 400, 40), "Juego terminado");
            GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2, 400, 40), "Pulsa R para reiniciar");
        }
    }
}
