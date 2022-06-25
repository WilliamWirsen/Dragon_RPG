using System.Collections;
using TMPro;
using UnityEngine;

public class ScreenPopupText : MonoBehaviour
{
    private TextMeshPro _textMesh;
    private float _startFontSize = 6;
    private float _maxFontSize = 12;
    
    private Color _startColor; 
    private Color _endColor;
    private float time = 0f;

    [SerializeField]
    private float duration = 5f;    

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshPro>();
        _textMesh.fontSize = _startFontSize;
        
    }
    public void Setup(int damageAmount)
    {
        _textMesh.text = damageAmount.ToString();
        if (damageAmount > 0)
        {
            _startColor = Color.red;
            _textMesh.color = _startColor;
            _endColor = new(1, 0.7f, 0.7f, 0);

        }
        else if (damageAmount == 0)
        {
            _startColor = Color.white;
            _textMesh.color = _startColor;
            _endColor = Color.yellow;
        }
        else if (damageAmount < 0)
        {            
            _startColor = Color.white;
            _textMesh.color = _startColor;
            _endColor = Color.cyan;
        }        

        Destroy(gameObject, 5);
    }

    private void LateUpdate()
    {
        ChangeMaterialColorOverTime();
        
        // Look away from the camera (the text will be inverted if it is looking directly at the camera)
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }

    private void ChangeMaterialColorOverTime()
    {
        _textMesh.color = Color.Lerp(_startColor, _endColor, time);

        if (time < 1)
        {
            time += Time.deltaTime / duration;
        }
    }

    public static ScreenPopupText CreateDamagePopup(Vector3 position, int damage, GameObject popupTextGameObject)
    {
        var randomY = Random.Range(1, 4);
        var randomX = Random.Range(-2, 2);
        var offset = new Vector3(randomX, randomY);
        var damagePopupTextTransform = Instantiate(popupTextGameObject, position + offset, Quaternion.identity);
        damagePopupTextTransform.transform.eulerAngles = new Vector3(0, 0, Random.Range(-45, 45));
        ScreenPopupText popupText = damagePopupTextTransform.GetComponent<ScreenPopupText>();
        popupText.Setup(damage);

        return popupText;
    }
}
