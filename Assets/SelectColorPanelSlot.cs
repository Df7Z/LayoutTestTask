using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectColorPanelSlot : MonoBehaviour
{
    [SerializeField] private Color _color = Color.black;
    [SerializeField] private Image _selectedRawImage;
    [SerializeField] private RawImage _colorRawImage;
    [SerializeField] private Button _button;
    private bool _selected;
    
    public Action<SelectColorPanelSlot> OnColorSelect;
    
    public Color ColorSlot => _color;
    
    private void Awake()
    {
        _button.onClick.AddListener(OnClickButton);
        _selectedRawImage.gameObject.SetActive(true);
        _selectedRawImage.enabled = false;
    }
    
    private void OnClickButton()
    {
        Select();
        OnColorSelect?.Invoke(this);
    }

    public void ResetData()
    {
        _selected = false;
        _selectedRawImage.enabled = false;
    }
    
    public void Select()
    {
        _selectedRawImage.enabled = true;
        _selected = true;
    }

    
    private void OnValidate()
    {
        _colorRawImage.color = _color;
    }
    
}
