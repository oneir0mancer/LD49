using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TutorialSequence : MonoBehaviour
{
    //[SerializeField] private GameObject _worldSidePanel;
    [SerializeField] private GameObject _turnsPanel;
    [SerializeField] private GameObject _winPointsCounter;
    [SerializeField] private GameObject _stabilityCounter;
    [Header("Minister Intro")]
    [SerializeField] private RectTransform _ministersSidePanel;
    [SerializeField] private RectTransform _textPanel1;
    [SerializeField] private TMP_Text _textBox1;
    [SerializeField] private TMP_Text _textBox2;
    [SerializeField] private Province[] _provinces;
    [SerializeField] private GameObject _personalTextPanelBlue;
    [SerializeField] private GameObject _personalTextPanelGreen;
    [SerializeField] private GameObject _personalTextPanelRed1;
    [Header("Tasks and dragging")]
    [SerializeField] private RectTransform _taskDragPanel;
    [SerializeField] private TMP_Text _panel2textBox1;
    [SerializeField] private TMP_Text _panel2textBox2;
    [SerializeField] private TMP_Text _panel2textBox3;
    [SerializeField] private GameObject _personalTextPanelRed2;
    [Header("Turns")]
    [SerializeField] private RectTransform _turnsTutorialPanel;
    [Header("Stability")]
    [SerializeField] private RectTransform _stabilityTutorialPanel;
    [SerializeField] private TMP_Text _panel4textBox1;
    [SerializeField] private TMP_Text _panel4textBox2;
    [SerializeField] private TMP_Text _panel4textBox3;
    [Header("Next")]
    [SerializeField] private RectTransform _grayTasksPanel;
    [SerializeField] private Province[] _foreignProvinces;
    [SerializeField] private GameObject[] _winPointIcons;
    
    private IEnumerator Start()
    {
        HideUI();

        yield return FirstPanelCoroutine();
        yield return SecondPanelCoroutine();
        yield return ThirdPanelCoroutine();
        yield return ForthPanelCoroutine();
        yield return FifthPanelCoroutine();
    }

    private IEnumerator FirstPanelCoroutine()
    {
        DragablePiece.CanDrag = false;
        
        var offset = _textPanel1.position - _ministersSidePanel.position;
        var ministerPos = _ministersSidePanel.position;
        _ministersSidePanel.position += offset.x * Vector3.right;
        
        _textPanel1.gameObject.SetActive(true);    //TODO some animation
        _textPanel1.transform.DOScale(Vector3.zero, 0.5f).From();
        
        yield return new WaitForSeconds(3.5f);
        _textBox1.gameObject.SetActive(false);
        
        _ministersSidePanel.DOMove(ministerPos, 2);
        yield return new WaitForSeconds(2);
        _textBox2.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(3.5f);
        ShowProvinces(_provinces, 0.5f);
        
        yield return new WaitForSeconds(1.5f);
        PersonalBoxAnimation(_personalTextPanelBlue, 2.5f);
        yield return new WaitForSeconds(1.5f);
        PersonalBoxAnimation(_personalTextPanelGreen, 2.5f);
        yield return new WaitForSeconds(1.5f);
        PersonalBoxAnimation(_personalTextPanelRed1, 2.5f);
        yield return new WaitForSeconds(2.5f);

        _textPanel1.transform.DOScale(Vector3.zero, 0.25f)
            .OnComplete(() => _textPanel1.gameObject.SetActive(false));
        
        DragablePiece.CanDrag = true;
    }

    private IEnumerator SecondPanelCoroutine()
    {
        _taskDragPanel.gameObject.SetActive(true);
        _taskDragPanel.transform.DOScale(Vector3.zero, 0.5f).From();
        
        //Wait for drag
        while (GameController.Instance.Turn <= 0)
            yield return null;
        
        _panel2textBox1.gameObject.SetActive(false);
        _panel2textBox2.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        _panel2textBox2.gameObject.SetActive(false);
        _panel2textBox3.gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);
        PersonalBoxAnimation(_personalTextPanelRed2, 1.5f);
        yield return new WaitForSeconds(2f);
        
        _taskDragPanel.transform.DOScale(Vector3.zero, 0.25f)
            .OnComplete(() => _taskDragPanel.gameObject.SetActive(false));
    }
    
    private IEnumerator ThirdPanelCoroutine()
    {
        _turnsPanel.SetActive(true);
        _turnsTutorialPanel.gameObject.SetActive(true);
        _turnsTutorialPanel.transform.DOScale(Vector3.zero, 0.5f).From();
        
        yield return new WaitForSeconds(4f);
        
        _turnsTutorialPanel.transform.DOScale(Vector3.zero, 0.25f)
            .OnComplete(() => _turnsTutorialPanel.gameObject.SetActive(false));
    }
    
    private IEnumerator ForthPanelCoroutine()
    {
        _stabilityCounter.SetActive(true);
        _stabilityTutorialPanel.gameObject.SetActive(true);
        _stabilityTutorialPanel.transform.DOScale(Vector3.zero, 0.5f).From();
        
        yield return new WaitForSeconds(3.5f);
        _panel4textBox1.gameObject.SetActive(false);
        _panel4textBox2.gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);
        _panel4textBox2.gameObject.SetActive(false);
        _panel4textBox3.gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);
        
        _stabilityTutorialPanel.transform.DOScale(Vector3.zero, 0.25f)
            .OnComplete(() => _stabilityTutorialPanel.gameObject.SetActive(false));
    }
    
    private IEnumerator FifthPanelCoroutine()
    {
        _grayTasksPanel.gameObject.SetActive(true);
        _grayTasksPanel.transform.DOScale(Vector3.zero, 0.5f).From();
        
        yield return new WaitForSeconds(2f);
        ShowProvinces(_foreignProvinces, 0.5f);
        ShowWinPoints(0.5f);
        yield return WaitForSecondsOrClick(5f);
        
        _grayTasksPanel.transform.DOScale(Vector3.zero, 0.25f)
            .OnComplete(() => _grayTasksPanel.gameObject.SetActive(false));
    }

    private Sequence PersonalBoxAnimation(GameObject panel, float time)
    {
        var seq = DOTween.Sequence();
        seq.AppendCallback(() => panel.SetActive(true));
        seq.Append(panel.transform.DOScale(Vector3.zero, 0.25f).From());
        seq.AppendInterval(time);
        seq.Append(panel.transform.DOScale(Vector3.zero, 0.25f));
        seq.AppendCallback(() => panel.SetActive(false));
        return seq;
    }

    private void ShowProvinces(Province[] provinces, float timeBetween = 0.5f)
    {
        var seq = DOTween.Sequence();
        foreach (var p in provinces)
        {
            seq.AppendCallback(() => p.gameObject.SetActive(true));
            seq.AppendInterval(timeBetween);
        }
    }

    private void ShowWinPoints(float timeBetween = 0.5f)
    {
        _winPointsCounter.SetActive(true); 
        foreach (var w in _winPointIcons)
        {
            w.SetActive(false);
        }
        
        var seq = DOTween.Sequence();
        foreach (var w in _winPointIcons)
        {
            seq.AppendCallback(() => w.SetActive(true));
            seq.AppendInterval(timeBetween);
        }
    }

    private void HideUI()
    {
        _turnsPanel.SetActive(false);
        _winPointsCounter.SetActive(false);
        _stabilityCounter.SetActive(false);
        
        foreach (var p in _provinces)
            p.gameObject.SetActive(false);
        
        foreach (var p in _foreignProvinces)
            p.gameObject.SetActive(false);
    }

    private IEnumerator WaitForSecondsOrClick(float seconds)
    {
        for (float timePassed = 0; timePassed < seconds; timePassed += Time.deltaTime)
        {
            if (Input.GetMouseButtonDown(0))
                yield break;
            
            yield return null;
        }
    }
}
