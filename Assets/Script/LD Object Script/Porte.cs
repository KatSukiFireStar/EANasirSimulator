using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Porte : MonoBehaviour
{
    bool allOpening;
    [SerializeField] List<Trigger> multiContrainteOpeningTriggers = new List<Trigger>();
    [SerializeField] List<Trigger> nonMultiContrainteOpeningTriggers = new List<Trigger>();
    [SerializeField] List<Trigger> allOpeningTriggers = new List<Trigger>();
    [SerializeField] List<Trigger> nonAllOpeningTriggers = new List<Trigger>();
    [SerializeField]bool locked;
    HingeJoint2D doorPivot; 
    void Start()
    {
        doorPivot = gameObject.GetComponent<HingeJoint2D>();
        
    }
    void Locked(bool _locked)
    {
        if(_locked == true)
        {
            doorPivot.limits.min.Equals(5);
            doorPivot.limits.max.Equals(5);
        }
    }
    //jai probablement fait de la merde
    // verifie si il y a 1 trigger sa ouvre (porte ou)
    bool AllOpening(List<Trigger> _triggers, bool _intialLockedState)
    {
        if(_triggers != null)
        {
            foreach (Trigger _trigger in _triggers)
            {
                if (_trigger.interacted == true)
                {
                    return false;
                }
                return true;
            }
        }
        return _intialLockedState;
    }
    // non ou 
    bool NonAllOpening(List<Trigger> _triggers, bool _intialLockedState)
    {
        if (_triggers != null)
        {
            foreach (Trigger _trigger in _triggers)
            {
                if (_trigger.interacted == true)
                {
                    return true;
                }
                return false;
            }
        }
        return _intialLockedState;
    }
    // verifie si il tout les triger son actif  (porte et)
    bool MultiContrainteOpening(List<Trigger> _triggers,bool _intialLockedState)
    {
        if (_triggers != null)
        {
            foreach (Trigger _trigger in _triggers)
            {
                if (_trigger.interacted == false)
                {
                    return true;
                    
                }
                return false;
            }
        }
        return _intialLockedState;
    }
    //non et
    bool NonMultiContrainteOpening(List<Trigger> _triggers, bool _intialLockedState)
    {
        if (_triggers != null)
        {
            foreach (Trigger _trigger in _triggers)
            {
                if (_trigger.interacted == false)
                {
                    return false;

                }
                return true;
            }
        }
        return _intialLockedState;
    }
    bool KeyOpening(bool _intialLockedState)
    {
        return _intialLockedState;
    }

    void Update()
    {
        Locked(AllOpening(allOpeningTriggers, locked));
        Locked(MultiContrainteOpening(multiContrainteOpeningTriggers, locked));
        Locked(KeyOpening(locked));
    }
}
