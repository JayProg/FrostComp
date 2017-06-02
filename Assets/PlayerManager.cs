using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
    public static string PLAYER_KEY = "Player_";
    public static Player CurrentPlayer;

    void Awake()
    {
        LoadPlayer();
        if(CurrentPlayer == null)
        {
            CurrentPlayer = new Player();
        }
    }

    public static void LoadPlayer()
    {
        CurrentPlayer = JsonUtility.FromJson<Player>(PlayerPrefs.GetString(PLAYER_KEY));
    }

    public static void SavePlayer()
    {
        PlayerPrefs.SetString(PLAYER_KEY, JsonUtility.ToJson(CurrentPlayer));
    }
}
