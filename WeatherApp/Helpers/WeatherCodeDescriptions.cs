namespace WeatherApp.Helpers;

public static class WeatherCodeDescriptions
{
    public static string GetDescription(int weatherCode)
    {
        if (AppSettings.LanguageCode == "en")
        {
            return weatherCode switch
            {
                0 => "Clear sky",
                1 => "Mainly clear",
                2 => "Partly cloudy",
                3 => "Overcast",
                45 => "Fog",
                48 => "Depositing rime fog",
                51 => "Light drizzle",
                53 => "Moderate drizzle",
                55 => "Dense drizzle",
                56 => "Light freezing drizzle",
                57 => "Dense freezing drizzle",
                61 => "Slight rain",
                63 => "Moderate rain",
                65 => "Heavy rain",
                66 => "Light freezing rain",
                67 => "Heavy freezing rain",
                71 => "Slight snow",
                73 => "Moderate snow",
                75 => "Heavy snow",
                77 => "Snow grains",
                80 => "Slight rain showers",
                81 => "Moderate rain showers",
                82 => "Violent rain showers",
                85 => "Slight snow showers",
                86 => "Heavy snow showers",
                95 => "Thunderstorm",
                96 => "Thunderstorm with slight hail",
                99 => "Thunderstorm with heavy hail",
                _ => "Unknown condition"
            };
        }

        return weatherCode switch
        {
            0 => "Céu limpo",
            1 => "Maioritariamente limpo",
            2 => "Parcialmente nublado",
            3 => "Nublado",
            45 => "Nevoeiro",
            48 => "Nevoeiro com geada",
            51 => "Chuvisco fraco",
            53 => "Chuvisco moderado",
            55 => "Chuvisco forte",
            56 => "Chuvisco gelado fraco",
            57 => "Chuvisco gelado forte",
            61 => "Chuva fraca",
            63 => "Chuva moderada",
            65 => "Chuva forte",
            66 => "Chuva gelada fraca",
            67 => "Chuva gelada forte",
            71 => "Neve fraca",
            73 => "Neve moderada",
            75 => "Neve forte",
            77 => "Grãos de neve",
            80 => "Aguaceiros fracos",
            81 => "Aguaceiros moderados",
            82 => "Aguaceiros violentos",
            85 => "Aguaceiros de neve fracos",
            86 => "Aguaceiros de neve fortes",
            95 => "Trovoada",
            96 => "Trovoada com granizo fraco",
            99 => "Trovoada com granizo forte",
            _ => "Condição desconhecida"
        };
    }
}
