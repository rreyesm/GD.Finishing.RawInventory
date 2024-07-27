using Stock_Finishing.Models;
using Stock_Finishing.RepositoryAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Finishing.Services
{
    public interface IUserService
    {
        Task<ResultModel<UserModel>> Authenticate(string userName, string password);
    }

    public class UserService : IUserService
    {
        HttpClient _httpClient;
        private IRepositoryApi _repositoryApi;

        public UserService()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(App.RestUrlLogin) };
            _repositoryApi = new RepositoryApi(_httpClient);
        }

        public async Task<ResultModel<UserModel>> Authenticate(string userName, string password)
        {
            ResultModel<UserModel> resultModel = new ResultModel<UserModel>();
            LoginInfo loginInfo = new LoginInfo();
            loginInfo.UserName = userName;
            loginInfo.Password = password;

            try
            {
                var responseApi = await _repositoryApi.Post<ResultModel<UserModel>, LoginInfo>(string.Empty, loginInfo);

                if (responseApi.Error)
                {
                    resultModel.IsSuccess = !responseApi.Error;
                    string error = await responseApi.ErrorMessage();
                    resultModel.Message = string.IsNullOrWhiteSpace(error) ? "unknown error" : error;
                }
                else
                {
                    resultModel = responseApi.Response;
                }

                return resultModel;

            }
            catch (Exception ex)
            {
                resultModel.Message = ex.Message;
                Debug.WriteLine($"Error {ex.ToString()}");
            }

            return resultModel;
        }
    }
}
