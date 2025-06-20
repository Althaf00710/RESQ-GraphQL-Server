using Core.Models;
using HotChocolate.Types;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using RESQserver_dotnet.Api.EmergencyCategoryApi;
using RESQserver_dotnet.Api.FirstAidDetailApi;
using System.Threading.Tasks;

namespace RESQserver_dotnet.Api.EmergencySubCategoryApi
{
    public class EmergencySubCategoryType : ObjectType<EmergencySubCategory>
    {
        protected override void Configure(IObjectTypeDescriptor<EmergencySubCategory> descriptor)
        {
            descriptor.Description("Represents a specialized subcategory of emergency situations");

            descriptor.Field(esc => esc.Id)
                .Description("The unique identifier of the emergency subcategory")
                .Type<NonNullType<IdType>>();

            descriptor.Field(esc => esc.Name)
                .Description("The name of the emergency subcategory (e.g., 'Earthquake', 'Flood')")
                .Type<NonNullType<StringType>>();

            descriptor.Field(esc => esc.Description)
                .Description("Detailed description of the emergency subcategory")
                .Type<StringType>();

            descriptor.Field(esc => esc.ImageUrl)
                .Description("URL of an illustrative image for this subcategory")
                .Type<StringType>();

            descriptor.Field(esc => esc.EmergencyCategoryId)
                .Description("The ID of the parent emergency category")
                .Type<NonNullType<IntType>>();

            // Configure navigation properties with resolvers
            descriptor.Field(esc => esc.EmergencyCategory)
                .Description("The parent emergency category this subcategory belongs to")
                .Type<EmergencyCategoryType>()
                .Resolve(async ctx =>
                {
                    var db = ctx.Service<AppDbContext>();
                    return await db.EmergencyCategories
                        .AsNoTracking()
                        .FirstOrDefaultAsync(ec => ec.Id == ctx.Parent<EmergencySubCategory>().EmergencyCategoryId);
                });

            descriptor.Field(esc => esc.FirstAidDetails)
                .Description("First aid instructions specific to this emergency subcategory")
                .Type<ListType<FirstAidDetailType>>()
                .Resolve(async ctx =>
                {
                    var db = ctx.Service<AppDbContext>();
                    return await db.FirstAidDetails
                        .AsNoTracking()
                        .Where(fad => fad.EmergencySubCategoryId == ctx.Parent<EmergencySubCategory>().Id)
                        .OrderBy(fad => fad.DisplayOrder)
                        .ToListAsync();
                });
        }
    }
}