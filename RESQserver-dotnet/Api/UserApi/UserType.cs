using Core.models;

namespace RESQserver_dotnet.Api.UserApi
{
    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            descriptor.Field(u => u.Id).Type<NonNullType<IntType>>();
            descriptor.Field(u => u.Name).Type<NonNullType<StringType>>();
            descriptor.Field(u => u.Email).Type<NonNullType<StringType>>();
            descriptor.Field(u => u.JoinedDate).Type<NonNullType<DateTimeType>>();
            descriptor.Field(u => u.LastActive).Type<NonNullType<StringType>>();
            descriptor.Field(u => u.Username).Type<NonNullType<StringType>>();
            descriptor.Field(u => u.ProfilePicturePath).Type<StringType>();

            descriptor.Field(u => u.Password).Ignore();
        }
    }
}
