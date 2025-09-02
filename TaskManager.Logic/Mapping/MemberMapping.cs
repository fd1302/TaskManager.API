using TaskManager.Shared.Dto_s.Member;
using TaskManager.Shared.Entities;

namespace TaskManager.Logic.Mapping;

public class MemberMapping
{
    public Member AddMemberDtoToMember(AddMemberDto addMemberDto)
    {
        return new Member()
        {
            UserName = addMemberDto.UserName,
            Email = addMemberDto.Email,
            Password = addMemberDto.Password,
        };
    }
    public MemberDto MemberToMemberDto(Member member)
    {
        return new MemberDto()
        {
            Id = member.Id,
            TenantId = member.TenantId,
            UserName = member.UserName,
            Email = member.Email,
            JoinedAt = member.JoinedAt,
            Role = member.Role
        };
    }
    public Member UpdateMemberDtoTOMember(UpdateMemberDto updateMemberDto)
    {
        return new Member()
        {
            UserName = updateMemberDto.UserName,
            Email = updateMemberDto.Email
        };
    }
    public Member ManagerTOMember(Manager manager)
    {
        return new Member()
        {
            Id = manager.Id,
            TenantId = manager.TenantId,
            UserName = manager.UserName,
            Password = manager.Password,
            Email = manager.Email,
            JoinedAt = manager.JoinedAt
        };
    }
}
