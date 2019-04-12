using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    public int collectables;
    public void AddCollectable(int i) {
        {
            collectables += i;
        }
    }
}
