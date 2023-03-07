using UnityEngine;
using UnityEngine.UI;

public class TeamInputPanel : MonoBehaviour
{
    [SerializeField] private InputField _inputField;
    [SerializeField] private Text _fieldText;
    [SerializeField] private NextStageGame _nextStageGame;
    private bool _uncorrectInput;
    
    private void Awake()
    {
        _inputField.onValueChanged.AddListener(OnValueChanged);
        _inputField.onEndEdit.AddListener(SuccessInput);
        SaveLoadManager.OnGameLoaded += LoadData;
    }

    private void LoadData(GameConfig _gameConfig)
    {
        _inputField.text = _gameConfig.TeamName;
    }
    
    private void OnValueChanged(string value)
    {
        if (value.Length > 9)
        {
            _uncorrectInput = true;
            _nextStageGame.ChangeCanClick(false);
            _fieldText.color = GameCore.Instance.SelectColorPanel.GetColorByIndex(3);
            return;
        }

        if (_uncorrectInput)
        {
            _uncorrectInput = false;
            _fieldText.color = Color.white;
            _nextStageGame.ChangeCanClick(true);
            SuccessInput(value);
        }
    }

    private void SuccessInput(string value)
    {
        if (!_uncorrectInput)
            SaveLoadManager.Instance.SaveTeamName(value);
    }
}
