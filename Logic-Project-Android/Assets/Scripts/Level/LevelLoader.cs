using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class LevelLoader : MonoBehaviour
{
    public event Action<int> SceneLoaded;
    private readonly float _duration = 0.5f;

    private GameTracker _tracker;
    private Animator _animator;

    private int BuildIndex =>
        SceneManager.GetActiveScene().buildIndex;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _tracker = FindObjectOfType<GameTracker>();
        _tracker.GameEnding += LoadNextLevel;

        SceneLoaded?.Invoke(BuildIndex);
    }

    public void LoadNextLevel() => StartCoroutine(LoadNextLevelRoutine());

    public void ReloadLevel()
        => SceneManager.LoadScene(BuildIndex);

    private IEnumerator LoadNextLevelRoutine()
    {
        if (BuildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
            yield break;

        _animator.SetTrigger("End");
        yield return new WaitForSeconds(_duration);

        LevelRepository.Instance.Serialize(BuildIndex + 1);
        SceneManager.LoadScene(BuildIndex + 1);
    } 
}
