using Dapper;
using EBSApi.Utility;
using EBSApi.Models;
using System.Data;
using EBSApi.Models.Dtos;

namespace EBSApi.Data
{
    public class AddressQueries : IAddressQueries
    {
        private readonly SqlUtility _utility;

        public AddressQueries(SqlUtility utility)
        {
            _utility = utility;
        }

        public async Task<Response<IEnumerable<Address>>> GetAllAddressesAsync()
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<IEnumerable<Address>> response = new Response<IEnumerable<Address>>();

            parameters.Add(
                "@return_value", 
                dbType: DbType.Int32, 
                direction: ParameterDirection.ReturnValue);

            using (IDbConnection connection = _utility.CreateConnection())
            {
                IEnumerable<Address> addresses = (await connection
                    .QueryAsync<Address>(
                        "dbo.getAllAddresses", 
                        parameters, 
                        commandType: CommandType.StoredProcedure))
                    .ToList();

                int returnValue = parameters.Get<int>("@return_value");

                if (returnValue != 0)
                {
                    response.Error = new Error
                    {
                        ErrorMessage = $"SQL exception occured with the return value of {returnValue}",
                        StatusCode = 400
                    };
                    return response;
                }

                response.Data = addresses;
                response.IsSuccess = true;
                return response;
            }
        }

        public async Task<Response<IEnumerable<Address>>> GetAddressesUserAsync(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<IEnumerable<Address>> response = new Response<IEnumerable<Address>>();

            parameters.Add(
                "@return_value", 
                dbType: DbType.Int32, 
                direction: ParameterDirection.ReturnValue);
            parameters.Add(
                "@userId", userId, 
                dbType: DbType.Int32, 
                direction: ParameterDirection.Input);

            using (IDbConnection connection = _utility.CreateConnection())
            {

                IEnumerable<Address> addresses = (await connection
                    .QueryAsync<Address>(
                        "dbo.getAddressesUser",
                        parameters,
                        commandType: CommandType.StoredProcedure))
                    .ToList();

                int returnValue = parameters.Get<int>("@return_value");
                
                if (returnValue != 0)
                {
                    response.Error = new Error
                    {
                        ErrorMessage = $"SQL exception occured with the return value of {returnValue}",
                        StatusCode = 400
                    };
                    return response;
                }

                response.Data = addresses;
                response.IsSuccess = true;
                return response;
            }
        }

        public async Task<Response<IEnumerable<Address>>> GetAddressesProviderAsync(int providerId)
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<IEnumerable<Address>> response = new Response<IEnumerable<Address>>();

            parameters.Add
                ("@return_value", 
                dbType: DbType.Int32, 
                direction: ParameterDirection.ReturnValue);
            parameters.Add(
                "@providerId", providerId, 
                dbType: DbType.Int32, 
                direction: ParameterDirection.Input);

            using (IDbConnection connection = _utility.CreateConnection())
            {

                IEnumerable<Address> addresses = (await connection
                    .QueryAsync<Address>(
                        "dbo.getAddressesProvider",
                        parameters,
                        commandType: CommandType.StoredProcedure))
                    .ToList();

                int returnValue = parameters.Get<int>("@return_value");

                if (returnValue != 0)
                {
                    response.Error = new Error
                    {
                        ErrorMessage = $"SQL exception occured with the return value of {returnValue}",
                        StatusCode = 400
                    };
                    return response;
                }

                response.Data = addresses;
                response.IsSuccess = true;
                return response;
            }
        }

        public async Task<Response<Address>> GetAddressAsync(int addressId)
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<Address> response = new Response<Address>();

            parameters.Add(
                "@return_value", 
                dbType: DbType.Int32, 
                direction: ParameterDirection.ReturnValue);
            parameters.Add(
                "@id", addressId, 
                dbType: DbType.Int32, 
                direction: ParameterDirection.Input);

            using (IDbConnection connection = _utility.CreateConnection())
            {
                Address address = (await connection
                    .QueryAsync<Address>(
                        "dbo.getAddress",
                        parameters,
                        commandType: CommandType.StoredProcedure))
                    .FirstOrDefault();

                int returnValue = parameters.Get<int>("@return_value");

                if (returnValue != 0)
                {
                    response.Error = new Error
                    {
                        ErrorMessage = $"SQL exception occured with the return value of {returnValue}",
                        StatusCode = 400
                    };
                    return response;
                }

                response.Data = address;
                response.IsSuccess = true;
                return response;
            }
        }

        public async Task<int> DeleteAddressAsync(int addressId)
        {
            /*
             * Komentaras: Nėra addressId validacijos. Gal vertėtų atsižvelgti ir į tokius atvejus kai įvestis pareina netvarkinga ir atlikti sanitarinį kintamųjų aptvarkymą ir validavimą.
             * 
             * Validuoti šio parametro nėra įmanoma, nebent būtų naudojama DB procedūra tikrinanti ar toks adresas jau egzistuoja.
             * Kadangi yra nustatytas int duomenų tipas, vartotojas ir taip negali paduoti neteisingo formato duomenų.
             * Validacija taip pat nereikalinga, nes naudojama procedūra, o ne tiesiog trynimo query, kuri bet kokiu atveju patikrina ar adresas egzistuoja.
             */

            DynamicParameters parameters = new();
            // Pašalintas nenaudojamas kintamasis

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);
            parameters.Add(
                "@id", addressId,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            using IDbConnection connection = _utility.CreateConnection();

            await connection
                .ExecuteAsync(
                    "dbo.deleteAddress",
                    parameters,
                    commandType: CommandType.StoredProcedure);

            /*
             * Try catch blokas čia nelabai turi prasmės, kadangi yra tik keletas galimų klaidų: 
             *  - Nėra execute teisių (programuotojo atsakomybė tai užtikrinti, kad produkcinėje aplinkoje to neatsitiktų) 
             *  - Neteisingi parametrai (taip būti negali, nes programuotojas nustato parametrų pavadinimus ir duomenų tipus  193 - 200 eilutėse)
             *  - Komandos timeout - tai gali atsitikti tik tokiu atveju, jei yra lėtas interneto ryšis arba yra komunikacijos problemos tarp DB ir API
             *  
             *  Bet kurios klaidos atveju, front-end dalis tinkamai atvaizduoja atitinkamą klaidą vartotojui. 
             *  Gauta klaida automatiškai yra log'inama į log failus, o try catch bloko atvejų, šią klaidą reikėtų papildomai log'inti.
             */

            int returnValue = parameters.Get<int>("@return_value");

            return returnValue;
        }

        public async Task<Response<Address>> CreateAddressAsync(Address address)
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<Address> response = new Response<Address>();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);

            parameters.Add(
                "@fullAddress", address.FullAddress,
                dbType: DbType.String,
                direction: ParameterDirection.Input);            
            
            parameters.Add(
                "@userId", address.UserId,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);            
            
            parameters.Add(
                "@providerId", address.ProviderId,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            using (IDbConnection connection = _utility.CreateConnection())
            {
                address.AddressId = (await connection
                    .QueryAsync<int>(
                        "dbo.createAddress",
                        parameters,
                        commandType: CommandType.StoredProcedure))
                    .FirstOrDefault();

                int returnValue = parameters.Get<int>("@return_value");

                if (returnValue != 0)
                {
                    response.Error = new Error
                    {
                        ErrorMessage = $"SQL exception occured with the return value of {returnValue}",
                        StatusCode = 400
                    };
                    return response;
                }

                response.Data = address;
                response.IsSuccess = true;
                return response;
            }
        }
    }
}
