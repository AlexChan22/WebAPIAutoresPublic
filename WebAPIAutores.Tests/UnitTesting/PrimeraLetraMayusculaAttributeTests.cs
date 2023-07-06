using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebAPIAutores.Tests.UnitTesting
{
    [TestClass]
    public class PrimeraLetraMayusculaAttributeTests
    {
        [TestMethod]
        public void PrimeraLetraMinuscula_DevuelveError()
        {
            // Prepare test
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            string valor = "felipe";
            var valContext = new ValidationContext(new { Nombre = valor});

            // Execute test
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);


            // verify test result
            Assert.AreEqual("La primera letra debe ser mayuscula", resultado.ErrorMessage);
        }


        [TestMethod]
        public void ValorNulo_NoDevuelveError()
        {
            // Prepare test
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            string valor = null;
            var valContext = new ValidationContext(new { Nombre = valor });

            // Execute test
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);


            // verify test result
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void ValorConPrimeraLetraMayuscula_NoDevuelveError()
        {
            // Prepare test
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            string valor = "Felipe";
            var valContext = new ValidationContext(new { Nombre = valor });

            // Execute test
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);


            // verify test result
            Assert.IsNull(resultado);
        }
    }
}