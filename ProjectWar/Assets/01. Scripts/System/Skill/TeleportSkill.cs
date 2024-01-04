using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System.Linq;
using DG.Tweening;
[System.Serializable]
public class TelePortPos
{
    public Vector2 startPos;
    public Vector2 endPos;
}
public class TeleportSkill : SkillBase
{
    [SerializeField] private float magicCircleTime;
    [SerializeField] private float maxMidY;
    [SerializeField] private float minMidY;
    [SerializeField] private TelePortPos[] blueTelePortPos;
    [SerializeField] private TelePortPos[] redTelePortPos;
    [SerializeField] private float teleportRange;
    [SerializeField] private float teleportDelay;
    [SerializeField] private LayerMask targetLayer;

    [SerializeField] private GameObject teleportEffect;
    [SerializeField] private GameObject magicCircleEffect;

    private List<Transform> units = new List<Transform>();
    private List<Vector2> unitDistances = new List<Vector2>();
    private float currentPercent;
    private Vector2 playerTeleportPosition;
    private int currentLineIndex;

    private TelePortPos[] currentTeleportPos;
    private PlayerMovement playerMovement = null;
    protected override bool ActiveSkill()
    {
        if (playerMovement == null)
            playerMovement = player.GetComponent<PlayerMovement>();

        playerMovement.SetIsTeleport(true);
        if (playerMovement.IsTeleport == true)
            return false;

        Debug.Log("TeleportStart");

        List<Transform> targetUnits = new List<Transform>();
        units = new List<Transform>();
        unitDistances = new List<Vector2>();
        currentPercent = 0f;
        string target = player.IsBlue ? "BlueUnit" : "RedUnit";
        Collider2D[] cols = Physics2D.OverlapCircleAll(player.transform.position, teleportRange);

        for (int i = 0; i < 3; i++)
        {
            blueTelePortPos[i].startPos = IngameManager.Instance.BluePoint[i].position;
            blueTelePortPos[i].endPos = IngameManager.Instance.NexusPoint[i].position;
        }

        for (int i = 0; i < 3; i++)
        {
            redTelePortPos[i].startPos = IngameManager.Instance.RedPoint[i].position;
            redTelePortPos[i].endPos = IngameManager.Instance.NexusPoint[i].position;
        }
        currentTeleportPos = player.IsBlue ? blueTelePortPos : redTelePortPos;
        foreach (Collider2D col in cols)
        {
            if (col.CompareTag(target) == true)
            {
                targetUnits.Add(col.transform);
            }
        }

        if(targetUnits != null)
        {
            Debug.Log("target unit count : " + targetUnits.Count);
            units = targetUnits.OrderBy(x => Vector2.Distance(player.transform.position, x.transform.position)).ToList();
            foreach (Transform unit in units)
            {
                unitDistances.Add(unit.position - player.transform.position);
            }
        }

        int lineIndex = IngameManager.Instance.FocusedLine;

        Debug.Log(lineIndex);
        
        currentLineIndex = player.transform.position.y > maxMidY ? 2 : player.transform.position.y < minMidY ? 0 : 1;

        currentPercent = Mathf.Abs(player.transform.position.x - currentTeleportPos[currentLineIndex].startPos.x)
            / Mathf.Abs((currentTeleportPos[currentLineIndex].endPos.x - currentTeleportPos[currentLineIndex].startPos.x));

        currentPercent = Mathf.Clamp(currentPercent, 0f, 1f);
        
        Debug.Log(currentPercent);

        playerTeleportPosition =
           currentTeleportPos[lineIndex].startPos + currentPercent * (currentTeleportPos[lineIndex].endPos - currentTeleportPos[lineIndex].startPos);

        StartCoroutine(TelePortStart());
        
        return true;
    }

    [ClientRpc]
    public void FinishTeleportClientRPC()
    {
        StartCoroutine(TelePortEnd());
    }

    private IEnumerator TelePortStart()
    {
        Debug.Log("TeleportEffectCoru");

        Instantiate(magicCircleEffect, player.transform.position, Quaternion.Euler(0f,0f,90f));


        if (units != null)
        {
            Debug.Log("10");
            foreach (var unit in units)
            {
                Instantiate(teleportEffect, unit.transform.position, Quaternion.identity);
                unit.gameObject.SetActive(false);
                yield return new WaitForSeconds(teleportDelay);
                Debug.Log("11");
            }
        }

        Instantiate(teleportEffect, player.transform.position, Quaternion.identity);
        player.transform.Find("Visual").gameObject.SetActive(false);
        if (IsHost)
        {
            player.GetComponent<PlayerMovement>().MoveImmediately(playerTeleportPosition);
            FinishTeleportClientRPC();
        }
    }

    private IEnumerator TelePortEnd()
    {
        Instantiate(magicCircleEffect, player.transform.position, Quaternion.Euler(0f,0f,90f));

        yield return new WaitForSeconds(1f);

        Instantiate(teleportEffect, player.transform.position, Quaternion.identity);
        player.transform.Find("Visual").gameObject.SetActive(true);

        yield return new WaitForSeconds(teleportDelay);

        for (int i = 0; i < unitDistances.Count; i++)
        {
            if (IsHost)
            {
                units[i].position = player.transform.position + (Vector3)unitDistances[i];
            }
            Instantiate(teleportEffect, units[i].transform.position, Quaternion.identity);
            units[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(teleportDelay);
        }
        playerMovement.SetIsTeleport(false);
    }
}
