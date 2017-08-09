using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonManager : MonoBehaviour {
    /* Unity editor. */
    public GameObject[] pages;
    /* Page order:
     * mainButtonsPage      = 0
     * newGamePage
     * loadGamePage
     * settingsPage
     * 
         */

    /************************************
     Menu Buttons Page
    ************************************/

    /* New Game button. */
    public void OpenPage(int page_id)
    {
        pages[0].SetActive(false);
        pages[page_id].SetActive(true);
    }

    /* Settings button. */


    /* Exit button. */
    public void ExitGame()
    {
        Application.Quit();
    }


    /************************************
     Save Game Page
    ************************************/

    public void StartNewGame(int slot_id)
    {
        // check if there is a file at slot_id
        // if so, prompt for override
        // if not, start the game:
        SceneManager.LoadSceneAsync("Desert Town");
    }

    /************************************
     Load Game Page
    ************************************/

    /* Load a specific save file given the corresponding slot id (0-3).*/
    public void LoadSaveFile(int slot_id)
    {
        
    }

    /* Prompt user to confirm deletion, then delete a specific save file given the corresponding slot id (0-3).*/
    public void DeleteSaveFile(int slot_id)
    {
        // confirm deletion
    }

    /************************************
     Shared
    ************************************/

    /* Set all other pages active and return to the main manu. */
    public void ReturnToMainMenuPage()
    {
        for (int i = 1; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }
        pages[0].SetActive(true);
    }

    /* Links to my GitHub account. */
    public void LinkToGithub()
    {
        Application.OpenURL("https://github.com/dvscheng");
    }
}
