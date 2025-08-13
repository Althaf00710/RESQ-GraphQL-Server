using Core.DTO;
using Core.Services.Interfaces;

namespace RESQserver_dotnet.Api.CivilianApi
{
    [ExtendObjectType<Mutation>]
    public class CivilianMutation
    {
        public async Task<CivilianPayload> CreateCivilian(CivilianCreateInput input, [Service] ICivilianService service)
        {
            try
            {
                var created = await service.Add(input);
                return new CivilianPayload(true, "Civilian created successfully", created);
            }
            catch (Exception ex)
            {
                return new CivilianPayload(false, $"Failed to create civilian: {ex.Message}");
            }
        }

        public async Task<CivilianPayload> UpdateCivilian(
            int id,
            CivilianUpdateInput input,
            [Service] ICivilianService service)
        {
            try
            {
                var updated = await service.Update(id, input);
                return new CivilianPayload(true, "Civilian updated successfully", updated);
            }
            catch (Exception ex)
            {
                return new CivilianPayload(false, $"Failed to update civilian: {ex.Message}");
            }
        }

        public async Task<CivilianPayload> RestrictCivilian(
            int id,
            [Service] ICivilianService service)
        {
            try
            {
                var deleted = await service.Restrict(id);
                if (!deleted)
                    return new CivilianPayload(false, $"Civilian with ID {id} not found");

                return new CivilianPayload(true, "Civilian Restricted successfully");
            }
            catch (Exception ex)
            {
                return new CivilianPayload(false, $"Failed to restrict civilian: {ex.Message}");
            }
        }

        public async Task<CivilianPayload> UnrestrictCivilian(
            int id,
            [Service] ICivilianService service)
        {
            try
            {
                var deleted = await service.Unrestrict(id);
                if (!deleted)
                    return new CivilianPayload(false, $"Civilian with ID {id} not found");

                return new CivilianPayload(true, "Civilian Unrestricted successfully");
            }
            catch (Exception ex)
            {
                return new CivilianPayload(false, $"Failed to Unrestrict civilian: {ex.Message}");
            }
        }

        public async Task<CivilianPayload> SendCivilianOtp(
            string phoneNumber,
            [Service] ICivilianService service)
        {
            try
            {
                var sent = await service.SendOTP(phoneNumber);
                if (!sent)
                    return new CivilianPayload(false, "Failed to send OTP");
                return new CivilianPayload(true, "OTP sent successfully");
            }
            catch (Exception ex)
            {
                return new CivilianPayload(false, $"Failed to send OTP: {ex.Message}");
            }
        }

        public async Task<CivilianPayload> LoginCivilian(
            string phoneNumber,
            int otp,
            [Service] ICivilianService service)
        {
            try
            {
                var login = await service.Login(phoneNumber, otp);
                return new CivilianPayload(true, login.JwtToken, login.Civilian);
            }
            catch (Exception ex)
            {
                return new CivilianPayload(false, $"Login failed: {ex.Message}");
            }
        }
    }
}
