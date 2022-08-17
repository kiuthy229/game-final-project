using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChatManager : MonoBehaviour
{
    public PlayerHit plMove;
    public PhotonView photonView;
    public GameObject BubbleSpeechObject;
    public Text UpdatedText;

    private InputField ChatInputField;
    private bool DisableSend;

    private void Awake()
    {
        ChatInputField = GameObject.Find("ChatInputField").GetComponent<InputField>();
    }

    private void Update()
    {
        if (photonView.isMine)
        {
            if(!DisableSend && ChatInputField.isFocused)
            {
                if (ChatInputField.text != "" && ChatInputField.text.Length > 0 && Input.GetKeyDown(KeyCode.LeftShift))
                {
                    photonView.RPC("SendMessage", PhotonTargets.AllBuffered, ChatInputField.text);
                    BubbleSpeechObject.SetActive(true);

                    ChatInputField.text = "";
                    DisableSend = true;
                }
            }
        }
    }

    [PunRPC]
    private void SendMessage(string message)
    {
        UpdatedText.text = message;
        StartCoroutine("Remove");
    }

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(4f);
        BubbleSpeechObject.SetActive(false);
        DisableSend = false;
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(BubbleSpeechObject.active);
        }
        else if(stream.isReading)
        {
            BubbleSpeechObject.SetActive((bool)stream.ReceiveNext());
        }
    }
}
