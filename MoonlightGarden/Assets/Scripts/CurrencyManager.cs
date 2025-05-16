using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    public int MoonlightShard = 0;
    public int MoonlightShardMax = 9999999;
    public int BloodMoonShard = 0;
    public int BloodMoonShardMax = 9999999;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateMoonlightShard(int amount)
    {
        MoonlightShard += amount;
        MoonlightShard = Mathf.Clamp(MoonlightShard, 0, MoonlightShardMax);
    }

    public int GetMoonlightShard()
    {
        return MoonlightShard;
    }
    public void UpdateBloodMoonShard(int amount)
    {
        BloodMoonShard += amount;
        BloodMoonShard = Mathf.Clamp(BloodMoonShard, 0, BloodMoonShardMax);
    }

    public int GetBloodMoonShard()
    {
        return BloodMoonShard;
    }
}