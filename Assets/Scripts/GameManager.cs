using UnityEngine;
using TMPro;
using System.Collections;

    

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI startText;
    public TextMeshProUGUI loseText;
    public TextMeshProUGUI scoreText;

    public static int TorpedosDestroyed;

    public static Transform WorldTransform;
    public static GameManager gameManager;



    private void Start()
    {
        WorldTransform = transform;
        gameManager = this;

        StartCoroutine(GameStart());
    }



    private void FixedUpdate()
    {
        //print(Time.time);
    }



    public static IEnumerator GameStart()
    {
        gameManager.startText.text = "Protect the capital ship from incoming missiles!";

        yield return new WaitForSeconds(5f);

        gameManager.startText.text = "";
    }



    public static IEnumerator GameLost(string loseText = "")
    {
        gameManager.loseText.text = loseText == "" ? "You failed to protect the capital ship!" : loseText;
        gameManager.scoreText.text = "Score = " + TorpedosDestroyed;

        yield return new WaitForSeconds(10f);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Defense");
    }
}
