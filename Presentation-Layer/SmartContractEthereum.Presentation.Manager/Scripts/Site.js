$(document).ready(function () {

    var tds = $('.status-style');

    tds.each(function() {
        switch($(this).text().replace(/[^0-9]/g, '')) {
            case "01": //Proposta em simulação
                $(this).css('background-color', '#000');
                break;
            case "02": //A proposta foi encaminhada para análise pela Mesa de Crédito (temporário).
                $(this).css('background-color', '#46b946');
                break;
            case "03": //A proposta está na fila das propostas a analisar (temporário).
                $(this).css('background-color', '#798679');
                break;
            case "05": //A proposta está sendo analisada (temporário).
                $(this).css('background-color', '#6c936c');
                break;
            case "07": //A proposta foi reprovada.
                $(this).css('background-color', '#d92626');
                break;
            case "10": //Indica que há necessidade de fornecer informações adicionais. Para obter as pendências deve ser utilizada a API GET/propostas/{id}/pendências.
                $(this).css('background-color', '#cccc33');
                break;
            case "17": //A proposta foi reprovada pela mesa de crédito.
                $(this).css('background-color', '#bf4040');
                break;
            case "20": //CONTRATO EFETIVADO
                $(this).css('background-color', '#39c639');
                break;
            case "21": //Indica que a proposta foi aprovada.
                $(this).css('background-color', '#19e619');
                break;
            case "30": //A proposta foi encaminhada para checagem pela Mesa de Crédito (temporário).
                $(this).css('background-color', '#4db34d');
                break;
            case "31": //Proposta passou da etapa de pré-análise e pode complementar seus dados.
                $(this).css('background-color', '#59a659');
                break;
            case "40": //Indica que a proposta está pré-efetivada e que os documentos a serem assinados precisam ser recuperados.
                $(this).css('background-color', '#39c639');
                break;
            case "50": //A proposta ainda não iniciou o cálculo de ofertas (temporário).
                $(this).css('background-color', '#6c936c');
                break;
            case "51": //Estão sendo calculadas ofertas para a proposta (temporário).
                $(this).css('background-color', '#669966');
                break;
            case "52": //Foram calculadas ofertas: que já podem ser selecionadas.
                $(this).css('background-color', '#53ac53');
                break;
            case "53": //Uma oferta foi selecionada (temporário).
                $(this).css('background-color', '#53ac53');
                break;
            case "60": //Indica que o crédito foi negado. 
                $(this).css('background-color', '#bf4040');
                break;
            case "61": //PROPOSTA CANCELADA
                $(this).css('background-color', '#b94646');
                break;
            default:
                break;
        }
    });
});
