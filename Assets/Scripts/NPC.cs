using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public void TalkToNPC(Transform talker)
    {
        transform.LookAt(talker);
        InGameUI.Instance.OpenNPCTalkWindow();
    }
}
