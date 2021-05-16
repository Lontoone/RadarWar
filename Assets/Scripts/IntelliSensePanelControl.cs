using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class IntelliSensePanelControl : MonoBehaviour
{
    public GameObject intelliSense_item;
    public Transform intelliSense_container;
    public const string INTELLISENSE_ITEM_GC = "INTELLISENSE_ITEM_GC";

    public InputFieldCaretPosition input;

    public static IntelliSensePanelControl instance;
    float panle_half_size;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        GCManager.RegisterObject(INTELLISENSE_ITEM_GC, intelliSense_item);

        input.onValueChanged.AddListener(OnInput);

        panle_half_size = transform.GetComponent<RectTransform>().rect.width * .25f;
    }
    private void Update()
    {
        //按下tab自動補齊
        if (Input.GetKeyDown(KeyCode.Tab) && intelliSense_container.childCount > 0)
        {
            //找到tag並replace
            Regex regex = new Regex(RPGCore.REGEX_SPLIT_LINES);

            IntelliSenseHintItem _item = intelliSense_container.GetChild(0).GetComponentInChildren<IntelliSenseHintItem>();
            string _new_tag = _item.commang_text.text;
            string _tag = input.text.Replace(regex, "tag", _new_tag);

            Debug.Log("tab " + _tag);
            input.text = _tag;

            //移標移至最後
            input.MoveCarcetToLast();
        }
    }

    public void OnInput(string _input)
    {
        Vector2 _localpos = input.GetLocalCaretPosition();
        _localpos.x += panle_half_size;
        Vector2 _worldpos = (Vector2)input.transform.TransformPoint(_localpos);
        Debug.Log(_localpos + " " + _worldpos + " " + panle_half_size);

        //跟著輸入移動
        intelliSense_container.transform.position = new Vector2(_worldpos.x, intelliSense_container.transform.position.y);

        //每次都刷新 
        foreach (Transform child in intelliSense_container)
        {
            GCManager.Destory(INTELLISENSE_ITEM_GC, child.gameObject);
        }

        //沒輸入時隱藏
        if (string.IsNullOrWhiteSpace(_input))
            return;

        //input從dict找相符的選項
        foreach (KeyValuePair<string, System.Action<object[]>> kp in CommandLib.CommandDict)
        {
            if (kp.Key.Contains(_input.Replace("/", "")))
            {
                IntelliSenseHintItem _hint = GCManager.Instantiate(INTELLISENSE_ITEM_GC, intelliSense_container).GetComponent<IntelliSenseHintItem>();
                _hint.transform.SetAsFirstSibling();
                _hint.transform.SetParent(intelliSense_container);

                _hint.commang_text.text = kp.Key;

                //該tag是否有info
                string _info = "";
                if (CommandLib.InfoDict.TryGetValue(kp.Key, out _info))
                    _hint.info_text.text = _info;
            }
        }

    }
}
