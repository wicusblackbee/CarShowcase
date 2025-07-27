using CarShowcase.Models;

namespace CarShowcase.Services;

public interface ICarService
{
    Task<List<Car>> GetAllCarsAsync();
    Task<Car?> GetCarByIdAsync(int id);
    Task<List<Car>> SearchCarsAsync(string? make = null, string? model = null, int? minYear = null, int? maxYear = null, decimal? maxPrice = null);
    Task<bool> AddCarAsync(Car car);
    Task<bool> UpdateCarAsync(Car car);
    Task<bool> DeleteCarAsync(int id);
    Task<List<string>> GetMakesAsync();
    Task<List<string>> GetModelsAsync(string make);
}
