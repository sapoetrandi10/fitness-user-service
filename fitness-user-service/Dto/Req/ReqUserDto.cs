namespace fitness_user_service.Dto.Req
{
    public class ReqUserDto
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public float Weight { get; set; } = 0;
        public float Height { get; set; } = 0;
    }
}
