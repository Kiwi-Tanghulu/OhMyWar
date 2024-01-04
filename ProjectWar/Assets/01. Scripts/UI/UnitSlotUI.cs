using UnityEngine;

public class UnitSlotUI : MonoBehaviour
{
	[SerializeField] UnitInfoSO so;

    private void Awake()
    {
        SkillSlotUI ui = GetComponent<SkillSlotUI>();
        if(so.unitType == UnitType.Healer)
            ui.SetContent($"체력 : {so.maxHealth}\n이동속도 : {so.moveSpeed}\n치유력 : {so.attackDamage}\n사정거리 : {so.attackDistance}");
        else
            ui.SetContent($"체력 : {so.maxHealth}\n이동속도 : {so.moveSpeed}\n공격력 : {so.attackDamage}\n사정거리 : {so.attackDistance}");
    }
}
