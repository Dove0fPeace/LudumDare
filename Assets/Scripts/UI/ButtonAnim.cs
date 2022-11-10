using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnim : MonoBehaviour
{
    [SerializeField] private Vector3 _scaleUIElement = new Vector3(0.3f,0.3f,0.3f);
    [SerializeField] private float _durationScaleUI = 0.2f;
    
    private RectTransform _transform;
    private Button _button;
    

    private void Start()
    {
        _transform = GetComponent<RectTransform>();
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
}
