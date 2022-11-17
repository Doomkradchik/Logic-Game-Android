using UnityEngine;

public class LevelRepository 
{
    public static LevelRepository Instance =
        new LevelRepository();

    private LevelRepository() { }

    private const string LEVEL_KEY = "level";
    private readonly int _defaultBuildIndex = 1;

    public int Deserialize()
    {
        return PlayerPrefs.GetInt(LEVEL_KEY, _defaultBuildIndex);
    }

    public void Serialize(int buildIndex)
    {
        PlayerPrefs.SetInt(LEVEL_KEY, buildIndex);
    }
}
