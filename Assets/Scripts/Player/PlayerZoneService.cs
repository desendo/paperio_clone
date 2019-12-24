using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Game
{


    
    public class PlayerZoneService : IReceive<SignalZoneBorderPass>
    {
        [Inject]
        PlayerZone zone;
        [Inject]
        PlayerRunner runner;
        [Inject]
        PlayerLine line;

        public PlayerZoneService()
        {
            SignalsController.Default.Add(this);
        }
        ~PlayerZoneService()
        {
            SignalsController.Default.Remove(this);
        }
        internal void ExitHomeZone()
        {
            Debug.Log("ExitHomeZone ");


        }

        internal void EnterHomeZone()
        {
            Debug.Log("EnterHomeZone ");


            if (line.LineDots.Count < 2) return;

        }

        public void HandleSignal(SignalZoneBorderPass arg)
        {
            if (arg.zone == zone)
            {
                if (arg.isExiting)
                {
                    ExitHomeZone();                    
                }
                else
                    EnterHomeZone();
            }
        }
    }
}
