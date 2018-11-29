using System.Web;

namespace SmartContractEthereum.Presentation.Manager.Models.EthereumContract
{
    public class EthereumContractViewModel : Domain.Entities.EthereumContract
    {
        public string Recipient { get; set; }

        public string Requester { get; set; }

        public HttpPostedFileBase FileUpload { get; set; }
    }
}