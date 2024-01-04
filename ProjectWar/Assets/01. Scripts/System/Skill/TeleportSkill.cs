using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using Unity.Netcode.Components;

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
    private int lineIndex;


    private TelePortPos[] currentTeleportPos;
    private PlayerMovement playerMovement = null;
    protected override bool ActiveSkill()
    {
        if (playerMovement == null)
            playerMovement = player.GetComponent<PlayerMovement>();

        if (playerMovement.IsTeleport)
            return false;

        playerMovement.SetIsTeleport(true);

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

        if (targetUnits != null)
        {
            Debug.Log("target unit count : " + targetUnits.Count);
            units = targetUnits.OrderBy(x => Vector2.Distance(player.transform.position, x.transform.position)).ToList();
            foreach (Transform unit in units)
            {
                unitDistances.Add(unit.position - player.transform.position);
            }
        }

        currentLineIndex = player.transform.position.y > maxMidY ? 0 : player.transform.position.y < minMidY ? 2 : 1;
        Debug.Log(currentLineIndex);
        lineIndex = 0;
        if (currentLineIndex == 2)
            lineIndex = 1;
        else if (currentLineIndex == 1)
            lineIndex = 0;
        else
            lineIndex = 2;
        Debug.Log(lineIndex);
        currentPercent = Mathf.Abs(player.transform.position.x - currentTeleportPos[currentLineIndex].startPos.x)
            / Mathf.Abs((currentTeleportPos[currentLineIndex].endPos.x - currentTeleportPos[currentLineIndex].startPos.x));

        currentPercent = Mathf.Clamp(currentPercent, 0f, 1f);

        Debug.Log(currentPercent);

        playerTeleportPosition =
           currentTeleportPos[lineIndex].startPos + currentPercent * (currentTeleportPos[lineIndex].endPos - currentTeleportPos[lineIndex].startPos);

        Debug.Log($"Target Start Pos : {currentTeleportPos[lineIndex].startPos} Target End Pos : {currentTeleportPos[lineIndex].endPos} Player Teleport Pos : {playerTeleportPosition}");

        StartCoroutine(TelePortStart());

        return true;
    }

    [ClientRpc]
    public void FinishTeleportClientRPC()
    {
        StartCoroutine(TelePortEnd());
        Debug.Log("È£½ºÆ®µµ ½ò ¼ö ÀÖ¾î!");
    }

    private IEnumerator TelePortStart()
    {
        

        Debug.Log("TeleportEffectCoru");

        Instantiate(magicCircleEffect, player.transform.position, Quaternion.Euler(0f, 0f, 90f));


        if (units != null)
        {
            foreach (var unit in units)
            {
                Instantiate(teleportEffect, unit.transform.position, Quaternion.identity);
                unit.gameObject.SetActive(false);
                yield return new WaitForSeconds(teleportDelay);
            }
        }
        if(units.Count <= 0)
        {
            yield return new WaitForSeconds(0.7f);
        }
        Instantiate(teleportEffect, player.transform.position, Quaternion.identity);
        player.transform.Find("Visual").gameObject.SetActive(false);
        player.GetComponent<PlayerMovement>().MoveImmediately(playerTeleportPosition);
        
        for(int i = 0; i < unitDistances.Count; i++)
        {
            if (IsHost)
            {
                units[i].GetComponent<NetworkTransform>().Teleport(player.transform.position + (Vector3)unitDistances[i],Quaternion.identity,new Vector3(1f,1f,1f));
                units[i].GetComponent<UnitMovement>().SetLine(lineIndex);
                Debug.Log("¿Å±è");
            }
        }

        FinishTeleportClientRPC();
    }

    private IEnumerator TelePortEnd()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(magicCircleEffect, player.transform.position, Quaternion.Euler(0f, 0f, 90f));
        yield return new WaitForSeconds(1f);

        Instantiate(teleportEffect, player.transform.position, Quaternion.identity);
        player.transform.Find("Visual").gameObject.SetActive(true);

        yield return new WaitForSeconds(teleportDelay);

        for (int i = 0; i < unitDistances.Count; i++)
        {
            //if (IsHost)
            //{
            //    units[i].position = player.transform.position + (Vector3)unitDistances[i];
            //}
            Instantiate(teleportEffect, units[i].transform.position, Quaternion.identity);
            units[i].gameObject.SetActive(true);
            Debug.Log("´Ù½Ãº¸¿©ÁÜ");
            yield return new WaitForSeconds(teleportDelay);
        }
        playerMovement.SetIsTeleport(false);
        playerMovement.SetMoveable(true);
    }
}
