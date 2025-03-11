namespace rp_api.DTO
{
    public class SaveAllRequest
    {
        public List<CompleteRoleRequest> Roles { get; set; }
        public long LastSaved {  get; set; }
    }
}
