using FCG.Domain.ValueObjects;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace FCG.Infrastructure.Mappings.Queries.Serializers;
public class PasswordSerializer : SerializerBase<Password>
{
    public override Password Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        => Password.FromHash(context.Reader.ReadString());

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Password value)
        => context.Writer.WriteString(value.ToString());
}
