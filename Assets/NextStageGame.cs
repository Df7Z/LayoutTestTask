using UnityEngine;
using UnityEngine.UI;

public class NextStageGame : MonoBehaviour
{
   [SerializeField] private Button _button;
   [SerializeField] private Image[] _buttonImages;
   [SerializeField] private Color _unactiveColor;
   private bool _canClick;
   
   private void Awake()
   {
      _button.onClick.AddListener(OnClickButton);
      _canClick = true;
   }

   private void OnClickButton()
   {
      GameCore.Instance.NextStage();
   }

   public void ChangeCanClick(bool state)
   {
      _canClick = state;

      _button.interactable = _canClick;
      foreach (var image in _buttonImages)
      {
         if (!_canClick)
            image.color = _unactiveColor;
         else
            image.color = Color.white;
      }
   }
}
