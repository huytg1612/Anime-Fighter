using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    [Header("Load Scene")]
    public string loadScene = "Versus";
    public float timeToLoad = 1f;

    private int playerTurn = 1;

    public static Character Player1Selected;
    public static Character Player2Selected;

    public List<Character> characterList = new List<Character>();
    public GameObject characterCellPrefabs;

    public Image selectPoint;

    [Header("Player 1")]
    public Image characterPoster1;
    public Text characterName1;
    public Image characterSprite1;

    [Header("Player 2")]
    public Image characterPoster2;
    public Text characterName2;
    public Image characterSprite2;

    private int selectedIndex = 0;
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] listAudio;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        foreach(Character character in characterList)
        {
            SpawnCharacterCell(character);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SelectCharacter();
    }

    private void SpawnCharacterCell(Character character)
    {
        GameObject charCell = Instantiate(characterCellPrefabs, transform);

        Image portrait = charCell.transform.Find("CharacterPortrait").GetComponent<Image>();
        Text name = charCell.transform.Find("CharacterName").GetComponent<Text>();

        portrait.sprite = character.characterPortrait;
        name.text = character.name;
    }

    private void SelectCharacter()
    {
        if(playerTurn <= 2)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                selectedIndex--;

                if (selectedIndex < 0)
                {
                    selectedIndex = characterList.Count - 1;
                }

                GameUtils.PlaySound(listAudio[0], audioSource);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                selectedIndex++;

                if (selectedIndex >= characterList.Count)
                {
                    selectedIndex = 0;
                }

                GameUtils.PlaySound(listAudio[0], audioSource);
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                AudioClip characterVoice = characterList[selectedIndex].characterVoice;

                if (playerTurn == 1)
                {
                    Player1Selected = characterList[selectedIndex];
                }
                else if (playerTurn == 2)
                {
                    Player2Selected = characterList[selectedIndex];
                }

                selectedIndex = 0;
                playerTurn++;

                GameUtils.PlaySound(listAudio[1], audioSource);

                if (characterVoice != null)
                {
                    StartCoroutine(PlayCharacterVoice(characterVoice, 0.5f));
                }

                if(playerTurn == 3)
                {
                    StartCoroutine(GameUtils.LoadScene(loadScene,timeToLoad));
                }
            }

            selectPoint.transform.position = transform.GetChild(selectedIndex).transform.position;

            setUI(selectedIndex);
        }
    }

    private void setUI(int selectedIndex)
    {
        Sprite characterPoster = characterList[selectedIndex].characterPoster;
        string name = characterList[selectedIndex].name;
        Sprite characterSprite = characterList[selectedIndex].characterSprite;

        if(playerTurn == 1)
        {
            characterName1.text = name;
            characterPoster1.sprite = characterPoster;
            characterSprite1.sprite = characterSprite;
        }
        else if (playerTurn == 2)
        {
            characterName2.text = name;
            characterPoster2.sprite = characterPoster;
            characterSprite2.sprite = characterSprite;
        }
    }

    private IEnumerator PlayCharacterVoice(AudioClip characterVoice,float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GameUtils.PlaySound(characterVoice, audioSource);
    }

    private IEnumerator LoadScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(loadScene);
    }
}
