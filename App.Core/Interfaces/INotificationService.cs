using App.Dtos.ResquestModel;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface INotificationService
    {
        Task SendEmail(EmailDto emailDto);
    }
}
