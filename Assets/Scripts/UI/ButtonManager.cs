using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayersData : MonoBehaviour {
    public static int maxPlayers = 8;
    public static int readyCount = 0;
    public static List<int> playerList = new List<int>();

    private static int currentPlayer = 0;

    // TRUCHERIAAAAA!! BUSCAR UNA MEJOR SOLUCION
    //public static bool isGameActive = false;

    public static bool[] playersSet = new bool[maxPlayers];

    public static int getPlayer() {
        int selected = -1;
        if (playerList.Count > 0) {
            if(currentPlayer < playerList.Count) {
                selected = playerList[currentPlayer];
                currentPlayer++;
            }
            else {
                Debug.Log("no player slots left");
            }
        }
        else {
            Debug.Log("no players in list");
        }
        return selected;
    }
}

public class ButtonManager : MonoBehaviour {

    private bool isReady = false;

    public Button p1Button;
    public Button p2Button;

    public Sprite X;
    public Sprite O;

    private void Start() {
        for(int a = 0; a < PlayersData.maxPlayers; a++) {
            PlayersData.playersSet[a] = false;
        }
        
    }

    private void Update() {

        for (int a = 1; a <= PlayersData.maxPlayers; a++) {
            if (Input.GetButtonDown("P" + a + "_Jump")) {
                PlayersData.playersSet[a - 1] = PlayersData.playersSet[a - 1] ? false : true;
                if (PlayersData.playersSet[a - 1]) {
                    PlayersData.playerList.Add(a);
                    PlayersData.readyCount++;
                    if (a == 1) { p1Button.image.sprite = O; }
                    if (a == 2) { p2Button.image.sprite = O; }
                }
                else {
                    PlayersData.playerList.Remove(a);
                    PlayersData.readyCount--;
                    if (a == 1) { p1Button.image.sprite = X; }
                    if (a == 2) { p2Button.image.sprite = X; }
                }
            }
        }

        if (PlayersData.readyCount > 0) {
            if (Input.GetButtonDown("P" + PlayersData.playerList[0] + "_Start")) {
                //PlayersData.isGameActive = true;
                SceneManager.LoadScene("MainGame");
            }
            if (Input.GetButtonDown("P" + PlayersData.playerList[0] + "_Select")) {
                Application.Quit();
            }
        }
    }
}

