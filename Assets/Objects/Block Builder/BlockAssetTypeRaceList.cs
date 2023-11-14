using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "ScriptableObjects/Block Asset List By Race")]
public class BlockAssetTypeRaceList : ScriptableObject
{
    [Serializable]
    public class RaceBlockAssets
    {
        [Serializable]
        public class BlockAssetByType
        {
            [SerializeField] private BlockAssetType type;
            [SerializeField] private List<Mesh> assets;

            public BlockAssetByType(BlockAssetType type)
            {
                this.type = type;
                assets = new List<Mesh>();
            }
        }

        [SerializeField] Race race;
        [SerializeField] List<BlockAssetByType> assetTypes;

        public RaceBlockAssets(Race race)
        {
            this.race = race;
            assetTypes = new List<BlockAssetByType>();
            GenerateAssetTypes();
        }

        private void GenerateAssetTypes()
        {
            foreach (BlockAssetType blocType in Enum.GetValues(typeof(BlockAssetType)))
            {
                if ((int)blocType < 0) continue;

                assetTypes.Add(new BlockAssetByType(blocType));
            }
        }
    }

    [SerializeField] private List<RaceBlockAssets> assetTypesByRace;

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

            RaceBlockAssets rba = new RaceBlockAssets(race);
            assetTypesByRace.Add(rba);
        }
    }
}
