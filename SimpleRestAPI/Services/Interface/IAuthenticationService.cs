using SimpleRestAPI.Models.Authentication;

namespace SimpleRestAPI.Services.Interface
{
    public interface IAuthenticationService
    {
        /*
         * Authentication 
         * Parameters:
         *  model: Contains the name and password of the user are trying to authenticate
         * Return:
         *  AuthenticationReasponse: who contains the authentication token if authentication succeed
         *  null: if authentication fail
         */
        AuthenticationResponse Authentication(AuthenticationRequest model);
    }
}
