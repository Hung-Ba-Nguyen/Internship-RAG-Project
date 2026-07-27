# RAG Knowledge Base

## Cấu trúc dự án (Clean Architecture)
- `Core.Domain`: Chứa Entities (User, Role) và BuildingBlocks.
- `Core.Application`: Xử lý business logic.
- `Infrastructure.Identity`: Chứa IdentityDbContext.cs.
- `Presentation.API`: Điểm vào của ứng dụng.