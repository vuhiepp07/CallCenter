using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Models
{
    public class PagingHelper<T>
    {
        public int _totalItems { get; set; }
        public int _currentPage { get; set; }
        public int _totalPages { get; set; }
        public int _rowsPerPage { get; set; }

        IList<T> values = new List<T>();
        List<T> ValuesListToView = new List<T> { };
        List<T> SelectedValues = new List<T> { };

        public PagingHelper(IList<T> arr)
        {
            values = arr;
            _rowsPerPage = 10;
        }

        public List<T> nextPage()
        {
                _currentPage++;
                SelectedValues = ValuesListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
                return SelectedValues;
        }

        public List<T> prevPage()
        {
                _currentPage--;
                SelectedValues = ValuesListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
                return SelectedValues;
        }

        public List<T> refreshView()
        {
            ValuesListToView = (List<T>)values;
            _currentPage = 1;
            _totalItems = values.Count;
            _totalPages = _totalItems / _rowsPerPage + (_totalItems % _rowsPerPage == 0 ? 0 : 1);
            SelectedValues = ValuesListToView.Skip((_currentPage - 1) * _rowsPerPage).Take(_rowsPerPage).ToList();
            return SelectedValues;
        }
    }
}
