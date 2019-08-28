using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VersusManage : MonoBehaviour
{
    [Header("Load Scene")]
    public string loadScene = "Fight";
    public float timeToLoad = 5f;

    [Header("Player1")]
    public Image characterPoster1;
    public Text characterName1;

    [Header("Player1")]
    public Image characterPoster2;
    public Text characterName2;

    private Character characterSelected1;
    private Character characterSelected2;

    // Start is called before the first frame update
    void Start()
    {
        characterSelected1 = CharacterSelect.Player1Selected;
        characterSelected2 = CharacterSelect.Player2Selected;

        if(characterSelected1 != null)
        {
            characterPoster1.sprite = characterSelected1.characterPoster;
            characterName1.text = characterSelected1.name;
        }

        if (characterSelected2 != null)
        {
            characterPoster2.sprite = characterSelected2.characterPoster;
            characterName2.text = characterSelected2.name;
        }

        StartCoroutine(GameUtils.LoadScene(loadScene,timeToLoad));
    }
}
