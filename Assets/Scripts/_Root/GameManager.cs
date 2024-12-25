using UnityEngine;

public enum GameState 
{
    Playing,
    GameOver
}

public class GameManager : PersistentSingleton<GameManager>
{
    public static GameState GameState 
    {
        get => Instance.gameState; 
        set => Instance.gameState = value;
    }
    public static System.Action OnGameOver;
    
    [SerializeField] GameState gameState = GameState.Playing;
}

