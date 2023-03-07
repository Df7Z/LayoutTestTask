using System;
using UnityEngine;
using UnityEngine.UI;

public class GameCore : MonoBehaviour
{
    [SerializeField] private Button _restartGame;
    [SerializeField] private SelectColorPanel _selectColorPanel;
    [SerializeField] private Animator _gameAnimator;
    [SerializeField] private Transform _endGameTransform;
    private int _cancelFormHash = Animator.StringToHash("CancelSelectForm");
    private int _cancelLogoHash = Animator.StringToHash("CancelSelectLogo");
    private int _resetLogoHash = Animator.StringToHash("Reset");
    private int _gameState;

    public const int MAX_GAME_STAGE = 3;
    public static GameCore Instance;
    public static Action<int> OnGameStageChange;
    
    public int GameState => _gameState;
    public SelectColorPanel SelectColorPanel => _selectColorPanel;

    
    private void Awake () 
    {
        if (Instance == null) 
        { 
            transform.parent = null;
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else if (Instance == this) 
        { 
            Destroy(gameObject); 
        }
    }

    private void Init()
    {
        _restartGame.onClick.AddListener(() => { ResetGame(); });
        SaveLoadManager.OnGameLoaded += LoadData;
    }

    private void LoadData(GameConfig gameConfig)
    {
        _gameState = gameConfig.GameStage;
        if (_gameState >= 1) _gameAnimator.CrossFade(_cancelFormHash, 0f);
        if (_gameState == 2) _gameAnimator.CrossFade(_cancelLogoHash, 0f);
    }
    
    public void NextStage()
    {
        _gameState++;
        
        if (_gameState > MAX_GAME_STAGE - 1)
        {
            EndGame();
            return;
        }

        ChangeAnimation();
        OnGameStageChange?.Invoke(_gameState);
        SaveLoadManager.Instance.SaveGame();
    }

    private void ChangeAnimation()
    {
        if (_gameState == 1)
            _gameAnimator.CrossFade(_cancelFormHash, 0f);
        
        if (_gameState == 2)
            _gameAnimator.CrossFade(_cancelLogoHash, 0f);
    }
    
    private void EndGame()
    {
        _endGameTransform.gameObject.SetActive(true);
    }

    private void ResetGame()
    {
        SaveLoadManager.Instance.ResetGameData();
        _endGameTransform.gameObject.SetActive(false);
        _gameAnimator.CrossFade(_resetLogoHash, 0f);
    }
}
