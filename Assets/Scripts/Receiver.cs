using System;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour
{
   #region Public Fields
   [NonSerialized] 
   public List<Emitter> emitters = new List<Emitter>();
   #endregion

   #region Private Fields
   private static readonly int _amplificationFactor = 1000;
   #endregion


   #region Unity Methods

   private void Update()
   {
      //GetSignalValue();
   }

   #endregion
   
   #region Public Methods
   public float GetSignalValue()
   {
      float value = 0.0f;

      foreach (Emitter emitter in emitters) 
         value = Mathf.Max(value, emitter.GetEmitterSignalPower());

      return value * _amplificationFactor;
   }

   public Vector3 GetPosition()
   {
      return transform.position;
   }
   #endregion
}
