using EBSApi.Models;
using EBSApi.Services;
using FluentAssertions;

namespace EBS.Tests.Services.TariffsService
{
    public class CalculatePrognosisForYearTests
    {
        private readonly ITariffServices _tariffService;

        public CalculatePrognosisForYearTests()
        {
            _tariffService = new TariffServices();
        }

        public static readonly IEnumerable<Tariff> _tarrifs = new Tariff[]
        {
            new Tariff(0.228, 0.282, 0.187, 0.19),
            new Tariff(0.239, 0.31, 0.198, 0.199),
            new Tariff(0.241, 0.262, 0.189, 0.202),
            new Tariff(0.262, 0.251, 0.141, 0.194),
            new Tariff(0.281, 0.28, 0.154, 0.206),
            new Tariff(0.245, 0.254, 0.142, 0.197),
            new Tariff(0.267, 0.272, 0.175, 0.201),
            new Tariff(0.225, 0.266, 0.187, 0.192),
            new Tariff(0.219, 0.255, 0.192, 0.174),
            new Tariff(0.285, 0.271, 0.181, 0.187)
        };

        [Fact]
        public void CalculatePrognosis_ReturnsRatesForYear()
        {
            var result = _tariffService.CalculatePrognosisForYear(_tarrifs);

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(12);
        }
    }
}
