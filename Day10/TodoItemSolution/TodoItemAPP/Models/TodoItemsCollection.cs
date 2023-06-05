using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
namespace TodoItemAPP.Models
{
    // RestApi를 쓰기 때문에 하는 일 우리가 직접 데이터를 처리하는게 아니라서
    public class TodoItemsCollection : ObservableCollection<TodoItem> // IList<TodoItem> , List<TodoItem> 차이점은 값이 변경되면 알아서 메시지를 날려줄 수 있는게 다른 점
    {
        public void CopyFrom(IEnumerable<TodoItem> todoItems)
        {
            this.Items.Clear(); // ObservableCollection<T> 자체가 Items 속성을 가지고 있다. 모든 데이터 삭제 초기화

            foreach(TodoItem item in todoItems)
            {
                this.Items.Add(item); // 하나씩 다시 추가
            }
            // 데이터가 바뀐 것을 알려준다. (전부 초기화)
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)); 
        }       
    }
}
