using Stock_Finishing.Models;
using Stock_Finishing.RepositoryAPI;
using System.Net;

namespace Stock_Finishing.Services
{
    public interface IFinishingService
    {
        Task<ResultModel<RuloMigrationModel>> GetRuloMigration(int ruloMigrationID);
        Task<ResultModel<RuloModel>> GetRulo(int ruloID);
        Task<ResultModel<ReprocessModel>> GetReprocess(int reprocessID);
        Task<ResultModel<decimal>> SubtractMetersInRawStyle(SubtractMetersModel request);
        Task<ResultModel<decimal>> SubtractMetersInProductionStyle(SubtractMetersModel request);
        Task<ResultModel<decimal>> SubtractMetersInReprocessStyle(SubtractMetersModel request);
        Task<ResultModel<InventoryMetersModel>> GetInventoryMeters(int type);
        Task<ResultModel<InventoryMetersModel>> GetInventoryMetersByParam(int type, string param);
        Task<ResultModel<List<StyleModel>>> GetListStyleData();
        Task<ResultModel<List<OriginModel>>> GetListOriginData(int type);
        Task<ResultModel<List<TabModel>>> GetTabs();
        Task<ResultModel<FabricInformationDTO>> GetFabricInformationForStock(int registerID);
        Task<ResultModel<decimal>> SubtractMetersByType(string type, SubtractMetersModel request);
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
            ResultModel<RuloMigrationModel> resultModel = new();
            var responseApi = await repositoryApi.Get<RuloMigrationModel>($"Finishing/GetRuloMigration?ruloMigrationID={ruloMigrationID}");

            await ValidateResponse(resultModel, responseApi);

            return resultModel;
        }

        public async Task<ResultModel<RuloModel>> GetRulo(int ruloID)
        {
            ResultModel<RuloModel> resultModel = new();
            var responseApi = await repositoryApi.Get<RuloModel>($"Finishing/GetRuloForStockApp?ruloID={ruloID}");

            await ValidateResponse(resultModel, responseApi);

            return resultModel;
        }

        public async Task<ResultModel<ReprocessModel>> GetReprocess(int reprocessID)
        {
            ResultModel<ReprocessModel> resultModel = new();
            var responseApi = await repositoryApi.Get<ReprocessModel>($"Finishing/GetReprocess?reprocessID={reprocessID}");

            await ValidateResponse(resultModel, responseApi);

            return resultModel;
        }

        public async Task<ResultModel<decimal>> SubtractMetersInRawStyle(SubtractMetersModel request)
        {
            ResultModel<decimal> resultModel = new();
            var responseApi = await repositoryApi.Post<decimal, SubtractMetersModel>($"Finishing/SubtractMetersInRawStyle", request);

            await ValidateResponse(resultModel, responseApi);

            return resultModel;
        }

        public async Task<ResultModel<decimal>> SubtractMetersInProductionStyle(SubtractMetersModel request)
        {
            ResultModel<decimal> resultModel = new();
            var responseApi = await repositoryApi.Post<decimal, SubtractMetersModel>($"Finishing/SubtractMetersInProductionStyle", request);

            await ValidateResponse(resultModel, responseApi);

            return resultModel;
        }

        public async Task<ResultModel<decimal>> SubtractMetersInReprocessStyle(SubtractMetersModel request)
        {
            ResultModel<decimal> resultModel = new();
            var responseApi = await repositoryApi.Post<decimal, SubtractMetersModel>($"Finishing/SubtractMetersInReprocessStyle", request);

            await ValidateResponse(resultModel, responseApi);

            return resultModel;
        }

        public async Task<ResultModel<InventoryMetersModel>> GetInventoryMeters(int type)
        {
            ResultModel<InventoryMetersModel> resultModel = new();
            var responseApi = await repositoryApi.Get<InventoryMetersModel>($"Finishing/GetInventoryMeters?type={type}");

            await ValidateResponse(resultModel, responseApi);

            return resultModel;
        }

        public async Task<ResultModel<InventoryMetersModel>> GetInventoryMetersByParam(int type, string param)
        {
            ResultModel<InventoryMetersModel> resultModel = new();
            var responseApi = await repositoryApi.Get<InventoryMetersModel>($"Finishing/GetInventoryMetersByParam?type={type}&param={param}");

            await ValidateResponse(resultModel, responseApi);

            return resultModel;
        }

        public async Task<ResultModel<List<StyleModel>>> GetListStyleData()
        {
            ResultModel<List<StyleModel>> resultModel = new();
            var responseApi = await repositoryApi.Get<List<StyleModel>>($"Finishing/GetListStyleData");

            await ValidateResponse(resultModel, responseApi);

            return resultModel;
        }
        public async Task<ResultModel<List<OriginModel>>> GetListOriginData(int type)
        {
            ResultModel<List<OriginModel>> resultModel = new();
            var responseApi = await repositoryApi.Get<List<OriginModel>>($"Finishing/GetListOriginData?type={type}");

            await ValidateResponse(resultModel, responseApi);

            return resultModel;
        }

        public async Task<ResultModel<List<TabModel>>> GetTabs()
        {
            ResultModel<List<TabModel>> resultModel = new();
            var responseApi = await repositoryApi.Get<List<TabModel>>($"Finishing/GetTabsForStockApp");

            await ValidateResponse(resultModel, responseApi);

            return resultModel;
        }

        public async Task<ResultModel<FabricInformationDTO>> GetFabricInformationForStock(int registerID)
        {
            ResultModel<FabricInformationDTO> resultModel = new();
            var responseApi = await repositoryApi.Get<FabricInformationDTO>($"Finishing/GetFabricInformationForStock?registerID={registerID}");

            await ValidateResponse(resultModel, responseApi);

            return resultModel;
        }

        public async Task<ResultModel<decimal>> SubtractMetersByType(string type, SubtractMetersModel request)
        {
            ResultModel<decimal> resultModel = new();
            var responseApi = await repositoryApi.Post<decimal, SubtractMetersModel>($"Finishing/SubtractMetersByType?type={type}", request);

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
