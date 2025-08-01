using Core.DTO;
using Core.Services.Interfaces;
using RESQserver_dotnet.Api.UserApi;

namespace RESQserver_dotnet.Api.RescueVehicleApi
{
    [ExtendObjectType<Mutation>]
    public class RescueVehicleMutation
    {
        public async Task<RescueVehiclePayload> CreateRescueVehicle(RescueVehicleCreateInput input, [Service] IRescueVehicleService rescueVehicleService)
        {
            try
            {
                var rescueVehicle = await rescueVehicleService.Add(input);
                return new RescueVehiclePayload(true, "Rescue Vehicle created successfully", rescueVehicle);
            }
            catch (Exception ex)
            {
                return new RescueVehiclePayload(false, ex.Message);
            }
        }
        public async Task<RescueVehiclePayload> UpdateRescueVehicle(int id, RescueVehicleUpdateInput input, [Service] IRescueVehicleService rescueVehicleService)
        {
            try
            {
                var rescueVehicle = await rescueVehicleService.Update(id, input);
                return new RescueVehiclePayload(true, "Rescue Vehicle updated successfully", rescueVehicle);
            }
            catch (Exception ex)
            {
                return new RescueVehiclePayload(false, ex.Message);
            }
        }
        public async Task<RescueVehiclePayload> DeleteRescueVehicle(int id, [Service] IRescueVehicleService rescueVehicleService)
        {
            try
            {
                await rescueVehicleService.Delete(id);
                return new RescueVehiclePayload(true, "Rescue Vehicle deleted successfully");
            }
            catch (Exception ex)
            {
                return new RescueVehiclePayload(false, ex.Message);
            }
        }

        public async Task<RescueVehiclePayload> LoginRescueVehicle(string plateNumber, string password, [Service] IRescueVehicleService rescueVehicleService)
        {
            try
            {
                var authData = await rescueVehicleService.Login(plateNumber, password);

                if (authData.JwtToken == null) return new RescueVehiclePayload(false, "Invalid Username or Password");

                return new RescueVehiclePayload(true, authData.JwtToken, authData.RescueVehicle);
            }
            catch (Exception ex)
            {
                return new RescueVehiclePayload(false, ex.Message);
            }
        }

    }
}
