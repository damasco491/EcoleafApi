﻿using HotChocolate;
using HotChocolate.Types;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Common.DTO.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using HotChocolate.Authorization;
using Common.Model.Global.Input;
using EcoleafAPI.Services.Queries.Users;

namespace EcoleafAPI.GraphQL.QueryTypes
{
    [ExtendObjectType("Query")]
    public class UsersQuery
    {
        private readonly IMediator _mediator;

        public UsersQuery(IMediator mediator)
        {
            _mediator = mediator;
        }

        [GraphQLName("getUserInfo")]
        [Authorize]
        public async Task<UserDTO> GetUserInfo(HttpContext context, ClaimsPrincipal claimsPrincipal, [Service] GetUsersQueryService getUsersQueryService)
        {
            UserDTO users = new UserDTO();
            try
            {
                
                var nameIdentifier = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
              
                UserDTO userDTO = new UserDTO();
                userDTO.UserUID = new Guid(nameIdentifier);
                // Call the mediator to send the query
                users = await getUsersQueryService.GetUsersByUserUIDQueryAsync(userDTO);
            }
            catch (GraphQLException ex)
            {
                var error = new Error(ex.Message, "500");
                throw new GraphQLException(error);
            }
            catch (System.Exception ex)
            {
                var error = new Error(ex.Message, "500");
                throw new GraphQLException(error);
            }
            return users;
        }



        [GraphQLName("getUsers")]
        [UseOffsetPaging]
        [UseFiltering]
        [UseSorting]
        [Authorize]
        public async Task<List<UserDTO>> GetUsers(HttpContext context, ClaimsPrincipal claimsPrincipal, [Service] GetUsersQueryService getUsersQueryService)
        {
            List <UserDTO> users = new List<UserDTO>();
            try
            {
              
                // Call the mediator to send the query
                users = await getUsersQueryService.GetUsersQueryAsync();
            }
            catch (GraphQLException ex)
            {
                var error = new Error(ex.Message, "500");
                throw new GraphQLException(error);
            }
            catch (System.Exception ex)
            {
                var error = new Error(ex.Message, "500");
                throw new GraphQLException(error);
            }
            return users;
        }
    }
}
