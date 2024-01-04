using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class TelePortPos
{
    public Vector2 startPos;
    public Vector2 endPos;
}
public class TeleportSkill : SkillBase
{
    [SerializeField] private float maxMidY;
    [SerializeField] private float minMidY;
    [SerializeField] private TelePortPos[] telePortPos;
    [SerializeField] private float teleportRange;
    [SerializeField] private float teleportDelay;
    [SerializeField] private LayerMask targetLayer;

    [SerializeField] private GameObject teleportEffect;

    private List<Transform> units = new List<Transform>();
    private List<Vector2> unitDistances = new List<Vector2>();
    private float currentPercent;
    private Vector2 playerTeleportPosition;
    private int currentLineIndex;
    protected override bool ActiveSkill()
    {
        Debug.Log("TeleportStart");

        List<Transform> targetUnits = new List<Transform>();
        units = new List<Transform>();
        unitDistances = new List<Vector2>();
        currentPercent = 0f;
        string target = player.IsBlue ? "BlueUnit" : "RedUnit";
        Collider2D[] cols = Physics2D.OverlapCircleAll(player.transform.position, teleportRange);

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
        
        currentLineIndex = player.transform.position.y > maxMidY ? 2 : player.transform.position.y < minMidY ? 0 : 1;

        currentPercent = Mathf.Abs(player.transform.position.x - telePortPos[lineIndex].startPos.x)
            / Mathf.Abs((telePortPos[lineIndex].endPos.x - telePortPos[lineIndex].startPos.x));

        currentPercent = Mathf.Clamp(currentPercent, 0f, 1f);
        
        Debug.Log(currentPercent);

        playerTeleportPosition = 
           telePortPos[currentLineIndex].startPos + currentPercent * (telePortPos[currentLineIndex].endPos - telePortPos[currentLineIndex].startPos);

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
        if(units != null)
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

        if (IsHost)
        {
            Debug.Log(playerTeleportPosition);
            player.GetComponent<PlayerMovement>().MoveImmediately(playerTeleportPosition);
            FinishTeleportClientRPC();
        }
    }

    private IEnumerator TelePortEnd()
    {
        for(int i = 0; i < unitDistances.Count; i++)
        {
            if (IsHost)
            {
                units[i].position = player.transform.position + (Vector3)unitDistances[i];
            }
            Instantiate(teleportEffect, units[i].transform.position, Quaternion.identity);
            units[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(teleportDelay);
        }
    }
}
