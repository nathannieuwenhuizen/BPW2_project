using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Character character;
    private CameraFade cameraFade;
    private CameraShake cameraShake;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject finishMenu;

    public static GameManager instance;

    private Checkpoint[] checkPoints;

    [SerializeField]
    private Stopwatch stopWatch;
    [SerializeField]
    private GameObject newHighscore;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        character = FindObjectOfType<Character>();
        cameraFade = FindObjectOfType<CameraFade>();
        cameraShake = cameraFade.GetComponent<CameraShake>();
        checkPoints = FindObjectsOfType<Checkpoint>();

        Pause(false);

        stopWatch.gameObject.SetActive(true);
    }

    public void RespawnPlayerAfterTime(float duration)
    {
        StartCoroutine(Respawning(duration));
    }
    public IEnumerator Respawning(float duration)
    {
        yield return new WaitForSeconds(duration / 2);
        cameraShake.Shake(0.1f);
        cameraFade.fadingOut = true;
        cameraFade.fadeSpeed = 0.3f;
        yield return new WaitForSeconds(duration/ 2);
        character.gameObject.SetActive(true);
        character.Respawn();
    }

    public void SetCheckPoint(Checkpoint checkpoint)
    {
        foreach(Checkpoint cp in checkPoints)
        {
            cp.Check = false;
        }
        checkpoint.Check = true;
    }
    public void Pause(bool pause)
    {
        if (finishMenu.activeSelf)
        {
            return;
        }

        Time.timeScale = pause ? 0f : 1f;
        pauseMenu.SetActive(pause);
    }

    public void GoToScene(int number)
    {
        Pause(false);
        SceneManager.LoadScene(number);
    }
    public void Finished()
    {
        finishMenu.SetActive(true);
        character.enabled = false;
        stopWatch.paused = true;
        stopWatch.ShowText();
        float levelNumber = SceneManager.GetActiveScene().buildIndex;

        if (PlayerPrefs.GetFloat("level" + levelNumber) == 0)
        {
            UpdateHighscore(levelNumber);
        }

        if (stopWatch.value < PlayerPrefs.GetFloat("level" + levelNumber))
        {
            UpdateHighscore(levelNumber);
        }
    }
    void UpdateHighscore(float levelNumber)
    {
        PlayerPrefs.SetFloat("level" + levelNumber, stopWatch.value);
        newHighscore.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
