using System.Web;

namespace SmartContractEthereum.Presentation.Manager.Models.EthereumContract
{
    public class EthereumContractViewModel : Domain.Entities.EthereumContract
    {
        public HttpPostedFileBase File { get; set; }
    }
}