using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private void Start()
        => SceneManager.LoadScene(LevelRepository.Instance.Deserialize()); 
}
