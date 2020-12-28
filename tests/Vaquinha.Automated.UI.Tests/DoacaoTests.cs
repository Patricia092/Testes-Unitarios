using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Vaquinha.Tests.Common.Fixtures;
using Xunit;

namespace Vaquinha.AutomatedUITests
{
    public class DoacaoTests : IDisposable, IClassFixture<DoacaoFixture>,
                                               IClassFixture<EnderecoFixture>,
                                               IClassFixture<CartaoCreditoFixture>
    {
        private DriverFactory _driverFactory = new DriverFactory();
        private IWebDriver _driver;

        private readonly DoacaoFixture _doacaoFixture;
        private readonly EnderecoFixture _enderecoFixture;
        private readonly CartaoCreditoFixture _cartaoCreditoFixture;

        public DoacaoTests(DoacaoFixture doacaoFixture, EnderecoFixture enderecoFixture, CartaoCreditoFixture cartaoCreditoFixture)
        {
            _doacaoFixture = doacaoFixture;
            _enderecoFixture = enderecoFixture;
            _cartaoCreditoFixture = cartaoCreditoFixture;
        }
        public void Dispose()
        {
            _driverFactory.Close();
        }

        [Fact]
        public void DoacaoUI_AcessoTelaHome()
        {
            // Arrange
            _driverFactory.NavigateToUrl("https://vaquinha.azurewebsites.net/");
            _driver = _driverFactory.GetWebDriver();

            // Act
            IWebElement webElement = null;
            webElement = _driver.FindElement(By.ClassName("vaquinha-logo"));

            // Assert
            webElement.Displayed.Should().BeTrue(because: "logo exibido");
        }
         
        [Fact]
        public void DoacaoUI_CriacaoDoacao()
        {
            //Arrange
            var doacao = _doacaoFixture.DoacaoValida();
            doacao.AdicionarEnderecoCobranca(_enderecoFixture.EnderecoValido());
            doacao.AdicionarFormaPagamento(_cartaoCreditoFixture.CartaoCreditoValido());
            _driverFactory.NavigateToUrl("https://vaquinha.azurewebsites.net/");
            _driver = _driverFactory.GetWebDriver();

            //Act
            IWebElement webElement = null;
            webElement = _driver.FindElement(By.ClassName("btn-yellow"));
            webElement.Click();

            IWebElement valor = _driver.FindElement(By.Id("valor"));
            valor.SendKeys(doacao.Valor.ToString());

            IWebElement campoNome = _driver.FindElement(By.Id("DadosPessoais_Nome"));
            campoNome.SendKeys(doacao.DadosPessoais.Nome);

            IWebElement campoEmail = _driver.FindElement(By.Id("DadosPessoais_Email"));
            campoEmail.SendKeys(doacao.DadosPessoais.Email);

            IWebElement campoMensagemApoio = _driver.FindElement(By.Id("DadosPessoais_MensagemApoio"));
            campoMensagemApoio.SendKeys(doacao.DadosPessoais.MensagemApoio);

            IWebElement campoEnderecoCobranca = _driver.FindElement(By.Id("EnderecoCobranca_TextoEndereco"));
            campoEnderecoCobranca.SendKeys(doacao.EnderecoCobranca.TextoEndereco);

            IWebElement campoEnderecoCobrancaNumero = _driver.FindElement(By.Id("EnderecoCobranca_Numero"));
            campoEnderecoCobrancaNumero.SendKeys(doacao.EnderecoCobranca.Numero);

            IWebElement campoEnderecoCobrancaCidade = _driver.FindElement(By.Id("EnderecoCobranca_Cidade"));
            campoEnderecoCobrancaCidade.SendKeys(doacao.EnderecoCobranca.Cidade);

            IWebElement campoEnderecoCobrancaEstado = _driver.FindElement(By.Id("estado"));
            campoEnderecoCobrancaEstado.SendKeys(doacao.EnderecoCobranca.Estado);

            IWebElement campoCep = _driver.FindElement(By.Id("cep"));
            campoCep.SendKeys(doacao.EnderecoCobranca.CEP);

            IWebElement campoComplemento = _driver.FindElement(By.Id("EnderecoCobranca_Complemento"));
            campoComplemento.SendKeys(doacao.EnderecoCobranca.Complemento);

            IWebElement campoTelefone = _driver.FindElement(By.Id("telefone"));
            campoTelefone.SendKeys(doacao.EnderecoCobranca.Telefone);

            IWebElement campoFormaPagamentoNomeTitular = _driver.FindElement(By.Id("FormaPagamento_NomeTitular"));
            campoFormaPagamentoNomeTitular.SendKeys(doacao.FormaPagamento.NomeTitular);

            IWebElement campoFormaPagamentoCartaoCredito = _driver.FindElement(By.Id("cardNumber"));
            campoFormaPagamentoCartaoCredito.SendKeys(doacao.FormaPagamento.NumeroCartaoCredito);

            IWebElement campoCartaoCreditoValidade = _driver.FindElement(By.Id("validade"));
            campoCartaoCreditoValidade.SendKeys(doacao.FormaPagamento.Validade);

            IWebElement campoCartaoCreditoCvv = _driver.FindElement(By.Id("cvv"));
            campoCartaoCreditoCvv.SendKeys(doacao.FormaPagamento.CVV);

            IWebElement doar = null;
            doar = _driver.FindElement(By.ClassName("btn-yellow"));
            doar.Click();

            //Assert
            _driver.Url.Should().NotContain("Doacoes/Create");
        }
    }
}