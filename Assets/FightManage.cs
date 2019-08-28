using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightManage : MonoBehaviour
{
    [Header("Scene Ultimate")]
    public GameObject SceneUltimate;

    [Header("Round Number Sprite")]
    public Sprite RoundNumber1;
    public Sprite RoundNumber2;
    public Sprite RoundNumber3;
    public Image RoundNumberImage;

    public static int roundNum = 1;

    [Header("Player 1")]
    public Image Player1_Point1;
    public Image Player1_Point2;
    public Image Player1_HealthBar;
    public Image Player1_Poster;
    public Text Player1_Name;

    [Header("Player 2")]
    public Image Player2_Point1;
    public Image Player2_Point2;
    public Image Player2_HealthBar;
    public Image Player2_Poster;
    public Text Player2_Name;

    [Header("Load Scene")]
    public string loadScene = "Fight";
    public float seconds = 5f;

    [Header("Fight Result")]
    public Image Result_Image;
    public Sprite KO;
    public Sprite P1_WIN;
    public Sprite P2_WIN;

    private int flag = 0;
    private bool isPause = false;

    public static int Player1_Point = 0;
    public static int Player2_Point = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(Player1_Point == 1)
        {
            Player1_Point1.color = Color.green;
        }else if(Player1_Point == 2)
        {
            Player1_Point1.color = Color.green;
            Player1_Point2.color = Color.green;
        }

        if (Player2_Point == 1)
        {
            Player2_Point1.color = Color.green;
        }else if(Player2_Point == 2)
        {
            Player2_Point1.color = Color.green;
            Player2_Point2.color = Color.green;
        }

        if (roundNum == 1)
        {
            RoundNumberImage.sprite = RoundNumber1;
        }else if(roundNum == 2)
        {
            RoundNumberImage.sprite = RoundNumber2;
        }else if(roundNum == 3)
        {
            RoundNumberImage.sprite = RoundNumber3;
        }

        if(CharacterSelect.Player1Selected != null)
        {
            Player1_Poster.sprite = CharacterSelect.Player1Selected.characterPoster;
            Player1_Name.text = CharacterSelect.Player1Selected.characterName;
        }
        if (CharacterSelect.Player2Selected != null)
        {
            Player2_Poster.sprite = CharacterSelect.Player2Selected.characterPoster;
            Player2_Name.text = CharacterSelect.Player2Selected.characterName;
        }
    }

    private void Update()
    {
        ProcessResult();
        PauseGame();
    }

    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0f;
            }
        }
    }

    private void ProcessResult()
    {
        if((Player1_HealthBar.fillAmount == 0 || Player2_HealthBar.fillAmount == 0) && flag == 0)
        {
            Controll.CanControll = false;
            ControllAI.CanControll = false;

            StartCoroutine(TimeControll(1f));

            if (Player2_HealthBar.fillAmount == 0 && Player1_HealthBar.fillAmount > 0)
            {
                Player1_Point++;
            }
            else if (Player2_HealthBar.fillAmount > 0 && Player1_HealthBar.fillAmount == 0)
            {
                Player2_Point++;
            }

            flag++;
            roundNum++;

            Debug.Log(roundNum);

            if (roundNum < 4)
            {
                if(Player1_Point == 2 || Player2_Point == 2)
                {
                    DisplayResult();

                    ReturnCharacterSelection();
                }
                else
                {
                    DisplayResult();

                    StartCoroutine(GameUtils.LoadScene(loadScene, seconds));
                }
            }
            else if(roundNum == 4)
            {
                DisplayResult();

                ReturnCharacterSelection();
            }
        }
    }

    private void DisplayResult()
    {
        if(Player1_Point == 2)
        {
            Result_Image.sprite = P1_WIN;
        }else if(Player2_Point == 2)
        {
            Result_Image.sprite = P2_WIN;
        }else if(Player1_Point == 1 || Player2_Point == 1)
        {
            Result_Image.sprite = KO;
        }

        var temp = Result_Image.color;
        temp.a = 1f;
        Result_Image.color = temp;

        Debug.Log("Alpha: " + Result_Image.color.a);
    }

    private void ReturnCharacterSelection()
    {
        roundNum = 1;

        Player2_Point = 0;
        Player1_Point = 0;

        StartCoroutine(GameUtils.LoadScene("CharacterSelection", seconds));
    }

    private IEnumerator TimeControll(float seconds)
    {
        Time.timeScale = 0.3f;
        yield return new WaitForSeconds(seconds);

        Time.timeScale = 1f;
    }
}
