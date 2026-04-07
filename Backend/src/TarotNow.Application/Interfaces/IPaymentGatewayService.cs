

using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IPaymentGatewayService
{
        bool VerifyWebhookSignature(string payload, string signature);
}
