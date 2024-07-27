using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Finishing.Services
{
    public interface IMessageService
    {
        Task<string> ShowMessageAsync(string title, string message, string destruction, params string[] buttons);
        Task ShowAlertAsync(string message, string title = "Mensaje", string accept = "Aceptar");
        Task<bool> ShowConfirmationAsync(string title, string message);
        Task ShowErrorAsync(string message);

        Task<string> ShowPromptAsync(string message, string title = "Introduce Información");
    }
    public class MessageService : IMessageService
    {
        public Task ShowErrorAsync(string message)
        {
            return Application.Current.MainPage.DisplayAlert("Error", message, "Ok");
        }

        public Task ShowAlertAsync(string message, string title, string accept)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, accept);
        }

        public async Task<bool> ShowConfirmationAsync(string title, string message)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, "Ok", "Cancelar");
        }

        public async Task<string> ShowMessageAsync(string title, string message, string destruction, params string[] buttons)
        {
            return await Application.Current.MainPage.DisplayActionSheet(title, message, destruction, buttons);
        }

        public async Task<string> ShowPromptAsync(string message, string title = "Introduce un comentario")
        {
            return await Application.Current.MainPage.DisplayPromptAsync(title, message, "Ok");
        }
    }
}