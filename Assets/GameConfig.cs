using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game config")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private int _gameStage;
    [SerializeField] private int[] _selectedFormIndexes = new int[GameCore.MAX_GAME_STAGE];
    [SerializeField] private int[] _selectedLogoIndexes = new int[GameCore.MAX_GAME_STAGE];
    [SerializeField] private string _teamName;
    [SerializeField] private bool _clearOnStart;
    public int GameStage => _gameStage;
    public int[] SelectedFormIndexes => _selectedFormIndexes;
    public int[] SelectedLogoIndexes => _selectedLogoIndexes;
    public string TeamName => _teamName;

    private void Awake()
    {
        if (_clearOnStart)
            ResetData();
    }

    public void ResetData()
    {
        _gameStage = 0;
        _selectedFormIndexes = new int[GameCore.MAX_GAME_STAGE];
        _selectedLogoIndexes = new int[GameCore.MAX_GAME_STAGE];
        _teamName = "";
    }

    public void SetFormIndex(int indexStage, int valueColorIndex)
    {
        _selectedFormIndexes[indexStage] = valueColorIndex;
    }
    
    public void SetLogoIndex(int indexStage, int valueColorIndex)
    {
        _selectedLogoIndexes[indexStage] = valueColorIndex;
    }
  
    public void SetFormIndex(int[] valueColorIndex)
    {
        _selectedFormIndexes = new int[3] {valueColorIndex[0], valueColorIndex[1], valueColorIndex[2]};
    }
    
    public void SetLogoIndex(int[] valueColorIndex)
    {
        _selectedLogoIndexes = new int[3] {valueColorIndex[0], valueColorIndex[1], valueColorIndex[2]};
    }

    
    public void SetGameStage(int value)
    {
        _gameStage = value;
    }
    
    public void SetTeamName(string value)
    {
        _teamName = value;
    }
    
}
