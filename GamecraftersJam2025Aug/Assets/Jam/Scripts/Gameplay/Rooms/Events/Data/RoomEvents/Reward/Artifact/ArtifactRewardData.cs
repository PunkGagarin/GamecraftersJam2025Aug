using System;
using Jam.Scripts.Gameplay.Artifacts;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    [Serializable]
    public class ArtifactRewardData : RoomRewardEventData
    {
        [field: SerializeField] public override Sprite Sprite { get; set; }
        [field: SerializeField] public ArtifactType ArtifactType { get;set; }
        [field: SerializeField] public RoomEventRewardType Type { get; set; } = RoomEventRewardType.Artifact;

        public override RewardInstance ToInstance() => new(Type, new ArtifactPayload(ArtifactType));     
    }
}