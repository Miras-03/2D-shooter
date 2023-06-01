using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerName : MonoBehaviour
{
    public TMP_InputField inputName;
    public Button setNameButton;

    public void OnTextFieldChange()
    {
        if(inputName.text.Length > 2) 
            setNameButton.interactable = true;
        else
            setNameButton.interactable = false;
    }

    public void OnClickSetName()
    {
        PhotonNetwork.NickName = inputName.text;
    }
}
