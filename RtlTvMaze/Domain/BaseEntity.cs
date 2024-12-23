using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RtlTvMaze.Domain.Interface;

namespace RtlTvMaze.Domain
{
    public abstract class BaseEntity : IEntity<string>
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        [BsonElement(Order = 0)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement(Order = 101)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement(Order = 101)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonRepresentation(BsonType.Boolean)]
        public bool IsDeleted { get; set; } = false;
    }
}
