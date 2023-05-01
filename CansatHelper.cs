using CansatGround.Models;
using CansatGround.Services;

namespace CansatGround;

public static class CansatHelper
{
    /// <summary>
    /// Добавление в DI контейнер зависимстей для Telegram
    /// </summary>
    /// <param name="services">Коллекция зависимостей для DI контейнера</param>
    /// <param name="configuration">Конфигурация </param>
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CansatGroundProperties>(x => configuration.GetValue<CansatGroundProperties>("CansatGroundProperties"));
    }
    
    /// <summary>
    /// Привести конфигурацию к классу типа <see cref="T"/>
    /// </summary>
    /// <param name="config">Конфигурация</param>
    /// <param name="section">Секция конфигурации</param>
    /// <typeparam name="T">Тип к которому нужно привести секцию конфигурации</typeparam>
    /// <returns>Тип к которому нужно привести секцию конфигурации</returns>
    public static T To<T>(this IConfiguration config, string section) where T : new()
    {
        var result = new T();
        config.GetSection("CansatGroundProperties").Bind(result);
        return result;
    }
}