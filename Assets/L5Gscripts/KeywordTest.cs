using System;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class KeywordTest : MonoBehaviour
{

    private KeywordController keyCon;
    private TargetTracking tageTra;
    private string[][] keywords;

    public GameObject refObj;
    public GameObject fukidashiObj;
    public GameObject minimapObj;
    TargetTracking script;
    ChangeAgentText CAscript;


    // Use this for initialization
    void Start()
    {
        keywords = new string[6][];
        keywords[0] = new string[] { "ゆにてぃちゃん", "ゆにてぃ" };//ひらがなでもカタカナでもいい
        keywords[1] = new string[] { "マップ", "ちず" };
        keywords[2] = new string[] { "おすすめ", "どこいけば" };
        keywords[3] = new string[] { "あんない", "あんないして" };
        keywords[4] = new string[] { "せつめい", "ここはなに" };
        keywords[5] = new string[] { "ありがとう", "もういいよ" };


        keyCon = new KeywordController(keywords, true);//keywordControllerのインスタンスを作成
        keyCon.SetKeywords();//KeywordRecognizerにkeywordsを設定する
        keyCon.StartRecognizing(0); //シーン中で音声認識を始めたいときに呼び出す

        script = refObj.GetComponent<TargetTracking>();
        CAscript = fukidashiObj.GetComponent<ChangeAgentText>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (keyCon.hasRecognized[0])//menu 止まってこっちを向く
        {
            Debug.Log("keyword[0] was recognized");
            keyCon.hasRecognized[0] = false;
            keyCon.StopRecognizing(0);
            keyCon.StartRecognizing(1);
            keyCon.StartRecognizing(2);
            keyCon.StartRecognizing(5);

            script.WalkFront();
            fukidashiObj.SetActive(true);
            CAscript.ChangeText("はい!");
        }
        if (keyCon.hasRecognized[1])//map マップを表示
        {
            Debug.Log("keyword[1] was recognized");
            keyCon.hasRecognized[1] = false;
            keyCon.StopRecognizing(1);
            keyCon.StopRecognizing(2);

            minimapObj.SetActive(true);
            fukidashiObj.SetActive(false);

        }
        if (keyCon.hasRecognized[2])//recommend おすすめを教える
        {
            Debug.Log("keyword[2] was recognized");
            keyCon.hasRecognized[2] = false;
            keyCon.StopRecognizing(1);
            keyCon.StopRecognizing(2);
            keyCon.StartRecognizing(3);

            CAscript.ChangeText("高汐先生の部屋かトイレかな");
        }
        if (keyCon.hasRecognized[3])//guide 案内する
        {
            Debug.Log("keyword[3] was recognized");
            keyCon.hasRecognized[3] = false;
            keyCon.StopRecognizing(3);
            keyCon.StartRecognizing(4);

            script.GuideStart();
            fukidashiObj.SetActive(false);
            script.SetDestination();
        }
        if (keyCon.hasRecognized[4])//explain 説明する
        {
            Debug.Log("keyword[4] was recognized");
            keyCon.hasRecognized[4] = false;
            keyCon.StopRecognizing(4);

            fukidashiObj.SetActive(true);
            CAscript.ChangeText("Kaz-san is Here");

            script.ShowFaceToMaster();
        }
        if (keyCon.hasRecognized[5])//back 待機状態に戻る
        {
            Debug.Log("keyword[5] was recognized");
            keyCon.hasRecognized[5] = false;
            keyCon.StopRecognizing(1);
            keyCon.StopRecognizing(2);
            keyCon.StopRecognizing(3);
            keyCon.StopRecognizing(4);
            keyCon.StopRecognizing(5);
            keyCon.StartRecognizing(0);

            minimapObj.SetActive(false);
            fukidashiObj.SetActive(false);
            script.GoFirst();
        }



    }
}
