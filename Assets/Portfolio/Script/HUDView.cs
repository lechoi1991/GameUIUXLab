using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Slider healthSlider;

    private int _score;
    private float _remainTime = 60f;
    private float _health = 100f;

    public event Action OnHealthDepleted;

    private void Start()
    {
        _score = 0;
        _remainTime = 60f;
        _health = 100f;

        healthSlider.maxValue = 100f;

        Refresh();
    }

    private void Refresh()
    {
        scoreText.text = _score.ToString();
        timeText.text = _remainTime.ToString("F1");
        healthSlider.value = _health;
    }

    public void AddScoreAndDamage()
    {
        _score += 10;

        _health = Mathf.Max(0f, _health - 10f);

        Refresh();

        if (_health <= 0f)
        {
            OnHealthDepleted?.Invoke();
        }
    }

    public void ResetHUD()
    {
        _score = 0;
        _remainTime = 60f;
        _health = 100f;

        healthSlider.maxValue = 100f;

        Refresh();
    }
}