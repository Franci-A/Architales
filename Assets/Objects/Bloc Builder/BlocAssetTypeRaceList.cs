using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "ScriptableObjects/Bloc Asset List By Race")]
public class BlocAssetTypeRaceList : ScriptableObject
{
    [Serializable]
    public class RaceBlocAssets
    {
        [Serializable]
        public class BlocAssetByType
        {
            [SerializeField] private BlocAssetType type;
            [SerializeField] private List<Mesh> assets;

            public BlocAssetByType(BlocAssetType type)
            {
                this.type = type;
                assets = new List<Mesh>();
            }
        }

        [SerializeField] Race race;
        [SerializeField] List<BlocAssetByType> assetTypes;

        public RaceBlocAssets(Race race)
        {
            this.race = race;
            assetTypes = new List<BlocAssetByType>();
            GenerateAssetTypes();
        }

        private void GenerateAssetTypes()
        {
            foreach (BlocAssetType blocType in Enum.GetValues(typeof(BlocAssetType)))
            {
                if ((int)blocType < 0) continue;

                assetTypes.Add(new BlocAssetByType(blocType));
            }
        }
    }

    [SerializeField] private List<RaceBlocAssets> assetTypesByRace;

    [Button("Regenerate Lists")]
    private void GenerateAssetTypesListsByRace()
    {
        assetTypesByRace.Clear();

        foreach (Race race in Enum.GetValues(typeof(Race)))
        {
            if ((int)race < 0)
            {
                continue;
            }

            RaceBlocAssets rba = new RaceBlocAssets(race);
            assetTypesByRace.Add(rba);
        }
    }
}
