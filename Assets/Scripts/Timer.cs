using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timer = 0;
    [SerializeField]
    private TMPro.TextMeshPro _TimerText;
    private bool _timerCounting = false;
    // Start is called before the first frame update
    void Start()
    {
        _TimerText = GetComponentInChildren<TMPro.TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerCounting)
        {
            timer += Time.deltaTime;

            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);
            string timeFormated = string.Format("{0:00}:{1:00}", minutes, seconds);
            _TimerText.text = timeFormated;
        }
    }

    public void StartTimer()
    {
        _timerCounting = true;
    }
    public void StopTimer()
    {
        _timerCounting = false;
    }
    
    public void ResetTimer()
    {
        timer = 0;
    }
}
