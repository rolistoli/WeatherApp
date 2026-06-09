namespace WeatherApp.Helpers;

public static class WeatherCodeDescriptions
{
    public static string GetDescription(int weatherCode)
    {
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
