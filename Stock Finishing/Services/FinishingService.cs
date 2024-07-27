using Stock_Finishing.Models;
using Stock_Finishing.RepositoryAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Finishing.Services
{
    public interface IFinishingService
    {
        Task<ResultModel<RuloMigrationModel>> GetRuloMigration(int ruloMigrationID);
        Task<ResultModel<decimal>> SubtractMetersToTheStyle(SubtractMetersModel request);
        Task<ResultModel<InventoryMetersModel>> GetInventoryMeters();
        Task<ResultModel<InventoryMetersModel>> GetInventoryMetersByStyle(string style);
    }

    public class FinishingService : IFinishingService
    {
        HttpClient httpClient;
        private IRepositoryApi repositoryApi;

        public FinishingService()
        {
            httpClient = new HttpClient() { BaseAddress = new Uri(App.RestUrlFinishing) };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.UserModel.Token);

            repositoryApi = new RepositoryApi(httpClient);
        }

        public async Task<ResultModel<RuloMigrationModel>> GetRuloMigration(int ruloMigrationID)
        {
            ResultModel<RuloMigrationModel> resultModel = new ResultModel<RuloMigrationModel>();
            var responseApi = await repositoryApi.Get<RuloMigrationModel>($"Finishing/GetRuloMigration?ruloMigrationID={ruloMigrationID}");

            await ValidateResponse(resultModel, responseApi);

            return resultModel;
        }

        public async Task<ResultModel<decimal>> SubtractMetersToTheStyle(SubtractMetersModel request)
        {
            ResultModel<decimal> resultModel = new ResultModel<decimal>();
            var responseApi = await repositoryApi.Post<decimal, SubtractMetersModel>($"Finishing/SubtractMetersToTheStyle", request);

            await ValidateResponse(resultModel, responseApi);

            return resultModel;
        }

        public async Task<ResultModel<InventoryMetersModel>> GetInventoryMeters()
        {
            ResultModel<InventoryMetersModel> resultModel = new ResultModel<InventoryMetersModel>();
            var responseApi = await repositoryApi.Get<InventoryMetersModel>($"Finishing/GetInventoryMeters");

            await ValidateResponse(resultModel, responseApi);

            return resultModel;
        }

        public async Task<ResultModel<InventoryMetersModel>> GetInventoryMetersByStyle(string style)
        {
            ResultModel<InventoryMetersModel> resultModel = new ResultModel<InventoryMetersModel>();
            var responseApi = await repositoryApi.Get<InventoryMetersModel>($"Finishing/GetInventoryMetersByStyle?style={style}");

            await ValidateResponse(resultModel, responseApi);

            return resultModel;
        }

        private async Task ValidateResponse<T>(ResultModel<T> resultModel, HttpResponseResult<T> responseApi)
        {
            if (responseApi.Error)
            {
                resultModel.IsSuccess = !responseApi.Error;

                switch (responseApi.HttpResponseMessage.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        resultModel.Message = "El rollo ya fue escaneado o el estilo no existe en inventario";
                        break;
                    case HttpStatusCode.NotFound:
                        resultModel.Message = "No se encontraron registros";
                        break;
                    default:
                        resultModel.Message = await responseApi.ErrorMessage();
                        break;
                }
            }
            else
            {
                resultModel.Data = responseApi.Response;
            }
        }
    }
}
