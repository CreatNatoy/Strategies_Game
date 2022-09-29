using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<int> OnPrintCoin;

    public static void PrintCoin(int coin) => OnPrintCoin?.Invoke(coin); 
}
