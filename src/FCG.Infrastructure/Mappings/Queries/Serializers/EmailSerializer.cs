using FCG.Domain.ValueObjects;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace FCG.Infrastructure.Mappings.Queries.Serializers;
public class EmailSerializer : SerializerBase<Email>
{
    public override Email Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        => new Email(context.Reader.ReadString());

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Email value)
        => context.Writer.WriteString(value.ToString());
}
