using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BIGMESS/Object settings")]
public class ObjectSettings : ScriptableObject
{
    public ObjectType objectType;
    public ObjectWeight weightType;

  //  public ObjectPriorityList priorityList;

    public Vector3 rotation;

    public bool IsTool()
    {
        return objectType == ObjectType.toolFreeHand //missing ---------
            || objectType == ObjectType.toolGardening
            || objectType == ObjectType.toolOwen
            || objectType == ObjectType.toolPaintingBrush
            || objectType == ObjectType.toolStationaryElectricWashing
            || objectType == ObjectType.toolStationaryWashing
            || objectType == ObjectType.toolWashing
            || objectType == ObjectType.toolWatering
            || objectType == ObjectType.plug; //-------------
    }
    
    public bool NeedsToBePlugged()
    {
        return objectType == ObjectType.toolStationaryElectricWashing;
    }

    public enum ObjectWeight
    {
        light,  // basic object
        medium, // object that slow you down
        heavy,   // object that need 2 players to be bringed
        immobile //stationary tools
    }

    public enum ObjectType
    {
        plug, //not sure-------------
        toolFreeHand,
        toolGardening,
        toolOwen,
        toolPaintingBrush,
        toolStationaryElectricWashing,
        toolStationaryWashing,
        toolWashing,
        toolWatering,
        couch,
        pillow,
        table,
        chairs,
        flowerPot,
        mugs,
        flowers,
        plate,
        framedPictures,
        fridge,
        lamp,
        bed,
        door,
        livingRoomTable,
        bedRoomTable,
        leek,
        carrots,
        chouFlour,
        radio,
        dirtPiles,
        pumpkin,
        electricPlug,
        

    }
}
