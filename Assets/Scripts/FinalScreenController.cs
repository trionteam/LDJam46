using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScreenController : MonoBehaviour
{
	public void ExitToMenu()
	{
		SceneManager.LoadScene("Scenes/MainMenu");
	}
}
