using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using MediatR;
using PetProject.Core.Database;
using PetProject.Core.Dtos;
using PetProject.Core.Dtos.Accounts;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Queries.GetUserInfo;

public class GetUserInfoHandler : IRequestHandler<GetUserInfoQuery, Result<UserDto, ErrorList>>
{
    private readonly IPostgresConnectionFactory _connectionFactory;

    public GetUserInfoHandler(
        IPostgresConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<UserDto, ErrorList>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var sqlQuery = """
                       SELECT
                       u.id,
                       u.user_name,
                       u.email,
                       u.name,
                       u.surname,
                       u.patronymic,
                       r.id as role_id,
                       r.name,
                       va.id as volunteer_account_id,
                       va.experience,
                       pa.id as participant_account_id,
                       va.requisites,
                       u.social_networks
                       FROM accounts.users u
                       LEFT JOIN accounts.role_user ru on u.id = ru.user_id
                       LEFT JOIN accounts.roles r ON r.id = ru.roles_id
                       LEFT JOIN accounts.volunteer_accounts va ON va.user_id = u.id
                       LEFT JOIN accounts.participant_accounts pa ON pa.user_id = u.id   
                       WHERE u.id = @UserId
                       """;

        var parameters = new DynamicParameters();

        parameters.Add("UserId", request.UserId);

        using var connection = _connectionFactory.GetConnection();

        var user =
            await connection
                .QueryAsync
                <UserDto, FullNameDto, RoleDto, VolunteerAccountDto?, ParticipantAccountDto?, string, string,
                    UserDto>(sqlQuery,
                    map: (user, fullName, role, volunteerAccount, participantAccount, jsonRequisites,
                        jsonSocialNetworks) =>
                    {
                        var socialNetworks = JsonSerializer
                                                 .Deserialize<SocialNetworkDto[]>(jsonSocialNetworks) ?? [];
                        user.SocialNetworks = socialNetworks;

                        if (volunteerAccount is not null)
                        {
                            volunteerAccount.Requisites = jsonRequisites != null
                                ? JsonSerializer.Deserialize<RequisiteDto[]>(jsonRequisites)
                                : [];

                            user.VolunteerAccount = volunteerAccount;
                        }
                        
                        if(participantAccount is not null)
                        {
                            user.ParticipantAccount = participantAccount;
                        }
                        
                        user.FullName = fullName;
                        user.Roles = user.Roles.Concat([role]).ToArray();
                        
                        return user;
                    },
                    splitOn:
                    "name, role_id, volunteer_account_id, participant_account_id, requisites, social_networks",
                    param: parameters
                );


        var result = user.SingleOrDefault();
        if(result is null)
            return Errors.General.NotFound().ToErrorList();

        return result;
    }
}