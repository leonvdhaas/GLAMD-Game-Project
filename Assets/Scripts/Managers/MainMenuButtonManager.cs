using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonManager : MonoBehaviour {

    public void StartGamebtn ( string newgamelvl)
    {
        SceneManager.LoadScene(newgamelvl);
    }
	public void ExitGameBtn()
    {
        Application.Quit();
    }

}
