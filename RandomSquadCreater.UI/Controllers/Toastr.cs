namespace RandomSquadCreater.UI.Controllers
{
    public class Toastr
    {
        public string Message { get; set; }
        public string Title { get; set; }
        public ToastrType Type { get; set; }



        public Toastr()
        {
            
        }

        public Toastr(string message, string title="Bilgilendirme",ToastrType type=ToastrType.Info)
        {
            this.Message = message;
            this.Title = title;
            this.Type = type;

        }


    }

    public enum ToastrType
    {
        Info=0,
        Success=1,
        Warning=2,
        Error=3
    }
}