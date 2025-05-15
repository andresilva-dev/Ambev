using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class CpfValidator : AbstractValidator<string>
    {
        public CpfValidator()
        {
            RuleFor(cpf => cpf)
                .NotEmpty()
                .WithMessage("The CPF cannot be empty.")
                .Must(BeValidCpf)
                .WithMessage("The provided CPF is not valid.");
        }

        private bool BeValidCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            if (cpf.All(c => c == cpf[0]))
                return false;

            int[] weights1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] weights2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            int sum1 = 0;
            for (int i = 0; i < 9; i++)
            {
                sum1 += (cpf[i] - '0') * weights1[i];
            }
            int remainder1 = sum1 % 11;
            int digit1 = remainder1 < 2 ? 0 : 11 - remainder1;
            if (digit1 != cpf[9] - '0')
                return false;

            int sum2 = 0;
            for (int i = 0; i < 10; i++)
            {
                sum2 += (cpf[i] - '0') * weights2[i];
            }
            int remainder2 = sum2 % 11;
            int digit2 = remainder2 < 2 ? 0 : 11 - remainder2;
            return digit2 == cpf[10] - '0';
        }
    }
}
