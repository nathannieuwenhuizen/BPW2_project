using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Toggle stopwacth;

    [SerializeField]
    private Text level1Highscore;
    [SerializeField]
    private Text level2Highscore;

    public void Start()
    {
        Setting.showStopwatch = false;
        float highscore1 = PlayerPrefs.GetFloat("level1");
        Debug.Log(highscore1);
        if (highscore1 != 0)
        {
            level1Highscore.text = "highscore: " + highscore1.ToString().Split(',')[0] + '.' + highscore1.ToString().Split(',')[1].Substring(0, 3);
        }

        float highscore2 = PlayerPrefs.GetFloat("level2");
        if (highscore2 != 0)
        {
            level2Highscore.text = "highscore: " + highscore2.ToString().Split(',')[0] + '.' + highscore2.ToString().Split(',')[1].Substring(0, 3);
        }

    }
    public void GoToLVL(int lvlNumber)
    {
        SceneManager.LoadScene(lvlNumber);
    }
    public void UpdateStopwatchToggle()
    {
        Setting.showStopwatch = stopwacth.isOn;
    }
}
