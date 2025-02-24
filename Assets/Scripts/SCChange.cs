using UnityEngine;

public class SCChange : MonoBehaviour
{
    public void ChangeScene(string scene) {
        SCManager.instance.LoadScene(scene);
    }

    public void ExitGame() {
        SCManager.instance.ExitGame();
    }
}
