using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contas.LibClasses
{
    public class Carteira
    {
        public double Saldo
        {
            get;
            private set;
        }
        public double LimiteConta { get; set; }
        public string Dono { get; set; }
        public string NumeroDaConta = Guid.NewGuid().ToString();
        public string cpf { get; set; }
        private DateTime UltimaCobranca = DateTime.MinValue;

        public bool Sacar(double Valor)
        {
            if (Valor > this.Saldo)
                return false;

            if (Valor < 0)
                return false;

            if (Valor < this.LimiteConta)
                return false;

            if (DateTime.Now.Hour < 8 || DateTime.Now.Hour > 17)
                return false;

            this.Saldo -= Valor;
            return true;
        }

        public bool Depositar(double Valor)
        {
            if (Valor < 0)
                return false;

            if (DateTime.Now.Hour < 8 || DateTime.Now.Hour > 17)
                return false;

            this.Saldo += Valor;
            return true;
        }

        public bool Transferir(Carteira destino, double valor)
        {  
            //se nao tiver saldo cancela transferencia retornando false
            if (this.Saldo <= valor)
                return false;

            //Executa transferencia tirando da conta origram e deposinto na conta destino
            this.Sacar(valor);
            bool tOK = destino.Depositar(valor);
            if (tOK)// se transferencia ocorreu com sucesso retorna true
                return true;
            else// caso ocorrer erro faz o rollback voltando dinheiro para conta de origem
            {
                this.Depositar(valor);
                return false;
            }
        }
        public bool CobrarTarifa(double Valor)
        {
            if (DateTime.Now.Month != UltimaCobranca.Month)
            {
                if (Valor > this.Saldo)
                    return false;

                this.Saldo -= Valor;
                UltimaCobranca = DateTime.Now;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
