using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonAnim : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Color _defaultButtonColor;
    [SerializeField] private Color _buttonColorOnSelect;

    [SerializeField] private Vector3 _scaleUIElement;
    [SerializeField] private float _durationScaleUI;

    private TMP_Text _targetText;
    private RectTransform _transform;

    private void Start()
    {
        _transform = GetComponent<RectTransform>();
        _targetText = transform.GetComponentInChildren<TMP_Text>();
        _targetText.color = _defaultButtonColor;
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
