using HospitalSystem.DTOs;

namespace HospitalSystem.API.Validation
{
    public static class UserValidation
    {
        private static bool _ValidateRole(byte Role) => Role >= 1 && Role < 4;

        private static bool _ValidatePermissions(int Permissions) => Permissions != 0 && Permissions >= -1;

        public static bool ValidateAddUserInput(AddUserDto addUserDto) =>
            !string.IsNullOrEmpty(addUserDto.Username) &&
            !string.IsNullOrEmpty(addUserDto.Password) &&
            _ValidateRole(addUserDto.Role) &&
            _ValidatePermissions(addUserDto.Permissions);

        public static bool ValidateUpdateUserInput(UpdateUserDto updateUserDto) =>
            updateUserDto.UserId > 0 &&
            !string.IsNullOrEmpty(updateUserDto.Username) &&
            _ValidateRole(updateUserDto.Role) &&
            _ValidatePermissions(updateUserDto.Permissions);
        
    }
}
