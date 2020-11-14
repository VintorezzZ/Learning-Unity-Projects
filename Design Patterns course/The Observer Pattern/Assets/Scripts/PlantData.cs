using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "plantdata", menuName = "Plant Data", order = 51)]
public class PlantData : ScriptableObject
{
   public enum THREAT { None, Low, Moderate, High }

    [SerializeField] private string plantName;
    [SerializeField] private THREAT plantThreat;
    [SerializeField] private Texture icon;

    public string Name { get { return plantName; } }
    public THREAT Threat { get { return plantThreat; } }
    public Texture Icon { get { return icon; } }
}
