using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LevelLoader : MonoBehaviour
{
    private readonly float _duration = 0.5f;

    private GameTracker _tracker;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _tracker = FindObjectOfType<GameTracker>();
        _tracker.GameEnding += LoadNextLevel;
    }

    private void LoadNextLevel() => StartCoroutine(LoadNextLevelRoutine());

    private IEnumerator LoadNextLevelRoutine()
    {
        var currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentBuildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
            yield break;

        _animator.SetTrigger("End");
        yield return new WaitForSeconds(_duration);

        LevelRepository.Instance.Serialize(currentBuildIndex + 1);
        SceneManager.LoadScene(currentBuildIndex + 1);
    } 
}
