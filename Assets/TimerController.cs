using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour {
    public List<Font> Fonts;
    public Text CountText;
    public Outline Outline;
    public Shadow Shadow;
    private List<float> _Intervals = new List<float>();

    public Vector2Int IntervalsCount;
    public Vector2 Multiplayer;
    public Vector2Int PointsAmount;
    public Vector2Int ExclamationAmount;
    public Vector2 FadeTime;
    public Vector2 OutlineDistance;
    public Vector2 ShadowDistance;

    void Start() {
        BeginCountDown();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StopAllCoroutines();
            BeginCountDown();
        }
    }

    public void BeginCountDown() {

        CountText.font = Fonts?[Random.Range(0, Fonts.Count - 1)];
        CountText.color = Random.ColorHSV();
        Outline.effectColor = Random.ColorHSV();
        Outline.effectDistance = new Vector2(Random.Range(OutlineDistance.x, OutlineDistance.y), Random.Range(OutlineDistance.x, OutlineDistance.y));
        Outline.effectColor = Random.ColorHSV();
        Shadow.effectDistance = new Vector2(Random.Range(ShadowDistance.x, ShadowDistance.y), Random.Range(ShadowDistance.x, ShadowDistance.y));
        var intervalsCount = Random.Range(IntervalsCount.x, IntervalsCount.y);

        _Intervals.Clear();
        for (int i = 0; i < intervalsCount; i++) {
            _Intervals.Add(Random.Range(Multiplayer.x, Multiplayer.y));
        }
        StartCoroutine(TimerRoutine());

        //var sum = 0f;
        //var three_two_time = Random.Range(0.1f, 2.3f);
        //sum += three_two_time;
        //var two_one_time = Random.Range(0.1f, sum );
        //sum += two_one_time;
        //var one_go_time = Random.Range(0.1f);
    }

    private IEnumerator TimerRoutine() {
        for (int i = _Intervals.Count; i > 1 ; i--) {
            CountText.text = i.ToString() + RandomSymbol(".", Random.Range(PointsAmount.x, PointsAmount.y));
            yield return new WaitForSeconds(_Intervals[i - 1]);
        }
        CountText.text = "1" + RandomSymbol(".", Random.Range(PointsAmount.x, PointsAmount.y));
        yield return new WaitForSeconds(_Intervals[0]);
        CountText.text = "GO " + RandomSymbol("!", Random.Range(ExclamationAmount.x, ExclamationAmount.y));
        yield return new WaitForSeconds(Random.Range(FadeTime.x, FadeTime.y));
        CountText.text = "";
    }

    private string RandomSymbol(string s, int count) {
        var result = "";
        //var count = Random.Range(PointsAmount.x, PointsAmount.y);
        for (int i = 0; i < count; i++) {
            result += s;
        }
        return result;
    }
}