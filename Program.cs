using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Gestao_de_Alunos
{
    // Estrutura de dados para representar um Aluno
    // Autores : Gabriel / Rui / Douglas 
    public class Aluno
    {
        public string codAlu; // código do aluno (string)
        public string nomAlu; // nome do aluno (string)
        public int idaAlu; // idade do aluno (int)
        public float medAlu; // média de notas do aluno (float)
        public float proAlu; // propina do aluno (float)
        public float salAlu; // saldo do aluno (float)

        // Estrutura de dados para representar o estado e valor da divida mensal
        public struct EstadoMensal
        {
            public int mes; // Identificador do mês
            public bool estadoPagamento; // true - pago; false - não pago
            public float valorDivida; // Valor da divida mensal
        }

        public EstadoMensal[] mesesParaPagar; // Array de meses para Pagar

        public bool teveDividas; // aluno teve dívidas?

        // Construtor para inicializar um objeto Aluno
        public Aluno()
        {
            codAlu = "";
            nomAlu = "";
            idaAlu = 0;
            medAlu = 0.0f;
            proAlu = 0.0f;
            salAlu = 0.0f;
            mesesParaPagar = new EstadoMensal[12];
            teveDividas = false;

            // Inicializa todos os meses com estado pago e valor da dívida zero
            for (int i = 0; i < mesesParaPagar.Length; i++)
            {
                mesesParaPagar[i].mes = i + 1;
                mesesParaPagar[i].estadoPagamento = true;
                mesesParaPagar[i].valorDivida = proAlu;
            }
        }

        // Método para verificar se há uma dívida no próximo mês
        public bool DividaProxima(int proximoMes)
        {
            // Verifica se o próximo mês existe na lista de mesesParaPagar
            if (proximoMes < mesesParaPagar.Length)
            {   
                // Verifica se o estado de pagamento do próximo mês é falso (ou seja, há uma dívida)
                if (!mesesParaPagar[proximoMes].estadoPagamento)
                    return true; // Há uma dívida no próximo mês
            }
            return false; // Não há dívida no próximo mês
        }
    }

    public class OperacoesComAlunos
    {
        // Solicita informações do aluno e insere na estrutura Alunos
        public static void InserirAlunos(ref Aluno[] Alunos)
        {
            // Quantidade de alunos a inserir
            int quantidadeAlunos = Convert.ToInt32(OperacoesGerais.LerInteiroValido("Digite o número de alunos a inserir: "));

            int tamanhoAtual = Alunos.Length;
            int newArraySize = tamanhoAtual + quantidadeAlunos;

            // Resize do array para acomodar os novos alunos
            Array.Resize(ref Alunos, newArraySize);

            // Ciclo para introdução dos dados dos alunos
            for (int i = tamanhoAtual; i < newArraySize; i++)
            {
                Alunos[i] = new Aluno(); // Inicialização do objeto Aluno

                bool codigoValido = false;
                string codigoAluno;

                do
                {
                    codigoAluno = OperacoesGerais.LerStringValida("\nInsere o código do aluno: ");
                    codigoValido = !OperacoesGerais.VerificarCodigo(Alunos, codigoAluno); // Verifica se o código do aluno já existe ou não

                    if (!codigoValido)
                        Console.WriteLine("O código do aluno já existe.");
                } while (!codigoValido);

                Alunos[i].codAlu = codigoAluno;
                Alunos[i].nomAlu = OperacoesGerais.LerStringValida("Insere o nome: ");
                Alunos[i].idaAlu = Convert.ToInt32(OperacoesGerais.LerInteiroValido("Insere a idade: ", 0, 100));
                Alunos[i].medAlu = Convert.ToSingle(OperacoesGerais.LerDecimalValido("Insere a média: ", 0, 20));
                Alunos[i].proAlu = Convert.ToSingle(OperacoesGerais.LerDecimalValido("Insere a propina do aluno: "));
                Alunos[i].salAlu = Convert.ToSingle(OperacoesGerais.LerDecimalValido("Insere o saldo: "));

                // Se o aluno tem um saldo negativo, teveDividas = true
                if (Alunos[i].salAlu < 0)
                    Alunos[i].teveDividas = true;

                // Inicialize o array de dívidas para cada aluno
                for (int j = 0; j < Alunos[i].mesesParaPagar.Length; j++)
                {
                    bool entradaValida; // Flag que verifica se a opção introduzida é valida
                    string entrada; // Declara a variável de entrada fora do loop

                    // Ciclo para definir o estado das dívidas
                    do
                    {
                        entrada = OperacoesGerais.LerStringValida($"O aluno pagou o mês {Alunos[i].mesesParaPagar[j].mes}? (S/N) - ").ToLower(); // Lê a entrada e converte para minúsculas

                        switch (entrada)
                        {
                            case "s":
                                Alunos[i].mesesParaPagar[j].estadoPagamento = true;
                                entradaValida = true; // Define a flag como true para sair do loop
                                break;

                            case "n":
                                Alunos[i].mesesParaPagar[j].estadoPagamento = false;

                                // Se o aluno não pagou o mês anterior ao atual
                                if (Alunos[i].mesesParaPagar[j].mes < DateTime.Now.Month)
                                    Alunos[i].teveDividas = true;

                                entradaValida = true; // Define a flag como true para sair do loop
                                break;

                            default:
                                Console.WriteLine("Entrada inválida. Por favor, insira 'S' ou 'N'.");
                                entradaValida = false; // Define a flag como false para continuar do loop
                                break;
                        }
                    } while (!entradaValida);
                }
            }
        }

        // Mostra informações de todos os alunos na estrutura Alunos
        public static void MostrarAlunos(Aluno[] Alunos, int flag)
        {
            for (int i = 0; i < Alunos.Length; i++)
                MostrarInformacaoAluno(Alunos[i], ref flag);
        }

        // Mostra informações detalhadas de um aluno
        public static void MostrarInformacaoAluno(Aluno reg, ref int flag)
        {
            // flag = 0 (consulta de baixo nivel no aluno)
            if (flag == 0)
            {
                Console.WriteLine(
                $"Cód: {reg.codAlu} " +
                $"Nome: {reg.nomAlu} " +
                $"Idade: {reg.idaAlu} anos " +
                $"Média de Notas: {reg.medAlu} valores"
                );
            }
            else if (flag == 1)
            {
                // flag = 1 (consulta de alto nível no aluno)
                Console.WriteLine(
                    $"Cód: {reg.codAlu} " +
                    $"Nome: {reg.nomAlu} " +
                    $"Idade: {reg.idaAlu} anos " +
                    $"Média de Notas: {reg.medAlu} valores " +
                    $"Propina: {reg.proAlu} euro(s) " +
                    $"Saldo: {reg.salAlu} euro(s) "
                    );

                Console.WriteLine("Propinas - Mês:");

                for (int i = 0; i < reg.mesesParaPagar.Length; i++)
                {
                    // Representação visual do estado do Pagamento
                    string estadoDoPagamento;

                    // Se o valor presente no estadoPagamento for TRUE
                    estadoDoPagamento = (reg.mesesParaPagar[i].estadoPagamento) ? "Pago" : "Não Pago";

                    // Apenas para dizer qual é o mês atual
                    if (i + 1 == DateTime.Now.Month)
                        Console.WriteLine($"Mês {reg.mesesParaPagar[i].mes} (mês atual): {estadoDoPagamento}");
                    else
                        Console.WriteLine($"Mês {reg.mesesParaPagar[i].mes}: {estadoDoPagamento}");
                }
            }
            else
            {
                Console.WriteLine("Valor da Flag inválida.");
                return;
            }
        }

        // Consulta informações de um aluno específico
        public static void ConsultarAluno(Aluno[] Alunos)
        {
            string codigoAluno;

            MostrarAlunos(Alunos, 0); // Mostra a lista de alunos disponíveis

            codigoAluno = OperacoesGerais.LerStringValida("Qual o código do aluno que quer consultar? - ");

            bool encontrouAluno = false;
            int flag = 1; // Define a flag para mostrar a informação detalhada do aluno

            // Itera sobre todos os alunos
            foreach (var aluno in Alunos)
            {
                // Verifica se o aluno atual corresponde ao código fornecido pelo utilizador
                if (aluno.codAlu == codigoAluno)
                {
                    MostrarInformacaoAluno(aluno, ref flag); // Mostra as informações detalhadas do aluno
                    encontrouAluno = true;
                    break;
                }
            }

            // Se o aluno não foi encontrado, informa o utilizador
            if (!encontrouAluno)
                Console.WriteLine($"O aluno com o código {codigoAluno} não foi encontrado.");
        }

        // Altera dados de um aluno específico
        public static void AlterarDadosAluno(Aluno[] Alunos)
        {
            string codigoAluno;
            bool encontrouAluno = false;

            // Mostra os alunos para que o utilizador possa escolher qual alterar
            MostrarAlunos(Alunos, 0);

            codigoAluno = OperacoesGerais.LerStringValida("Qual o código do aluno que quer alterar? ");

            // Itera sobre todos os alunos
            for (int i = 0; i < Alunos.Length; i++)
            {
                // Verifica se o aluno atual corresponde ao código fornecido pelo utilizador
                if (Alunos[i].codAlu == codigoAluno)
                {
                    // Solicita ao utilizador que insira os novos dados do aluno
                    Alunos[i].nomAlu = OperacoesGerais.LerStringValida("Insere o nome: ");
                    Alunos[i].idaAlu = Convert.ToInt32(OperacoesGerais.LerInteiroValido("Insere a idade: ", 0, 100));
                    Alunos[i].medAlu = Convert.ToSingle(OperacoesGerais.LerDecimalValido("Insere a média: ", 0, 20));
                    Alunos[i].proAlu = Convert.ToSingle(OperacoesGerais.LerDecimalValido("Insere a propina do aluno: "));
                    Alunos[i].salAlu = Convert.ToSingle(OperacoesGerais.LerDecimalValido("Insere o saldo: "));

                    // Verifica se o saldo do aluno é negativo (indicando dívidas)
                    if (Alunos[i].salAlu < 0)
                        Alunos[i].teveDividas = true;

                    encontrouAluno = true;
                    break;
                }
            }

            // Se o aluno não foi encontrado, informa o utilizador
            if (!encontrouAluno)
                Console.WriteLine($"O aluno com o código {codigoAluno} não foi encontrado.");
        }

        // Elimina um aluno da estrutura Alunos
        public static void EliminarAluno(ref Aluno[] Alunos)
        {
            string codigoAluno;
            string entrada;
            bool entradaValida;
            bool alunoEncontrado = false;

            // Mostra os alunos para que o utilizador possa escolher qual remover
            MostrarAlunos(Alunos, 0);

            // Solicita o código do aluno que o utilizador deseja remover
            codigoAluno = OperacoesGerais.LerStringValida("Qual o código do aluno que deseja eliminar? ");

            for (int i = 0; i < Alunos.Length; i++) // Itera sobre todos os alunos na lista
            {
                if (Alunos[i].codAlu == codigoAluno) // Verifica se o código do aluno atual corresponde ao código fornecido pelo utilizador
                {
                    alunoEncontrado = true; // Confirma que encontrou o aluno

                    do // Confirmação da remoção com o utilizador
                    {
                        entrada = OperacoesGerais.LerStringValida($"Pretende eliminar o aluno {codigoAluno}? (s/n): ").ToLower(); // Lê a entrada e converte para minúsculas

                        switch (entrada)
                        {
                            case "s":
                                // Move os elementos para preencher o espaço do aluno removido
                                for (int j = i; j < Alunos.Length - 1; j++)
                                    Alunos[j] = Alunos[j + 1];

                                Array.Resize(ref Alunos, Alunos.Length - 1);

                                Console.WriteLine($"O aluno com o código {codigoAluno} foi eliminado.");

                                entradaValida = true; // Define a flag como true para sair do loop
                                break;

                            case "n":
                                Console.WriteLine("Registo não eliminado.");

                                entradaValida = true; // Define a flag como true para sair do loop
                                break;

                            default:
                                Console.WriteLine("Entrada inválida. Por favor, insira 'S' ou 'N'.");
                                entradaValida = false; // Define a flag como false para continuar no loop
                                break;
                        }
                    } while (!entradaValida);
                }
            }

            if (!alunoEncontrado)
                Console.WriteLine($"O aluno com o código {codigoAluno} não foi encontrado.");
        }
    }

    public class OperacoesFinanceiras
    {
        // Paga as propinas de um aluno ou de todos os alunos
        public static void PagarPropinas(Aluno[] Alunos)
        {
            OperacoesComAlunos.MostrarAlunos(Alunos, 0); // Mostra a lista de alunos

            string codigoAluno = OperacoesGerais.LerStringValida("Qual é o aluno que quer pagar as propinas? - ");

            bool encontreiAluno = false;

            // Itera sobre todos os alunos
            for (int i = 0; i < Alunos.Length; i++)
            {
                if (Alunos[i].codAlu == codigoAluno)
                {
                    encontreiAluno = true;

                    // Mostra as opções de pagamento de propinas
                    Console.WriteLine("1. Pagar as propinas do mês corrente.");
                    Console.WriteLine("2. Pagar as propinas de outro mês que não seja o mês corrente.");
                    Console.WriteLine($"3. Pagar todas as propinas do aluno {Alunos[i].nomAlu}. \n");

                    // Recebe a escolha do utilizador
                    int escolha = Convert.ToInt32(Console.ReadLine());

                    switch (escolha)
                    {
                        case 1:
                            // Verifica se as propinas do mês corrente não foram pagas
                            if (!Alunos[i].mesesParaPagar[DateTime.Now.Month - 1].estadoPagamento)
                            {
                                Alunos[i].mesesParaPagar[DateTime.Now.Month - 1].estadoPagamento = true; // Marca as propinas do mês corrente como pagas
                                Console.WriteLine($"As propinas do mês atual para o aluno {Alunos[i].codAlu} foram pagas.");
                            }
                            else
                                Console.WriteLine($"As propinas do mês atual para o aluno {Alunos[i].codAlu} já estão pagas.");
                            break;

                        case 2:
                            Console.WriteLine();
                            int mesEscolhido = Convert.ToInt32(OperacoesGerais.LerInteiroValido("Escreva o número do mês que quer pagar: "));

                            // Verifica se as propinas do mês escolhido não foram pagas
                            if (!Alunos[i].mesesParaPagar[mesEscolhido - 1].estadoPagamento)
                            {
                                Alunos[i].mesesParaPagar[mesEscolhido - 1].estadoPagamento = true; // Marca as propinas do mês escolhido como pagas
                                Console.WriteLine($"As propinas do mês {mesEscolhido} para o aluno {Alunos[i].codAlu} foram pagas.");
                            }
                            else
                                Console.WriteLine($"As propinas do mês {mesEscolhido} para o aluno {Alunos[i].codAlu} já estão pagas.");
                            break;

                        case 3:
                            // Itera sobre todos os meses de propinas do aluno atual
                            for (int j = 0; j < Alunos[i].mesesParaPagar.Length; j++)
                            {
                                // Verifica se as propinas do mês atual não foram pagas
                                if (!Alunos[i].mesesParaPagar[j].estadoPagamento)
                                {
                                    Alunos[i].mesesParaPagar[j].estadoPagamento = true; // Marca as propinas do mês atual como pagas
                                    Console.WriteLine($"As propinas do aluno {Alunos[i].codAlu} foram pagas.");
                                }
                                else
                                    Console.WriteLine($"As propinas do aluno {Alunos[i].codAlu} já estão pagas.");
                            }
                            break;

                        default:
                            Console.WriteLine("Opção Inválida");
                            break;
                    }
                }
            }

            if (!encontreiAluno)
                Console.WriteLine($"O aluno com o código {codigoAluno} não foi encontrado.");
        }

        // Mostra alunos com dividas e alunos que já tiveram dividas!
        public static void AlunosComDividas(Aluno[] Alunos)
        {
            // Iterar sobre todos os alunos 
            for (int i = 0; i < Alunos.Length; i++)
            {
                // Verificar se o aluno teve dívidas
                if (AlunoTemDividas(Alunos[i]))
                {
                    Console.WriteLine($"O aluno {Alunos[i].nomAlu} ({Alunos[i].codAlu}) tem dividas.");
                }
                else if (Alunos[i].teveDividas)
                {
                    Console.WriteLine($"O aluno {Alunos[i].nomAlu} ({Alunos[i].codAlu}) já teve dívidas, mas está em dia agora.");
                }
            }
        }

        // Verifica se um aluno tem dividas!
        public static bool AlunoTemDividas(Aluno aluno)
        {
            // Iterar sobre os meses para pagar do aluno
            for (int j = 0; j < aluno.mesesParaPagar.Length; j++)
            {
                // Verificar se o pagamento do mês não foi feito
                if (!aluno.mesesParaPagar[j].estadoPagamento && aluno.mesesParaPagar[j].mes < DateTime.Now.Month)
                    return true;
            }
            return false;
        }

        // Exibe os alunos que nunca tiveram dívidas
        public static void AlunoSemDividas(Aluno[] Alunos)
        {
            // Itera sobre todos os alunos
            for (int i = 0; i < Alunos.Length; i++)
            {
                // Verifica se o aluno não teve dívidas
                if (!Alunos[i].teveDividas)
                    // Mostra as informações do aluno
                    OperacoesComAlunos.MostrarAlunos(Alunos, 0);
            }
        }

        // Verifica se os alunos têm dívidas futuras no próximo mês
        public static void VerificarDividasFuturas(Aluno[] Aluno, int mesAtual)
        {
            Console.WriteLine("Dívidas futuras:");

            // Iterar sobre todos os alunos
            foreach (var aluno in Aluno)
            {
                // Verificar se o aluno tem uma dívida no próximo mês
                if (aluno.DividaProxima(mesAtual))
                    Console.WriteLine($"Aluno {aluno.nomAlu} ({aluno.codAlu}) tem uma dívida no próximo mês.");
                else
                    Console.WriteLine($"Aluno {aluno.nomAlu} ({aluno.codAlu}) não tem uma dívida no próximo mês.");
            }
        }

        // Carrega saldo para um aluno específico
        public static void CarregarSaldo(Aluno[] Alunos)
        {
            // Variáveis para armazenar o código do aluno e o valor a ser carregado
            string codAluno;
            int valor;

            // Solicita o código do aluno que deseja carregar o saldo
            codAluno = OperacoesGerais.LerStringValida("Qual é o aluno que quer carregar o saldo? - ");

            // Itera sobre todos os alunos na lista
            for (int i = 0; i < Alunos.Length; i++)
            {
                // Verifica se o código do aluno atual corresponde ao código fornecido
                if (Alunos[i].codAlu == codAluno)
                {
                    // Solicita o valor a ser carregado para o saldo do aluno
                    valor = Convert.ToInt32(OperacoesGerais.LerInteiroValido("Qual é o montante que quer carregar o saldo? - "));

                    // Adiciona o valor ao saldo do aluno correspondente
                    Alunos[i].salAlu = Alunos[i].salAlu + valor;

                    // Exibe mensagem de confirmação do carregamento do saldo
                    Console.WriteLine("O saldo foi carregado.");
                }
            }
        }
    }

    public class OperacoesGerais
    {
        // Função que verifica a existência de alunos
        public static bool VerificarQuantidadeAlunos (Aluno[] Alunos)
        {
            if (Alunos.Length == 0)
                return true;

            return false;
        }

        // Função que mostra o Melhor e o Pior Aluno sem dividas
        public static void MelhorPiorAluno(Aluno[] Alunos)
        {
            // Indices para saber as posições dos alunos
            int indicePiorAluno = 0;
            int indiceMelhorAluno = 0;

            float melhorMedia = -1;
            float piorMedia = -1;

            // Ciclo para encontrar o melhor e o pior aluno que nunca tiveram dividas 
            for (int i = 0; i < Alunos.Length; i++)
            {
                // Ciclo para verificar se o aluno nunca teve dívidas
                for (int j = 0; j < Alunos[i].mesesParaPagar.Length; j++)
                {
                    // Verifica o estado do pagamento do mês (o mês tem que ser anterior ao atual)
                    if (Alunos[i].mesesParaPagar[j].estadoPagamento == true && Alunos[i].mesesParaPagar[j].mes <= DateTime.Now.Month)
                    {
                        // Verifica se a média do aluno atual é maior que a do melhor aluno
                        if (Alunos[i].medAlu > melhorMedia)
                        {
                            melhorMedia = Alunos[i].medAlu;
                            indiceMelhorAluno = i;
                        }
                        // Verifica se a média do aluno atual é menor que a do pior aluno
                        else if (Alunos[i].medAlu < piorMedia)
                        {
                            piorMedia = Alunos[i].medAlu;
                            indicePiorAluno = i;
                        }
                    }
                }
            }
            // Exibe o melhor e pior aluno que nnca tiveram dividas 
            Console.WriteLine($"Melhor aluno: {Alunos[indiceMelhorAluno].nomAlu}, Média: {melhorMedia}");
            Console.WriteLine($"Pior aluno: {Alunos[indicePiorAluno].nomAlu}, Média: {piorMedia}");
        }

        // Função para escrever o array de alunos no arquivo
        public static void escreverArrayParaFicheiro(Aluno[] Alunos, string ficheiro)
        {
            // para guardar chars: ç ã ê á ... no ficheiro
            Console.OutputEncoding = Encoding.UTF8;
            // abrir o ficheiro para ESCRITA
            StreamWriter textWriter = new StreamWriter(ficheiro);

            // separar campo a campo da esturtura por ponto e vírgula
            CsvConfiguration csvConf = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = ";"
            };

            // var que escrever cada registo
            CsvWriter writer = new CsvWriter(textWriter, csvConf);

            // definir o cabeçalho dos campos no ficheiro
            writer.WriteField("Codigo");
            writer.WriteField("Nome");
            writer.WriteField("Idade");
            writer.WriteField("Media");
            writer.WriteField("Propina");
            writer.WriteField("Saldo");
            for (int j = 0; j < 12; j++)
            {
                writer.WriteField($"Mes{j + 1}");
                writer.WriteField($"Mes{j + 1}_Pago");
                writer.WriteField($"Mes{j + 1}_Divida");
            }
            writer.NextRecord();

            // ciclo para leitura do array e registo de estruturas no ficheiro
            for (int i = 0; i < Alunos.Length; i++)
            {
                // Escrever o cabeçalho dos campos no arquivo
                writer.WriteField(Alunos[i].codAlu);
                writer.WriteField(Alunos[i].nomAlu);
                writer.WriteField(Convert.ToString(Alunos[i].idaAlu));
                writer.WriteField(Convert.ToString(Alunos[i].medAlu));
                writer.WriteField(Convert.ToString(Alunos[i].proAlu));
                writer.WriteField(Convert.ToString(Alunos[i].salAlu));

                foreach (Aluno.EstadoMensal mes in Alunos[i].mesesParaPagar)
                {
                    writer.WriteField(Convert.ToString(mes.mes));
                    writer.WriteField(Convert.ToString(mes.estadoPagamento));
                    writer.WriteField(Convert.ToString(mes.valorDivida));
                }

                writer.NextRecord();
            }

            textWriter.Close(); // fecho do ficheiro
        }

        // Função para ler o ficheiro e criar o array
        public static void lerFicheiroParaArray(ref Aluno[] Alunos, string ficheiro)
        {
            int i;
            // Abrir o ficheiro para LEITURA
            StreamReader textReader = new StreamReader(ficheiro);

            // Configurar o leitor CSV
            CsvConfiguration csvConf2 = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = ";"
            };

            // Criar o leitor CSV
            CsvReader reader = new CsvReader(textReader, csvConf2);

            // Ignorar o cabeçalho
            i = 0;
            while (reader.Read() == true)
            {
                if (i > 0)
                {
                    // carregar o array com os registos do ficheiro s/ cabeçalho
                    Array.Resize(ref Alunos, Alunos.Length + 1);
                    Alunos[i - 1] = new Aluno();

                    // Ler dados básicos do aluno
                    Alunos[i - 1].codAlu = reader.GetField(0);
                    Alunos[i - 1].nomAlu = reader.GetField(1);
                    Alunos[i - 1].idaAlu = Convert.ToInt32(reader.GetField(2));
                    Alunos[i - 1].medAlu = Convert.ToSingle(reader.GetField(3));
                    Alunos[i - 1].proAlu = Convert.ToSingle(reader.GetField(4));
                    Alunos[i - 1].salAlu = Convert.ToSingle(reader.GetField(5));

                    // Inicializar o array mesesParaPagar
                    Alunos[i - 1].mesesParaPagar = new Aluno.EstadoMensal[12];

                    // Ler dados dos meses para pagar
                    for (int j = 0; j < 12; j++)
                    {
                        Alunos[i - 1].mesesParaPagar[j].mes = Convert.ToInt32(j + 1);
                        Alunos[i - 1].mesesParaPagar[j].estadoPagamento = bool.Parse(reader.GetField(7 + (j * 3)));
                        Alunos[i - 1].mesesParaPagar[j].valorDivida = Convert.ToSingle(reader.GetField(8 + (j * 3)));
                    }
                }
                i++;
            }
            textReader.Close();

            Console.WriteLine("O ficheiro foi carregado com sucesso. Prima Enter para continuar.");
            Console.ReadKey();
        }

        // Verifica se o código do aluno já existe na estrutura Alunos
        public static bool VerificarCodigo(Aluno[] col, string codigoAlu)
        {
            foreach (var aluno in col)
            {
                if (aluno != null && codigoAlu == aluno.codAlu)
                    return true;
            }
            return false;
        }

        // Função para ler uma string não vazia
        public static string LerStringValida(string mensagem)
        {
            string entrada;
            do
            {
                Console.Write(mensagem); // Mostra a mensagem de prompt e lê a entrada do utilizador
                entrada = Console.ReadLine().Trim();
            } while (string.IsNullOrWhiteSpace(entrada)); // Repete até ser inserida uma string não vazia
            return entrada;
        }

        // Função para ler um inteiro válido do console dentro do intervalo especificado, com mensagem de prompt opcional
        public static int LerInteiroValido(string mensagem, int valorMinimo = int.MinValue, int valorMaximo = int.MaxValue)
        {
            int entrada;
            bool valido;
            do
            {
                Console.Write(mensagem); // Mostra a mensagem de prompt e lê a entrada do utilizador
                valido = int.TryParse(Console.ReadLine(), out entrada) && entrada >= valorMinimo && entrada <= valorMaximo;
            } while (!valido); // Repete até ser inserido um inteiro válido dentro do intervalo especificado
            return entrada;
        }

        // Lê um número decimal válido da consola dentro do intervalo especificado, com mensagem de prompt opcional
        public static float LerDecimalValido(string mensagem, float valorMinimo = float.MinValue, float valorMaximo = float.MaxValue)
        {
            float entrada;
            bool valido;
            do
            {
                Console.Write(mensagem); // Mostra a mensagem de prompt e lê a entrada do utilizador
                valido = float.TryParse(Console.ReadLine(), out entrada) && entrada >= valorMinimo && entrada <= valorMaximo;
            } while (!valido); // Repete até ser inserido um número decimal válido dentro do intervalo especificado
            return entrada;
        }
    }

    public class Estatistica
    {
        // Função para exibir opções de estatísticas
        public static void ExibirOpcoesEstatisticas(Aluno[] Alunos)
        {
            Console.WriteLine("\nOpções Estatísticas:");
            Console.WriteLine("1. Média das idades dos alunos");
            Console.WriteLine("2. Média das médias dos alunos");
            Console.WriteLine("3. Número de alunos com saldo negativo");
            Console.WriteLine("4. Percentagem de alunos com dívidas");
            Console.WriteLine("5. Todas\n");

            int opcao = Convert.ToInt16(OperacoesGerais.LerInteiroValido("Escolha uma opção: "));

            switch (opcao)
            {
                case 1:
                    CalcularMediaIdades(Alunos);
                    break;

                case 2:
                    CalcularMediaMedias(Alunos);
                    break;

                case 3:
                    ContarAlunosComSaldoNegativo(Alunos);
                    break;

                case 4:
                    CalcularPercentagemAlunosComDividas(Alunos);
                    break;

                case 5:
                    CalcularMediaIdades(Alunos);
                    CalcularMediaMedias(Alunos);
                    ContarAlunosComSaldoNegativo(Alunos);
                    CalcularPercentagemAlunosComDividas(Alunos);
                    break;

                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }

        // Calcula a média das idades dos alunos
        public static void CalcularMediaIdades(Aluno[] Alunos)
        {
            int somaIdades = 0;

            // Itera sobre cada aluno na lista
            foreach (var aluno in Alunos)
            {
                // Soma as idades de todos os alunos
                somaIdades += aluno.idaAlu;
            }
            // Calcula a média das idades 
            Console.WriteLine($"A média das idades dos alunos é: {somaIdades / Alunos.Length}");
        }

        // Calcula a média das médias dos alunos
        public static void CalcularMediaMedias(Aluno[] Alunos)
        {
            float somaMedias = 0;

            // Itera sobre cada aluno na lista
            foreach (var aluno in Alunos)
            {
                // Soma as médias de todos os alunos
                somaMedias += aluno.medAlu;
            }

            // Calcula a média das médias
            Console.WriteLine($"A média das médias dos alunos é: {somaMedias / Alunos.Length}");
        }

        // Conta quantos alunos têm saldo negativo
        public static void ContarAlunosComSaldoNegativo(Aluno[] Alunos)
        {
            int contador = 0;

            // Itera sobre cada aluno na lista
            foreach (var aluno in Alunos)
            {
                // Verifica se o saldo do aluno é negativo e incrementa o contador
                if (aluno.salAlu < 0)
                    contador++;
            }

            // Número de alunos com saldo negativo
            Console.WriteLine($"O número de alunos com saldo negativo é: {contador}");

        }

        // Calcula a percentagem de alunos que já tiveram dívidas
        public static void CalcularPercentagemAlunosComDividas(Aluno[] Alunos)
        {
            int alunosComDividas = 0;

            // Itera sobre cada aluno na lista
            foreach (var aluno in Alunos)
            {
                // Verifica se o aluno já teve dívidas e incrementa o contador
                if (aluno.teveDividas)
                    alunosComDividas++;
            }

            // Calcula a percentagem de alunos com dívidas
            float percentagemAlunosDividas = (float)alunosComDividas / Alunos.Length * 100;
            Console.WriteLine($"A percentagem de alunos com dívidas é: {percentagemAlunosDividas:F2}%");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Aluno[] Alunos = new Aluno[0]; // Array para armazenar os alunos
            int opcaoMenu = 0; // Opção do menu
            string ficheiro = @"C:/Temp/alunos.csv"; // def de path + ficheiro csv

            // Loop do menu
            do
            {
                opcaoMenu = ListaMenu(Alunos); // Chama a função para exibir o menu e recebe a opção escolhida

                switch (opcaoMenu)
                {
                    case 1:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Inserir alunos **********\n");
                        OperacoesComAlunos.InserirAlunos(ref Alunos); // Chama a função para inserir alunos
                        break;

                    case 2:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Listar alunos **********\n");
                        OperacoesComAlunos.MostrarAlunos(Alunos, 0); // Chama a função para listar alunos
                        break;

                    case 3:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Consultar Aluno **********\n");
                        OperacoesComAlunos.ConsultarAluno(Alunos); // Chama a função para consultar um aluno
                        break;

                    case 4:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Alterar dados de Aluno **********\n");
                        OperacoesComAlunos.AlterarDadosAluno(Alunos); // Chama a função para alterar os dados de um aluno
                        break;

                    case 5:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Eliminar Aluno **********\n");
                        OperacoesComAlunos.EliminarAluno(ref Alunos); // Chama a função para eliminar um aluno
                        break;

                    case 6:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Pagar Propinas **********\n");
                        OperacoesFinanceiras.PagarPropinas(Alunos); // Chama a função para pagar propinas
                        break;

                    case 7:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Alunos que já tiveram dívidas **********\n");
                        OperacoesFinanceiras.AlunosComDividas(Alunos); // Chama a função para listar os alunos que já tiveram dívidas
                        break;

                    case 8:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Verificar dívidas futuras **********\n");
                        OperacoesFinanceiras.VerificarDividasFuturas(Alunos, DateTime.Now.Month); // Chama a função para verificar as dívidas futuras
                        break;

                    case 9:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Carregar Saldo **********\n");
                        OperacoesFinanceiras.CarregarSaldo(Alunos); // Chama a função para carregar saldo de um aluno
                        break;

                    case 10:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Melhor / Pior Aluno **********\n");
                        OperacoesGerais.MelhorPiorAluno(Alunos); // Chama a função para carregar saldo de um aluno
                        break;

                    case 11:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Mostrar Alunos sem Dívidas **********\n");
                        OperacoesFinanceiras.AlunoSemDividas(Alunos);
                        break;

                    case 12:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Exportar Lista de Alunos para ficheiro **********\n");
                        OperacoesGerais.escreverArrayParaFicheiro(Alunos, ficheiro);
                        break;

                    case 13:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Importar ficheiro para Lista de Alunos **********\n");
                        OperacoesGerais.lerFicheiroParaArray(ref Alunos, ficheiro);
                        break;

                    case 14:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Estatisticas **********\n");
                        Estatistica.ExibirOpcoesEstatisticas(Alunos);
                        break;
                }
            } while (opcaoMenu != 0);
        }

        // Menu de opções
        static int ListaMenu(Aluno[] Alunos)
        {
            // Exibe as opções de menu e retorna a opção escolhida
            Console.WriteLine("\nGestão de Alunos");
            Console.WriteLine("==========================================\n");
            Console.WriteLine("1. Inserir Aluno(s)");

            // Se não existirem alunos na lista, não mostra
            if (!OperacoesGerais.VerificarQuantidadeAlunos(Alunos))
            {
                Console.WriteLine("2. Listagem de Alunos");
                Console.WriteLine("3. Consultar Aluno");
                Console.WriteLine("4. Alterar dados de um Aluno");
                Console.WriteLine("5. Eliminar um Aluno");
                Console.WriteLine("6. Pagar Propinas");
                Console.WriteLine("7. Listagem de dívidas");
                Console.WriteLine("8. Listagem de dívidas próximas");
                Console.WriteLine("9. Carregar o saldo de um Aluno");
                Console.WriteLine("10. Mostrar o melhor e o pior Aluno sem dívidas");
                Console.WriteLine("11. Mostrar Aluno que nunca teve dívidas");
                Console.WriteLine("12. Carregar array de Alunos");
            }
                Console.WriteLine("13. Carregar ficheiro com array");

            // Se não existirem alunos na lista, não mostra
            if (!OperacoesGerais.VerificarQuantidadeAlunos(Alunos))
                Console.WriteLine("14. Estatisticas");
            
            Console.WriteLine("0. Sair");

            return Convert.ToInt32(OperacoesGerais.LerDecimalValido("\nEscolhe a opção: "));
        }  
    }
}