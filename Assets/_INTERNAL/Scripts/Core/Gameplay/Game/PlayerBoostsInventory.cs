using System;
using UnityEngine;

namespace Core.Gameplay.Game
{
    public static class PlayerBoostsInventory
    {
        private const string BoostCountKeyPrefix = "Boost_Count_";

        public static event Action<BoostType, int> OnBoostCountChanged;

        public static int GetCount(BoostType type)
        {
            return PlayerPrefs.GetInt(GetKey(type), 0);
        }

        public static void Add(BoostType type, int amount = 1)
        {
            if (amount <= 0)
            {
                Debug.LogWarning($"Can not add non-positive boost amount: {amount}");
                return;
            }

            SetCount(type, GetCount(type) + amount);
        }

        public static bool TryConsume(BoostType type, int amount = 1)
        {
            if (amount <= 0)
            {
                Debug.LogWarning($"Can not consume non-positive boost amount: {amount}");
                return false;
            }

            int currentCount = GetCount(type);
            if (currentCount < amount)
                return false;

            SetCount(type, currentCount - amount);
            return true;
        }

        private static void SetCount(BoostType type, int count)
        {
            int safeCount = Mathf.Max(0, count);
            PlayerPrefs.SetInt(GetKey(type), safeCount);
            PlayerPrefs.Save();
            OnBoostCountChanged?.Invoke(type, safeCount);
        }

        private static string GetKey(BoostType type) => $"{BoostCountKeyPrefix}{type}";
    }
}