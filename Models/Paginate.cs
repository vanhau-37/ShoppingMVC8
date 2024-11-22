namespace Shopping_mvc8.Models
{
    public class Paginate
    {
        public int TotelItems { get; set; }// tong so item
        public int PageSize { get; set; }//tong so item tren 1 trang
        public int CurrentPage { get; set; }//trang hien tai
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public Paginate() { }
        public Paginate(int totelItems, int page, int pageSize = 10) 
        {
            int totalPages = (int)Math.Ceiling((decimal)totelItems/(decimal)pageSize);
            int currentPage = page;
            int startPage = currentPage - 5;
            int endPage = currentPage + 4;
            if(startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if(endPage >= totalPages)
            {
                endPage = totalPages;
                if(endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }
            TotelItems = totelItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;

        }

    }
}
