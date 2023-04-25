using Graidex.Application.Infrastructure.ResultObjects.Generic;

namespace Graidex.Application.Tests.ResultObjects
{
    internal class GenericResultTests
    {
        [Test]
        public void Check_FailureResult_Test()
        {
            var result = CheckIfPositive(-1);

            Assert.IsTrue(result.IsFailure(out var failure));
            Assert.That(failure!.Justification, Is.EqualTo("N is negative or 0"));
        }

        [Test]
        public void Check_SuccessResult_Test()
        {
            var result = CheckIfPositive(10);

            Assert.IsTrue(result.IsSuccess(out var success));
            Assert.That(success!.Value, Is.EqualTo(10));
        }

        [Test]
        public void CheckPolymorphism_SuccessResult_DoesNotThrow()
        {
            var result = new ResultFactory<Animal>();
            Assert.DoesNotThrow(() => result.Success(new Cat()));
        }

        private Result<int> CheckIfPositive(int N)
        {
            var result = new ResultFactory<int>();

            if (N <= 0)
            {
                return result.Failure("N is negative or 0");
            }

            return result.Success(N);
        }

        private class Animal
        {

        }

        private class Cat : Animal
        {

        }
    }
}
