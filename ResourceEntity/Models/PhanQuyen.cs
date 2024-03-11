namespace ResourceEntity.Models;

public partial class PhanQuyen
{
	public int UserId { get; set; }

	public int RoleId { get; set; }

	public string? Note { get; set; }

	public virtual RoleDesc Role { get; set; } = null!;

	public virtual User User { get; set; } = null!;
}