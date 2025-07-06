using Core.Models;

namespace RESQserver_dotnet.Api.SnakeApi
{
    public class SnakeType : ObjectType<Snake>
    {
        protected override void Configure(IObjectTypeDescriptor<Snake> descriptor)
        {
            descriptor.Description("Represents a snake species with venom classification");

            descriptor.Field(s => s.Id)
                .Description("The unique identifier for the snake record")
                .Type<NonNullType<IdType>>();

            descriptor.Field(s => s.Name)
                .Description("The common name of the snake")
                .Type<NonNullType<StringType>>();

            descriptor.Field(s => s.ScientificName)
                .Description("The scientific name (binomial nomenclature) of the snake")
                .Type<NonNullType<StringType>>();

            descriptor.Field(s => s.Description)
                .Description("Detailed description of the snake's characteristics")
                .Type<NonNullType<StringType>>();

            descriptor.Field(s => s.VenomType)
                .Description("Classification of venom (neurotoxic, hemotoxic, cytotoxic, or non-venomous)")
                .Type<NonNullType<StringType>>();

            descriptor.Field(s => s.ImageUrl)
                .Description("URL to an image of the snake")
                .Type<StringType>();

            descriptor.Field("dangerLevel")
                .Description("Computed danger level based on venom type")
                .Type<StringType>()
                .Resolve(ctx =>
                {
                    var snake = ctx.Parent<Snake>();
                    return snake.VenomType switch
                    {
                        "neurotoxic" => "Extremely Dangerous",
                        "hemotoxic" => "Very Dangerous",
                        "cytotoxic" => "Dangerous",
                        "non-venomous" => "Harmless",
                        _ => "Unknown"
                    };
                });
        }
    }
}
