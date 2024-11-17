using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelUI : MonoBehaviour {

    public Text AnnouncerTextLine1;
    public Text AnnouncerTextLine2;
    public Text LevelTimer;

    public Slider[] healthSliders;

    public int player1Score = 0;
    public int player2Score = 0;
    public Text player1Text;
    public Text player2Text;

    public GameObject[] winIndicatorGrids;
    public GameObject winIndicator;

    public static LevelUI instance;
    public static LevelUI GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
        SetScoreText();
    }

    public void AddWinIndicator(int player)
    {
        GameObject go = Instantiate(winIndicator, transform.position, Quaternion.identity) as GameObject;
        go.transform.SetParent(winIndicatorGrids[player].transform);
    }

    public void SetScoreText()
    {
        player1Text.text = player1Score.ToString();
        player2Text.text = player2Score.ToString();
    }
}
