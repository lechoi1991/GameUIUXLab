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
    public event Action OnTimeDepleted;

    private bool _isTimerRunning;

    private void Start()
    {
        _score = 0;
        _remainTime = 60f;
        _health = 100f;
        _isTimerRunning = false;

        healthSlider.maxValue = 100f;

        Refresh();
    }

    private void Update()
    {
        if (!_isTimerRunning)
            return;

        _remainTime -= Time.deltaTime;

        if (_remainTime <= 0f)
        {
            _remainTime = 0f;
            _isTimerRunning = false;

            Refresh();
            
            OnTimeDepleted?.Invoke();
            return;
        }

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
         _isTimerRunning = true;

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

        _isTimerRunning = false;

        healthSlider.maxValue = 100f;

        Refresh();
    }
}