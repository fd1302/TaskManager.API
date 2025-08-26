using TaskManager.DataAccess.Repository;
using TaskManager.Logic.Mapping;
using TaskManager.Logic.Services;
using TaskManager.Shared.Dto_s;
using TaskManager.Shared.Dto_s.Authentication;
using TaskManager.Shared.Dto_s.Member;

namespace TaskManager.Logic.AppManager;

public class MemberManager
{
    private readonly MemberRepository _memberRepository;
    private readonly MemberMapping _memberMapping;
    private readonly PasswordHashing _passHasher;
    private readonly EmailVerification _emailVerification;
    private readonly AuthService _authService;
    public MemberManager(MemberRepository memberRepository, MemberMapping memberMapping, PasswordHashing passHasher,
        EmailVerification emailVerification, AuthService authService)
    {
        _memberRepository = memberRepository ??
            throw new ArgumentNullException(nameof(memberRepository));
        _memberMapping = memberMapping ??
            throw new ArgumentNullException(nameof(memberMapping));
        _passHasher = passHasher ??
            throw new ArgumentNullException(nameof(passHasher));
        _emailVerification = emailVerification ??
            throw new ArgumentNullException(nameof(emailVerification));
        _authService = authService ??
            throw new ArgumentNullException(nameof(authService));
    }
    public async Task<AddEntityResultDto> AddAsync(AddMemberDto addMemberDto)
    {
        if (addMemberDto.UserName.Length < 5 || addMemberDto.UserName.Length > 100)
        {
            throw new ArgumentException("Username must be between 5-100 characters.");
        }
        else if (string.IsNullOrEmpty(addMemberDto.UserName) || addMemberDto.UserName.Contains(' '))
        {
            throw new ArgumentException("Username can't be empty or contain white space.");
        }
        else if (addMemberDto.Email.Length < 10 || addMemberDto.Email.Length > 256)
        {
            throw new ArgumentException("Email must be between 20-256 characters.");
        }
        else if (string.IsNullOrEmpty(addMemberDto.Email) || addMemberDto.Email.Contains(' '))
        {
            throw new ArgumentException("Email can't be empty or contant white space.");
        }
        else if (addMemberDto.Password.Length < 5 || addMemberDto.Password.Length > 100)
        {
            throw new ArgumentException("Password must be between 10-100 characters.");
        }
        else if (string.IsNullOrEmpty(addMemberDto.Password) || addMemberDto.Password.Contains(' '))
        {
            throw new ArgumentException("Password can't be empty or contan white space.");
        }
        bool userNameExists = await _memberRepository.MemberExistsAsync(addMemberDto.UserName, null);
        if (userNameExists)
        {
            throw new ArgumentException("Username already exists.");
        }
        bool checkEmail = await _emailVerification.Verify(addMemberDto.Email);
        if (!checkEmail)
        {
            throw new ArgumentException("Email is not valid.");
        }
        var member = _memberMapping.AddMemberDtoToMember(addMemberDto);
        member.Id = Guid.NewGuid();
        member.JoinedAt = DateTime.UtcNow;
        member.Password = _passHasher.Sha256HashPass(addMemberDto.Password);
        var result = await _memberRepository.AddAsync(member);
        if (result == 0)
        {
            return new AddEntityResultDto();
        }
        var auth = new AuthenticationDto()
        {
            UserName = member.UserName,
            Password = addMemberDto.Password
        };
        var authResult = await _authService.MemberAuthenticationAsync(auth);
        Console.WriteLine($"{authResult.ErrorMessage}");
        return new AddEntityResultDto()
        {
            Id = member.Id,
            IsAdded = true,
            Token = authResult.Token
        };
    }
    public async Task<IEnumerable<MemberDto>?> GetMembersAsync(string? searchQuery)
    {
        var memebers = await _memberRepository.GetMembersAsync(searchQuery);
        if (memebers == null)
        {
            return null;
        }
        var mappedMembers = memebers.Select(_memberMapping.MemberToMemberDto);
        return mappedMembers;
    }
    public async Task<MemberDto?> GetMemberAsync(Guid? id, string? userName)
    {
        if (id != null)
        {
            var memberWithId = await _memberRepository.GetMemberAsync(id, null);
            if (memberWithId == null)
            {
                return null;
            }
            var mappedMemberA = _memberMapping.MemberToMemberDto(memberWithId);
            return mappedMemberA;
        }
        else if (userName != null)
        {
            var memberWithUserName = await _memberRepository.GetMemberAsync(null, userName);
            if (memberWithUserName == null)
            {
                return null;
            }
            var mappedMemberB = _memberMapping.MemberToMemberDto(memberWithUserName);
            return mappedMemberB;
        }
        return null;
    }
    public async Task<ResultDto> UpdateAsync(UpdateMemberDto updateMemberDto, Guid id)
    {
        bool memberExists = await _memberRepository.MemberExistsAsync(null, id);
        if (!memberExists)
        {
            return new ResultDto()
            {
                Operation = "Update",
                Message = "Wrong id or manager doesn't exist."
            };
        }
        else if (!string.IsNullOrEmpty(updateMemberDto.UserName))
        {
            if (updateMemberDto.UserName.Length < 5 || updateMemberDto.UserName.Length > 100)
            {
                throw new ArgumentException("Username must be between 5-100 characters.");
            }
        }
        else if (!string.IsNullOrEmpty(updateMemberDto.Email))
        {
            if (updateMemberDto.Email.Length < 20 || updateMemberDto.Email.Length > 100)
            {
                throw new ArgumentException("Email must be between 20-256 characters.");
            }
        }
        var member = _memberMapping.UpdateMemberDtoTOMember(updateMemberDto);
        bool result = await _memberRepository.UpdateAsync(member, id);
        if (!result)
        {
            return new ResultDto()
            {
                Operation = "Update",
                Message = "There was an error while handling the request."

            };
        }
        return new ResultDto()
        {
            Success = true,
            Operation = "Update",
            Message = "Updated successfuly."
        };
    }
    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        bool memberExists = await _memberRepository.MemberExistsAsync(null, id);
        if (!memberExists)
        {
            return new ResultDto()
            {
                Operation = "Delete",
                Message = "Member not found, Wrong id or doesn't exist."
            };
        }
        var result = await _memberRepository.DeleteAsync(id);
        return new ResultDto()
        {
            Success = true,
            Operation = "Delete",
            Message = "Member deleted successfuly."
        };
    }
}
