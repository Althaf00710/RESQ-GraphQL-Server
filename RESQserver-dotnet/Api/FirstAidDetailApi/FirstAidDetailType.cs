using Core.Models;
using HotChocolate.Types;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using RESQserver_dotnet.Api.EmergencySubCategoryApi;
using System.Threading.Tasks;

namespace RESQserver_dotnet.Api.FirstAidDetailApi
{
    public class FirstAidDetailType : ObjectType<FirstAidDetail>
    {
        protected override void Configure(IObjectTypeDescriptor<FirstAidDetail> descriptor)
        {
            descriptor.Description("Represents a step in first aid instructions for emergency situations");

            descriptor.Field(fad => fad.Id)
                .Description("The unique identifier of the first aid instruction")
                .Type<NonNullType<IdType>>();

            descriptor.Field(fad => fad.Point)
                .Description("The specific first aid instruction point")
                .Type<NonNullType<StringType>>();

            descriptor.Field(fad => fad.DisplayOrder)
                .Description("The display order of this point in the sequence")
                .Type<NonNullType<IntType>>();

            descriptor.Field(fad => fad.ImageUrl)
                .Description("Optional visual aid URL for this instruction")
                .Type<StringType>();

            descriptor.Field(fad => fad.EmergencySubCategoryId)
                .Description("The ID of the related emergency subcategory")
                .Type<NonNullType<IntType>>();

            // Configure navigation property 
            descriptor.Field(fad => fad.EmergencySubCategory)
                .Description("The emergency subcategory this instruction belongs to")
                .Type<EmergencySubCategoryType>()
                .Resolve(async ctx =>
                {
                    var db = ctx.Service<AppDbContext>();
                    return await db.EmergencySubCategories
                        .AsNoTracking()
                        .FirstOrDefaultAsync(esc => esc.Id == ctx.Parent<FirstAidDetail>().EmergencySubCategoryId);
                });
        }
    }
}