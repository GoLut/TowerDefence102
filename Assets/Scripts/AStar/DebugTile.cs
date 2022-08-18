using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugTile : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI f, g, h;

   public TextMeshProUGUI F
   {
      get => f;
      set => f = value;
   }

   public TextMeshProUGUI G
   {
      get => g;
      set => g = value;
   }

   public TextMeshProUGUI H
   {
      get => h;
      set => h = value;
   }
}
