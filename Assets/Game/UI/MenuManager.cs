using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI[] textItems = new TextMeshProUGUI[3];
    public Canvas canvas;



    private int currentSelectedItem;



    void Start()
    {
        textItems[0].color = Color.gray;
    }



    void Update()
    {
        int menuMove = Input.GetKeyDown(KeyCode.W) ? -1 : 0;
        menuMove = Input.GetKeyDown(KeyCode.S) ? 1 : menuMove;



        if (menuMove != 0)
        {
            textItems[currentSelectedItem].color = Color.white;

            currentSelectedItem = (currentSelectedItem + menuMove) % textItems.Length;
            if (currentSelectedItem < 0)
                currentSelectedItem = textItems.Length - 1;

            textItems[currentSelectedItem].color = Color.gray;
        }



        if (Input.GetAxis("Fire1") > 0 || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            MenuSelect();
    }



    public void MenuSelect()
    {
        switch (currentSelectedItem)
        {
            // Start new game
            case 0:
                SceneManager.LoadScene("Defense");
                break;

            // Options
            case 1:
                break;

            // Exit Game
            case 2:
                Application.Quit();
                break;
        }
    }
}
