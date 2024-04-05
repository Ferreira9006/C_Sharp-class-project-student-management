using System;

namespace Gestao_de_Alunos
{
    // Estrutura de dados para representar um Aluno
    // Autores : Gabriel / Rui / Douglas 
    public class sAluno
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

        // Construtor para inicializar um objeto sAluno
        public sAluno()
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
        public bool DividaProxima(int mesAtual)
        {
            // Posição no Array e não o mês
            int proximoMes = mesAtual;

            // Verifica se o próximo mês existe na lista de mesesParaPagar
            if (proximoMes < mesesParaPagar.Length)
            {
                // 1 2 3 4 5 6 7 8 9 10 11 12 (meses)
                // 0 1 2 3 4 5 6 7 8 9  10 11 (Array)

                // Verifica se o estado de pagamento do próximo mês é falso (ou seja, há uma dívida)
                if (!mesesParaPagar[proximoMes].estadoPagamento)
                {
                    return true; // Há uma dívida no próximo mês
                }
            }
            return false; // Não há dívida no próximo mês
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            sAluno[] sAlu = new sAluno[0]; // Array para armazenar os alunos
            int opcaoMenu = 0; // Opção do menu

            // Loop do menu
            do
            {
                opcaoMenu = ListaMenu(); // Chama a função para exibir o menu e recebe a opção escolhida

                switch (opcaoMenu)
                {
                    case 1:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Inserir alunos **********");
                        InserirAlunos(ref sAlu); // Chama a função para inserir alunos
                        break;

                    case 2:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Listar alunos **********");
                        MostrarAlunos(sAlu, 0); // Chama a função para listar alunos
                        break;

                    case 3:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Consultar Aluno **********");
                        ConsultarAluno(sAlu); // Chama a função para consultar um aluno
                        break;

                    case 4:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Alterar dados de Aluno **********");
                        AlterarDadosAluno(sAlu); // Chama a função para alterar os dados de um aluno
                        break;

                    case 5:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Eliminar Aluno **********");
                        EliminarAluno(ref sAlu); // Chama a função para eliminar um aluno
                        break;

                    case 6:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Pagar Propinas **********");
                        PagarPropinas(sAlu); // Chama a função para pagar propinas
                        break;

                    case 7:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Alunos que já tiveram dívidas **********");
                        AlunosComDividas(sAlu); // Chama a função para listar os alunos que já tiveram dívidas
                        break;

                    case 8:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Verificar dívidas futuras **********");
                        VerificarDividasFuturas(sAlu, DateTime.Now.Month); // Chama a função para verificar as dívidas futuras
                        break;

                    case 9:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Carregar Saldo **********");
                        CarregarSaldo(sAlu); // Chama a função para carregar saldo de um aluno
                        break;

                    case 10:
                        Console.Clear(); // Limpa a Consola
                        Console.WriteLine("********** Melhor / Pior Aluno **********");
                        MelhorPiorAluno(sAlu); // Chama a função para carregar saldo de um aluno
                        break;

                    case 11:
                        Console.Clear(); // Limpa a Consola
                        AlunoSemDividas(sAlu);
                        break;

                    case 12:
                        Console.Clear(); // Limpa a Consola
                        break;

                    case 13:
                        Console.Clear(); // Limpa a Consola
                        break;

                    case 14:
                        Console.Clear(); // Limpa a Consola
                        ExibirOpcoesEstatisticas(sAlu);
                        break;
                }
            } while (opcaoMenu != 0);
        }

        // Menu de opções
        static int ListaMenu()
        {
            // Exibe as opções de menu e retorna a opção escolhida
            Console.WriteLine("\nGestão de Alunos");
            Console.WriteLine("==========================================");
            Console.WriteLine("1. Inserir Aluno");
            Console.WriteLine("2. Listar Alunos");
            Console.WriteLine("3. Consultar Aluno");
            Console.WriteLine("4. Alterar dados de Aluno");
            Console.WriteLine("5. Eliminar Aluno");
            Console.WriteLine("6. Pagar Propinas");
            Console.WriteLine("7. Listagem de dívidas");
            Console.WriteLine("8. Listagem de dívidas próximas");
            Console.WriteLine("9. Carregar o saldo de um Aluno");
            Console.WriteLine("10. Calcular melhor e pior Aluno sem dívidas");
            Console.WriteLine("11. Calcular Aluno que nunca teve dívidas");
            Console.WriteLine("12. Carregar array de Alunos");
            Console.WriteLine("13. Carregar ficheiro com array");
            Console.WriteLine("14. Estatisticas");
            Console.WriteLine("0. Sair");

            Console.Write("Escolhe a opção: ");
            return Convert.ToInt32(Console.ReadLine());
        }

        // Insere alunos na estrutura
        // Solicita informações do aluno e insere na estrutura sAlu
        static void InserirAlunos(ref sAluno[] sAlu)
        {
            Console.Write("Digite o número de alunos a inserir: ");
            int quantidadeAlunos = Convert.ToInt32(Console.ReadLine());

            int tamanhoAtual = sAlu.Length;
            int newArraySize = tamanhoAtual + quantidadeAlunos;

            // Resize do array para acomodar os novos alunos
            Array.Resize(ref sAlu, newArraySize);

            // Ciclo para introdução dos dados dos alunos
            for (int i = tamanhoAtual; i < newArraySize; i++)
            {
                sAlu[i] = new sAluno(); // Inicialização do objeto sAluno

                bool codigoValido = false;
                string codigoAluno;

                do
                {
                    Console.Write("\n Insere o código do aluno: ");
                    codigoAluno = Console.ReadLine();

                    // Verifica se o código do aluno já existe ou não
                    codigoValido = !VerificarCodigo(sAlu, codigoAluno);
                    if (!codigoValido)
                        Console.WriteLine("O código do aluno já existe.");

                } while (!codigoValido);

                sAlu[i].codAlu = codigoAluno;

                Console.Write("Insere o nome: ");
                sAlu[i].nomAlu = Console.ReadLine();

                Console.Write("Insere a idade: ");
                sAlu[i].idaAlu = Convert.ToInt32(Console.ReadLine());

                Console.Write("Insere média: ");
                sAlu[i].medAlu = Convert.ToSingle(Console.ReadLine());

                Console.Write("Insere a propina do aluno: ");
                sAlu[i].proAlu = Convert.ToSingle(Console.ReadLine());

                Console.Write("Insere o saldo: ");
                sAlu[i].salAlu = Convert.ToSingle(Console.ReadLine());

                // Se o aluno tem um saldo negativo, teveDividas = true
                if (sAlu[i].salAlu < 0)
                    sAlu[i].teveDividas = true;

                // Inicialize o array de dívidas para cada aluno
                for (int j = 0; j < sAlu[i].mesesParaPagar.Length; j++)
                {
                    bool entradaValida; // Flag que verifica se a opção introduzida é valida
                    string entrada; // Declara a variável de entrada fora do loop

                    // Ciclo para definir o estado das dívidas
                    do
                    {
                        Console.Write($"O aluno pagou o mês {sAlu[i].mesesParaPagar[j].mes}? (S/N) - ");
                        entrada = Console.ReadLine().ToLower(); // Lê a entrada e converte para minúsculas

                        if (entrada == "s")
                        {
                            sAlu[i].mesesParaPagar[j].estadoPagamento = true;
                            entradaValida = true; // Define a flag como true para sair do loop
                        }
                        else if (entrada == "n")
                        {
                            sAlu[i].mesesParaPagar[j].estadoPagamento = false;

                            // Se o aluno não pagou o mês anterior ao atual
                            if (sAlu[i].mesesParaPagar[j].mes < DateTime.Now.Month)
                                sAlu[i].teveDividas = true;

                            entradaValida = true; // Define a flag como true para sair do loop
                        }
                        else
                        {
                            Console.WriteLine("Entrada inválida. Por favor, insira 'S' ou 'N'.");
                            entradaValida = false; // Define a flag como false para continuar do loop
                        }
                    } while (!entradaValida);
                }
            }
        }

        // Verifica se o código do aluno já existe na estrutura sAlu
        static bool VerificarCodigo(sAluno[] col, string codigoAlu)
        {
            foreach (var aluno in col)
            {
                if (aluno != null && codigoAlu == aluno.codAlu)
                    return true;
            }
            return false;
        }

        // Mostra informações de todos os alunos na estrutura sAlu
        static void MostrarAlunos(sAluno[] sAlu, int flag)
        {
            if (sAlu.Length > 0)
            {
                for (int i = 0; i < sAlu.Length; i++)
                    MostrarInformacaoAluno(sAlu[i], ref flag);
            }
            else
                Console.WriteLine("Não existe nenhum aluno na lista.");
        }

        // Mostra informações detalhadas de um aluno
        static void MostrarInformacaoAluno(sAluno reg, ref int flag)
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
                    if (reg.mesesParaPagar[i].estadoPagamento)
                        estadoDoPagamento = "Pago";
                    else
                        estadoDoPagamento = "Não Pago";

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
        static void ConsultarAluno(sAluno[] sAlu)
        {
            string codigoAluno;

            // Verifica se há alunos na lista
            if (sAlu.Length > 0)
            {
                MostrarAlunos(sAlu, 0); // Mostra a lista de alunos disponíveis

                Console.Write("Qual o código do aluno que quer consultar? - ");
                codigoAluno = Console.ReadLine();

                bool encontrouAluno = false;
                int flag = 1; // Define a flag para mostrar a informação detalhada do aluno

                // Itera sobre todos os alunos
                foreach (var aluno in sAlu)
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
            else
                Console.WriteLine("\n Não existem alunos na lista. \n"); // Mensagem exibida se não houver alunos na lista
        }

        // Altera dados de um aluno específico
        static void AlterarDadosAluno(sAluno[] sAlu)
        {
            // Verifica se há alunos na lista
            if (sAlu.Length > 0)
            {
                string codigoAluno;
                bool encontrouAluno = false;

                // Mostra os alunos para que o utilizador possa escolher qual alterar
                MostrarAlunos(sAlu, 0);

                Console.WriteLine("Qual o código do aluno que quer alterar?");
                codigoAluno = Console.ReadLine();

                // Itera sobre todos os alunos
                for (int i = 0; i < sAlu.Length; i++)
                {
                    // Verifica se o aluno atual corresponde ao código fornecido pelo utilizador
                    if (sAlu[i].codAlu == codigoAluno)
                    {
                        // Solicita ao utilizador que insira os novos dados do aluno
                        Console.Write("Insere o nome: ");
                        sAlu[i].nomAlu = Console.ReadLine();

                        Console.Write("Insere a idade: ");
                        sAlu[i].idaAlu = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Insere média: ");
                        sAlu[i].medAlu = Convert.ToSingle(Console.ReadLine());

                        Console.Write("Insere a propina do aluno: ");
                        sAlu[i].proAlu = Convert.ToSingle(Console.ReadLine());

                        Console.Write("Insere o saldo: ");
                        sAlu[i].salAlu = Convert.ToSingle(Console.ReadLine());

                        // Verifica se o saldo do aluno é negativo (indicando dívidas)
                        if (sAlu[i].salAlu < 0)
                            sAlu[i].teveDividas = true;

                        encontrouAluno = true;
                        break;
                    }
                }

                // Se o aluno não foi encontrado, informa o utilizador
                if (!encontrouAluno)
                    Console.WriteLine($"O aluno com o código {codigoAluno} não foi encontrado.");
            }
            else
                Console.WriteLine("Não existem alunos na lista.");
        }

        // Elimina um aluno da estrutura sAlu
        static void EliminarAluno(ref sAluno[] sAlu)
        {
            // Verifica se há alunos na lista
            if (sAlu.Length > 0)
            {
                string codigoAluno;
                bool encontrouAluno = false;
                int indexToRemove;

                // Mostra os alunos para que o utilizador possa escolher qual remover
                MostrarAlunos(sAlu, 1);

                // Solicita o código do aluno que o utilizador deseja remover
                Console.WriteLine("Qual o código do aluno que deseja eliminar?");
                codigoAluno = Console.ReadLine();

                // Itera sobre todos os alunos na lista
                for (int i = 0; i < sAlu.Length; i++)
                {
                    // Verifica se o código do aluno atual corresponde ao código fornecido pelo utilizador
                    if (sAlu[i].codAlu == codigoAluno)
                    {
                        // Confirmação da remoção com o utilizador
                        Console.Write($"Pretende eliminar o aluno {codigoAluno}? (s/n): ");
                        if (Console.ReadLine().ToLower() == "s")
                        {
                            indexToRemove = i;

                            // Move os elementos para preencher o espaço do aluno removido
                            for (int j = indexToRemove; j < sAlu.Length - 1; j++)
                            {
                                sAlu[j] = sAlu[j + 1];
                            }
                            Array.Resize(ref sAlu, sAlu.Length - 1);

                            Console.WriteLine($"O aluno com o código {codigoAluno} foi eliminado.");

                            encontrouAluno = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Registo não eliminado.");
                            return; // Se o utilizador não quer eliminar a função, sai dela.
                        }
                    }
                }

                // Se o aluno não foi encontrado na lista
                if (!encontrouAluno)
                {
                    Console.WriteLine($"O aluno com o código {codigoAluno} não foi encontrado.");
                    return; // Sai da função
                }
            }
            else
                Console.WriteLine("\n Não existem alunos na lista. \n");
        }

        // Paga as propinas de um aluno ou de todos os alunos
        static void PagarPropinas(sAluno[] sAlu)
        {
            // Verifica se há alunos na lista
            if (sAlu.Length > 0)
            {
                Console.WriteLine("Deseja pagar as propinas de todos os alunos? S/N");
                string resposta = Console.ReadLine().ToLower();

                // Se o utilizador optar por pagar as propinas de todos os alunos
                if (resposta == "s")
                {
                    // Itera sobre todos os alunos
                    for (int i = 0; i < sAlu.Length; i++)
                    {
                        // Itera sobre todos os meses de propinas do aluno atual
                        for (int j = 0; i < sAlu[i].mesesParaPagar[j].mes; j++)
                        {
                            // Verifica se o mês atual não está pago
                            if (sAlu[i].mesesParaPagar[j + 1].estadoPagamento == false)
                            {
                                // Marca o mês atual como pago
                                sAlu[i].mesesParaPagar[j + 1].estadoPagamento = true;
                            }
                        }

                        // Informa que todas as propinas do aluno atual foram pagas
                        Console.WriteLine($"As propinas de todos os meses do aluno {sAlu[i].nomAlu} (Cod:{sAlu[i].codAlu}) já estão pagas.");
                    }
                }
                // Se o utilizador optar por pagar as propinas de um aluno específico
                else
                {
                    // Mostra os alunos para que o utilizador possa escolher qual pagar as propinas
                    MostrarAlunos(sAlu, 0);

                    Console.Write("Qual é o código de aluno que pretende pagar as propinas? - ");
                    string alunoPagarPropinas = Console.ReadLine();

                    // Itera sobre todos os alunos
                    for (int i = 0; i < sAlu.Length; i++)
                    {
                        // Verifica se o aluno atual corresponde ao código fornecido pelo utilizador
                        if (sAlu[i].codAlu == alunoPagarPropinas)
                        {
                            // Mostra as opções de pagamento de propinas
                            Console.WriteLine("1. Pagar as propinas do mês corrente.");
                            Console.WriteLine("2. Pagar as propinas de outro mês que não seja o mês corrente.");
                            Console.WriteLine($"3. Pagar todas as propinas do aluno {sAlu[i].nomAlu}. \n");

                            // Recebe a escolha do utilizador
                            int escolha = Convert.ToInt32(Console.ReadLine());

                            switch (escolha)
                            {
                                case 1:
                                    // Verifica se as propinas do mês corrente não foram pagas
                                    if (sAlu[i].mesesParaPagar[DateTime.Now.Month - 1].estadoPagamento == false)
                                    {
                                        // Marca as propinas do mês corrente como pagas
                                        sAlu[i].mesesParaPagar[DateTime.Now.Month - 1].estadoPagamento = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"As propinas do mês atual para o aluno {sAlu[i].codAlu} já estão pagas.");
                                    }
                                    break;

                                case 2:
                                    Console.WriteLine("Escreva o número do mês que quer pagar: ");
                                    int mesEscolhido = Convert.ToInt32(Console.ReadLine());

                                    // Verifica se as propinas do mês escolhido não foram pagas
                                    if (sAlu[i].mesesParaPagar[mesEscolhido - 1].estadoPagamento == false)
                                    {
                                        // Marca as propinas do mês escolhido como pagas
                                        sAlu[i].mesesParaPagar[mesEscolhido - 1].estadoPagamento = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"As propinas do mês {mesEscolhido} para o aluno {sAlu[i].codAlu} já estão pagas.");
                                    }
                                    break;

                                case 3:
                                    // Itera sobre todos os meses de propinas do aluno atual
                                    for (int j = 0; j < sAlu[i].mesesParaPagar.Length; j++)
                                    {
                                        // Verifica se as propinas do mês atual não foram pagas
                                        if (sAlu[i].mesesParaPagar[j].estadoPagamento == false)
                                        {
                                            // Marca as propinas do mês atual como pagas
                                            sAlu[i].mesesParaPagar[j].estadoPagamento = true;
                                        }
                                    }
                                    Console.WriteLine($"As propinas do aluno {sAlu[i].codAlu} já estão pagas.");
                                    break;

                                default:
                                    Console.WriteLine("Opção Inválida");
                                    break;
                            }
                        }
                    }
                }
            }
            else
                Console.WriteLine("Não existem alunos na lista.");
        }

        // Mostra alunos com dividas e alunos que já tiveram dividas!
        static void AlunosComDividas(sAluno[] sAlu)
        {
            // Iterar sobre todos os alunos 
            for (int i = 0; i < sAlu.Length; i++)
            {
                // Verificar se o aluno teve dívidas
                if (AlunoTemDividas(sAlu[i]))
                {
                    Console.WriteLine($"O aluno {sAlu[i].nomAlu} ({sAlu[i].codAlu}) tem dividas.");
                }
                else if (sAlu[i].teveDividas)
                {
                    Console.WriteLine($"O aluno {sAlu[i].nomAlu} ({sAlu[i].codAlu}) já teve dívidas, mas está em dia agora.");
                }
            }
        }

        // Verifica se um aluno tem dividas!
        static bool AlunoTemDividas(sAluno aluno)
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

        // Verifica se os alunos têm dívidas futuras no próximo mês
        static void VerificarDividasFuturas(sAluno[] sAlu, int mesAtual)
        {
            Console.WriteLine("Dívidas futuras:");

            // Iterar sobre todos os alunos
            for (int i = 0; i < sAlu.Length; i++)
            {
                // Verificar se o aluno tem uma dívida no próximo mês
                if (sAlu[i].DividaProxima(mesAtual))
                    Console.WriteLine($"Aluno {sAlu[i].nomAlu} ({sAlu[i].codAlu}) tem uma dívida no próximo mês.");
                else
                    Console.WriteLine($"Aluno {sAlu[i].nomAlu} ({sAlu[i].codAlu}) não tem uma dívida no próximo mês.");
            }
        }

        // Carrega saldo para um aluno específico
        static void CarregarSaldo(sAluno[] sAlu)
        {
            // Variáveis para armazenar o código do aluno e o valor a ser carregado
            string codAluno;
            int valor;

            // Solicita o código do aluno que deseja carregar o saldo
            Console.Write("Qual é o aluno que quer carregar o saldo? - ");
            codAluno = Console.ReadLine();

            // Itera sobre todos os alunos na lista
            for (int i = 0; i < sAlu.Length; i++)
            {
                // Verifica se o código do aluno atual corresponde ao código fornecido
                if (sAlu[i].codAlu == codAluno)
                {
                    // Solicita o valor a ser carregado para o saldo do aluno
                    Console.Write("Qual é o montante que quer carregar o saldo? - ");
                    valor = Convert.ToInt32(Console.ReadLine());

                    // Adiciona o valor ao saldo do aluno correspondente
                    sAlu[i].salAlu = sAlu[i].salAlu + valor;

                    // Exibe mensagem de confirmação do carregamento do saldo
                    Console.WriteLine("O saldo foi carregado.");
                }
            }
        }

        // Mostra qual é o melhor e o pior aluno sem dividas
        static void MelhorPiorAluno(sAluno[] sAlu)
        {
            // Indices para saber as posições dos alunos
            int indicePiorAluno = 0;
            int indiceMelhorAluno = 0;

            float melhorMedia = -1;
            float piorMedia = -1;

            // Ciclo para encontrar o melhor e o pior aluno que nunca tiveram dividas 
            for (int i = 0; i < sAlu.Length; i++)
            {
                // Ciclo para verificar se o aluno nunca teve dívidas
                for (int j = 0; j < sAlu[i].mesesParaPagar.Length; j++)
                {
                    // Verifica o estado do pagamento do mês (o mês tem que ser anterior ao atual)
                    if (sAlu[i].mesesParaPagar[j].estadoPagamento == true && sAlu[i].mesesParaPagar[j].mes <= DateTime.Now.Month)
                    {
                        // Verifica se a média do aluno atual é maior que a do melhor aluno
                        if (sAlu[i].medAlu > melhorMedia)
                        {
                            melhorMedia = sAlu[i].medAlu;
                            indiceMelhorAluno = i;
                        }
                        // Verifica se a média do aluno atual é menor que a do pior aluno
                        else if (sAlu[i].medAlu < piorMedia)
                        {
                            piorMedia = sAlu[i].medAlu;
                            indicePiorAluno = i;
                        }
                    }
                }
            }
            // Exibe o melhor e pior aluno que nnca tiveram dividas 
            Console.WriteLine($"Melhor aluno: {sAlu[indiceMelhorAluno].nomAlu}, Média: {melhorMedia}");
            Console.WriteLine($"Pior aluno: {sAlu[indicePiorAluno].nomAlu}, Média: {piorMedia}");
        }

        // Exibe os alunos que nunca tiveram dívidas
        static void AlunoSemDividas(sAluno[] sAlu)
        {
            // Itera sobre todos os alunos
            for (int i = 0; i < sAlu.Length; i++)
            {
                // Verifica se o aluno não teve dívidas
                if (!sAlu[i].teveDividas)
                    // Mostra as informações do aluno
                    MostrarAlunos(sAlu, 0);
            }
        }

        // Função para exibir opções de estatísticas
        static void ExibirOpcoesEstatisticas(sAluno[] sAlu)
        {
            Console.WriteLine("\nOpções Estatísticas:");
            Console.WriteLine("1. Média das idades dos alunos");
            Console.WriteLine("2. Média das médias dos alunos");
            Console.WriteLine("3. Número de alunos com saldo negativo");
            Console.WriteLine("4. Percentagem de alunos com dívidas");
            Console.WriteLine("5. Todas\n");

            Console.Write("Escolha uma opção: ");
            int opcao = Convert.ToInt32(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    CalcularMediaIdades(sAlu);
                    break;

                case 2:
                    CalcularMediaMedias(sAlu);
                    break;

                case 3:
                    ContarAlunosComSaldoNegativo(sAlu);
                    break;

                case 4:
                    CalcularPercentagemAlunosComDividas(sAlu);
                    break;

                case 5:
                    CalcularMediaIdades(sAlu);
                    CalcularMediaMedias(sAlu);
                    ContarAlunosComSaldoNegativo(sAlu);
                    CalcularPercentagemAlunosComDividas(sAlu);
                    break;

                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }

        // Calcula a média das idades dos alunos
        static void CalcularMediaIdades(sAluno[] sAlu)
        {
            // Verifica se há alunos na lista
            if (sAlu.Length > 0)
            {
                int somaIdades = 0;

                // Itera sobre cada aluno na lista
                foreach (var aluno in sAlu)
                {
                    // Soma as idades de todos os alunos
                    somaIdades += aluno.idaAlu;
                }
                // Calcula a média das idades 
                Console.WriteLine($"A média das idades dos alunos é: {somaIdades / sAlu.Length}");
            }
            else
                Console.WriteLine("Não existem alunos na lista.");
        }

        // Calcula a média das médias dos alunos
        static void CalcularMediaMedias(sAluno[] sAlu)
        {
            // Verifica se há alunos na lista
            if (sAlu.Length > 0)
            {
                float somaMedias = 0;

                // Itera sobre cada aluno na lista
                foreach (var aluno in sAlu)
                {
                    // Soma as médias de todos os alunos
                    somaMedias += aluno.medAlu;
                }
                // Calcula a média das médias
                Console.WriteLine($"A média das médias dos alunos é: {somaMedias / sAlu.Length}");
            }
            else
                Console.WriteLine("Não existem alunos na lista.");
        }

        // Conta quantos alunos têm saldo negativo
        static void ContarAlunosComSaldoNegativo(sAluno[] sAlu)
        {
            // Verifica se há alunos na lista
            if (sAlu.Length > 0)
            {
                int contador = 0;

                // Itera sobre cada aluno na lista
                foreach (var aluno in sAlu)
                {
                    // Verifica se o saldo do aluno é negativo e incrementa o contador
                    if (aluno.salAlu < 0)
                        contador++;
                }

                // Número de alunos com saldo negativo
                Console.WriteLine($"O número de alunos com saldo negativo é: {contador}");
            }
            else
                Console.WriteLine("Não existem alunos na lista.");
        }

        // Calcula a percentagem de alunos que já tiveram dívidas
        static void CalcularPercentagemAlunosComDividas(sAluno[] sAlu)
        {
            // Verifica se há alunos na lista
            if (sAlu.Length > 0)
            {

                int alunosComDividas = 0;

                // Itera sobre cada aluno na lista
                foreach (var aluno in sAlu)
                {
                    // Verifica se o aluno já teve dívidas e incrementa o contador
                    if (aluno.teveDividas)
                        alunosComDividas++;
                }

                // Calcula a percentagem de alunos com dívidas
                float percentagemAlunosDividas = (float)alunosComDividas / sAlu.Length * 100;
                Console.WriteLine($"A percentagem de alunos com dívidas é: {percentagemAlunosDividas:F2}%");
            }
            else
                Console.WriteLine("Não existem alunos na lista.");
        }

    }
}