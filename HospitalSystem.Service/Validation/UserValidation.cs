using HospitalSystem.Infrastructure.DTOs.Users;

namespace HospitalSystem.API.Validation
{
    public static class UserValidation
    {
        public static bool ValidateUserId(int UserId) => UserId > 0;

        private static bool _ValidateRole(byte Role) => Role >= 1 && Role < 4;

        private static bool _ValidatePermissions(int Permissions) => Permissions != 0 && Permissions >= -1;

        public static bool ValidateUsername(string Username) => !string.IsNullOrEmpty(Username);
        public static bool ValidateAddUserInput(AddUserDto addUserDto) =>
            ValidateUsername(addUserDto.Username) &&
            !string.IsNullOrEmpty(addUserDto.Password) &&
            _ValidateRole(addUserDto.Role) &&
            _ValidatePermissions(addUserDto.Permissions);

        public static bool ValidateUpdateUserInput(UpdateUserDto updateUserDto) =>
            ValidateUserId(updateUserDto.UserId) &&
            ValidateUsername(updateUserDto.Username) &&
            _ValidateRole(updateUserDto.Role) &&
            _ValidatePermissions(updateUserDto.Permissions);

        public static bool ValidateChangePasswordInput(ChangePasswordDto changePasswordDto) =>
           ValidateUserId(changePasswordDto.UserId) && !string.IsNullOrEmpty(changePasswordDto.Password);

        public static bool ValidateFindUserInput(FindUserDto findUserDto) =>
            ValidateUsername(findUserDto.Username) && !string.IsNullOrEmpty(findUserDto.Password);
    }
}
