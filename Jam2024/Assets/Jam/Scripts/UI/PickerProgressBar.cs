using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickerProgressBar : MonoBehaviour
{
    [SerializeField] private Color _defaultColor;

    [SerializeField] private Image _progressBar;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void UpdateProgressBar(float progress)
    {
        _progressBar.transform.localScale = new Vector3(progress, 1, 1);

        if (progress == 1)
        {
            gameObject.SetActive(false);
        }
    }

    public void RestartProgressBar()
    {
        gameObject.SetActive(false);
        UpdateProgressBar(0);
    }
}
