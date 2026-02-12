using Shared.DataTtansferObjects.IdentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IAuthenticationService
    {
        //Login
        //Take Email and Password Then Return Token, Email and DisplayName
        Task<UserDto> LoginAsync(LoginDto loginDto);

        //Register
        //Take Email, Password, UserName, Display Name And Phone Number Then Return Token, Email and Display Name
        Task<UserDto> RegisterAsync(RegisterDto registerDto);

        //Check Email
        //Take Email Then Return boolean
        public Task<bool> CheckEmailExistsAsync(string email);


        //Get Current User Address
        //Take Email Then Return Address
        public Task<AddressDto> CurrentUserAddressAsync(string email);


        //Update Current User Address
        //Take Updated Address and Email Then Return Address after Update 
        public Task<AddressDto> UpdateCurrentUserAddressAsync(string email, AddressDto addressDto);


        //Get Current User 
        //Take Email Then Return Token , Email and Display Name
        public Task<UserDto> GetCurrentUserAsync(string email);

    }
}
