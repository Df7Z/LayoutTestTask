using System;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    public static SaveLoadManager Instance;
    public static Action<GameConfig> OnGameLoaded;
    
    private void Awake () 
    {
        if (Instance == null) 
        { 
            transform.parent = null;
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Invoke("LoadGame", Time.deltaTime);
        }
        else if (Instance == this) 
        { 
            Destroy(gameObject); 
        }
    }
    
    private void LoadGame()
    {
        OnGameLoaded?.Invoke(_gameConfig);
    }
    
    public void SaveTeamName(string value)
    {
        _gameConfig.SetTeamName(value);
    }
    
    public void SaveGame()
    {
        _gameConfig.SetGameStage(GameCore.Instance.GameState);
    }

    public void SaveGame(int indexStage, int valueColorIndex, int id) 
    {
     
        if (id == 0) //form
        {
            _gameConfig.SetFormIndex(indexStage, valueColorIndex);
        }
        else if (id == 1) //logo
        {
            _gameConfig.SetLogoIndex(indexStage, valueColorIndex);
        }
    }
    
    public void SaveGame(int[] valueColorIndexes, int id) 
    {
        if (id == 0) //form
        {
            _gameConfig.SetFormIndex(valueColorIndexes);
        }
        else if (id == 1) //logo
        {
            _gameConfig.SetLogoIndex(valueColorIndexes);
        }
    }
    
    public void ResetGameData()
    {
        _gameConfig.ResetData();
        LoadGame();
    }
    
    public int[] GetInfoByStage(int stage)
    {
        if (stage == 0)
            return _gameConfig.SelectedFormIndexes;
        if (stage == 1)
            return _gameConfig.SelectedLogoIndexes;
        return null;
    }
}
