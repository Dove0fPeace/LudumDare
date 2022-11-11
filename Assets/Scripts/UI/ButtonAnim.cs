using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonAnim : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Color _defaultButtonColor;
    [SerializeField] private Color _buttonColorOnSelect;

    private TMP_Text _targetText;
    [SerializeField] private Vector3 _scaleUIElement = new Vector3(0.3f,0.3f,0.3f);
    [SerializeField] private float _durationScaleUI = 0.2f;
    
    private RectTransform _transform;
    private Button _button;
    

    private void Start()
    {
        _transform = GetComponent<RectTransform>();
        _targetText = transform.GetComponentInChildren<TMP_Text>();
        _targetText.color = _defaultButtonColor;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ButtonAnimation);
        
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(ButtonAnimation);
    }

    private void ButtonAnimation()
    {
        _transform.DOPunchScale(_scaleUIElement, _durationScaleUI);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _targetText.color = _buttonColorOnSelect;
        _transform.DOScale(_scaleUIElement, _durationScaleUI);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _targetText.color = _defaultButtonColor;
        _transform.DOScale(new Vector3(1,1,1), _durationScaleUI);
    }
}
