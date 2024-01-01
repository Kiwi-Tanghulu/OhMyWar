using UnityEngine;

public class IngameManager : MonoBehaviour
{
    private static IngameManager instance;
    public static IngameManager Instance {
        get {
            if(instance == null)
                instance = FindObjectOfType<IngameManager>();
            return instance;
        }
    }
}
