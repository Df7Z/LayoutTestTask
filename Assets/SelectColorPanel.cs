using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SelectColorPanel : MonoBehaviour
{
    [SerializeField] private Button _nextStageButton;
    [SerializeField] private Button _previousStageButton;
    [SerializeField] private Button _randomColorButton;
    [SerializeField] private RawImage[] _formImages = new RawImage[MAX_STAGE];
    [SerializeField] private RawImage[] _logoImages = new RawImage[MAX_STAGE];
    [SerializeField] private RawImage[] _colorImages = new RawImage[MAX_STAGE];
    [SerializeField] private RawImage[] _colorImagesEnable = new RawImage[MAX_STAGE];
    [SerializeField] private SelectColorPanelSlot[] _selectColorPanelSlots;
    private int[] _currentStageColors = new int[MAX_STAGE];
    private int _currentStage;
    private SelectColorPanelSlot _currentSelectColorPanelSlot;
    
    private const int MAX_STAGE = 3;
    public static Action<Color> OnChangeColor;
    
    private void Awake()
    {
        foreach (var selectColorPanelSlot in _selectColorPanelSlots)
        {
            selectColorPanelSlot.OnColorSelect += OnSlotSelected; 
        }
        
        _randomColorButton.onClick.AddListener(RandomColor);
        _previousStageButton.onClick.AddListener(OnClickPreviousButton);
        _nextStageButton.onClick.AddListener(OnClickNextButton);
        GameCore.OnGameStageChange += RandomLogo;
        SaveLoadManager.OnGameLoaded += LoadData;

        foreach (var colorImageEnable in _colorImagesEnable)
        {
            colorImageEnable.enabled = false;
        }

        _colorImages[_currentStage].color = _selectColorPanelSlots[_currentStageColors[_currentStage]].ColorSlot;
        _colorImagesEnable[_currentStage].enabled = true; 
    }

    private void StartColor(RawImage[] images)
    {
        foreach (var selectPanelSlot in _selectColorPanelSlots)
        {
            selectPanelSlot.ResetData();
        }
        
        for (int i = 0; i < _currentStageColors.Length; i++)
        {
            _currentStageColors[i] = GetRandomColorIndex();
            _colorImages[i].color =  _selectColorPanelSlots[_currentStageColors[i]].ColorSlot;
            images[i].color =  _selectColorPanelSlots[_currentStageColors[i]].ColorSlot;
            
        }
        
        if (GameCore.Instance.GameState == 0)
           SaveData(_currentStageColors, 0);
        if (GameCore.Instance.GameState == 1)
           SaveData(_currentStageColors, 1);
          
         
        _currentSelectColorPanelSlot =  _selectColorPanelSlots[_currentStageColors[_currentStage]];
        _currentSelectColorPanelSlot.Select();
    }
    

    private void RandomLogo(int stage)
    {
        if (stage == 1)
            StartColor(_logoImages);
    }

    private void RandomColor()
    {
        OnSlotSelected(_selectColorPanelSlots[GetRandomColorIndex()]);
    }

    private int GetRandomColorIndex()
    {
        var rnd = 0;
        while (true)
        {
            rnd = Random.Range(0, _selectColorPanelSlots.Length);
            if (!CheckColor(rnd, _currentStageColors))
            {
                RandomColor();
                continue;
            }

            return rnd;
        }
    }
    
    private void OnSlotSelected(SelectColorPanelSlot _newSelectColorPanelSlot)
    {
        if (!CheckColor(Array.IndexOf(_selectColorPanelSlots, _newSelectColorPanelSlot),
                SaveLoadManager.Instance.GetInfoByStage(GameCore.Instance.GameState)))
        {
            _newSelectColorPanelSlot.ResetData();
            return;
        }
        if (_currentSelectColorPanelSlot != null) _currentSelectColorPanelSlot.ResetData();
        _currentSelectColorPanelSlot = _newSelectColorPanelSlot;
        _colorImages[_currentStage].color = _currentSelectColorPanelSlot.ColorSlot;
        OnChangeColor?.Invoke(_currentSelectColorPanelSlot.ColorSlot);
        
        if (GameCore.Instance.GameState == 0)
            UpdateInfo(_currentSelectColorPanelSlot.ColorSlot, _formImages, 0);
        if (GameCore.Instance.GameState == 1)
            UpdateInfo(_currentSelectColorPanelSlot.ColorSlot, _logoImages, 1);

        _currentStageColors[_currentStage] = Array.IndexOf(_selectColorPanelSlots, _currentSelectColorPanelSlot);
    }

    private bool CheckColor(int newIndex, int[] indexes)
    {
        foreach (var index in indexes)
        {
            if (newIndex == index) return false;
        }

        return true;
    }
    
    public Color GetColorByIndex(int index)
    {
        return _selectColorPanelSlots[index].ColorSlot;
    }
    
    private void UpdateInfo(Color color, RawImage[] images, int id)
    {
        images[_currentStage].color = color;
        SaveData(id);
    }
    
    private void OnClickPreviousButton()
    {
        _colorImagesEnable[_currentStage].enabled = false;
        if (_currentSelectColorPanelSlot != null) 
            _currentSelectColorPanelSlot.ResetData();
        
        _currentStage--;
        if (_currentStage < 0)
            _currentStage = _colorImages.Length - 1;
        
        _currentSelectColorPanelSlot = _selectColorPanelSlots[_currentStageColors[_currentStage]];
        _currentSelectColorPanelSlot.Select();
        OnChangeColor?.Invoke(_currentSelectColorPanelSlot.ColorSlot);
        _colorImagesEnable[_currentStage].enabled = true;
    }
    
    private void OnClickNextButton()
    {
        _colorImagesEnable[_currentStage].enabled = false;
        if (_currentSelectColorPanelSlot != null) 
            _currentSelectColorPanelSlot.ResetData();
        
        _currentStage++;
        if (_currentStage > _colorImages.Length - 1)
            _currentStage = 0;
        
        _currentSelectColorPanelSlot = _selectColorPanelSlots[_currentStageColors[_currentStage]];
        _currentSelectColorPanelSlot.Select();
        OnChangeColor?.Invoke(_currentSelectColorPanelSlot.ColorSlot);
        _colorImagesEnable[_currentStage].enabled = true;
    }
    
    private void LoadData(GameConfig gameConfig)
    {
        if (!LoadInfo(gameConfig.SelectedFormIndexes, _formImages))
            StartColor(_formImages);
        
        if (gameConfig.GameStage >= 1)
        {
            if (!LoadInfo(gameConfig.SelectedLogoIndexes, _logoImages))
                StartColor(_logoImages);       
        }
        
        _selectColorPanelSlots[_currentStageColors[_currentStage]].Select();
    }

    private bool CheckValidSave(int[] indexes)
    {
        int validNum = 0;
        foreach (var index in indexes)
        {
            if (index == 0)
                validNum++;

        }
        if (validNum > 1)
            return false;
        return true;
    }
    
    private bool LoadInfo(int[] indexes, RawImage[] images)
    {
        if (!CheckValidSave(indexes))
            return false;
        
        for (int i = 0; i < MAX_STAGE; i++)
        {
            images[i].color = _selectColorPanelSlots[indexes[i]].ColorSlot;
            _colorImages[i].color = _selectColorPanelSlots[indexes[i]].ColorSlot;
        }

        _currentStageColors = indexes;
        return true;
    }
    
    private void SaveData(int id)
    {
        SaveLoadManager.Instance.SaveGame(_currentStage, Array.IndexOf(_selectColorPanelSlots, _currentSelectColorPanelSlot), id);
    }
    
    private void SaveData(int[] info,int id)
    {
        SaveLoadManager.Instance.SaveGame(info, id);
    }
}
