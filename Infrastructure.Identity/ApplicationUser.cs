using System;
using Microsoft.AspNetCore.Identity;

namespace RAGKnowledgeBase.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ApplicationUser()
    {
        Id = Guid.NewGuid();
    }
}
