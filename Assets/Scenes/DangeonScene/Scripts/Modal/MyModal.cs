using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gravitons.UI.Modal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MyModal : Modal
{
    [Tooltip ("Modal title")]
    [SerializeField] protected TextMeshProUGUI m_Title;
    [Tooltip ("Modal body")]
    [SerializeField] protected TextMeshProUGUI m_Body;
    [Tooltip ("Buttons in the modal")]
    [SerializeField] protected Button[] m_Buttons;

    Image _modalBack;

    /// <summary>
    /// Deactivate buttons in awake
    /// </summary>
    public void Awake ()
    {
        for (int i = 0; i < m_Buttons.Length; i++)
        {
            m_Buttons[i].gameObject.SetActive (false);
        }
        _modalBack = GetComponentInParent<Image> ();
    }
    public override void Show (ModalContentBase modalContent, ModalButton[] modalButton)
    {
        GenericModalContent content = (GenericModalContent) modalContent;
        m_Title.text = content.Title;
        m_Body.text = content.Body;

        //Activate buttons and populate properties
        for (int i = 0; i < modalButton.Length; i++)
        {
            if (i >= m_Buttons.Length)
            {
                Debug.LogError ($"Maximum number of buttons of this modal is {m_Buttons.Length}. But {modalButton.Length} ModalButton was given. To display all buttons increase the size of the button array to at least {modalButton.Length}");
                return;
            }
            m_Buttons[i].gameObject.SetActive (true);
            m_Buttons[i].GetComponentInChildren<TextMeshProUGUI> ().text = modalButton[i].Text;
            int index = i; //Closure
            m_Buttons[i].onClick.AddListener (() =>
            {
                if (modalButton[index].Callback != null)
                {
                    modalButton[index].Callback ();
                }

                if (modalButton[index].CloseModalOnClick)
                {
                    Close ();
                }
                m_Buttons[index].onClick.RemoveAllListeners ();
            });
        }
        // modalbackを表示する(modal以外押せないようにするため)
        _modalBack.enabled = true;
        // 表示時のアニメーション
        this.transform.DOScale (new Vector3 (0.1f, 0.1f, 0.1f), 0f);
        this.transform.DOScale (new Vector3 (1f, 1f, 1f), 0.3f);
    }

    public override void Close()
    {
        // modalbackは消す
        _modalBack.enabled = false;
        base.Close();
    }
}