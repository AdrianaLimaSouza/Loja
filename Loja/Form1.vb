
Imports System.Data.SqlClient 'Namespace quando trabalhamos com DB SQL
Imports System.IO 'quando utilizamos imagens num projeto com base de Dados temos que usar este namespace (Imports.IO)
' esta é uma Aplicação CRUD (Criar, Ler, Atualizar e Deletar) 
Public Class Form1

    Dim con As SqlConnection = New SqlConnection("Data Source=(localdb)\MSSQLLocalDB;Database=Loja;Integrated Security=true;") 'Isto é a ligação a Base de Dados e sempre tem que ser colocada
    Dim ImagemBytes() As Byte 'variável para Imagens

    Sub CarregarDados()
        Dim da As New SqlDataAdapter("SELECT * FROM Produtos", con) ' criamos a variável
        Dim dt As New DataTable

        da.Fill(dt) ' a variável da irá preencher o datagridview e criar a tabela

        dgvProdutos.DataSource = dt

        'Formatar com Currency na Coluna Preço
        dgvProdutos.Columns("Preco").DefaultCellStyle.Format = "C2"
        dgvProdutos.Columns("Preco").DefaultCellStyle.FormatProvider = Globalization.CultureInfo.GetCultureInfo("pt-PT")

        Dim pic1 As New DataGridViewImageColumn
        pic1 = dgvProdutos.Columns(3)
        dgvProdutos.RowTemplate.Height = 70


        pic1.ImageLayout = DataGridViewImageCellLayout.Zoom

        dgvProdutos.AllowUserToAddRows = False

    End Sub

    Private Sub btnInserir_Click(sender As Object, e As EventArgs) Handles btnInserir.Click
        Dim Preco As Decimal
        Decimal.TryParse(txtPreco.Text, Preco)

        Dim cmd As New SqlCommand("INSERT into Produtos(NomeProduto,Preco,Foto) VALUES (@n,@p,@f)", con)

        cmd.Parameters.AddWithValue("@n", txtNome.Text)
        cmd.Parameters.AddWithValue("@p", Preco)
        cmd.Parameters.AddWithValue("@f", ImagemBytes)

        con.Open()
        cmd.ExecuteNonQuery()
        MessageBox.Show("Registo do Produto submetido com sucesso")

        con.Close()

        CarregarDados()
    End Sub

    Private Sub btnImagem_Click(sender As Object, e As EventArgs) Handles btnImagem.Click
        Dim ofd As New OpenFileDialog
        ofd.Filter = "fotosLoja|*.jpg;*.png"

        If ofd.ShowDialog = DialogResult.OK Then
            picFoto.Image = Image.FromFile(ofd.FileName)
            ImagemBytes = File.ReadAllBytes(ofd.FileName)
        End If

    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        Dim id As Integer = dgvProdutos.CurrentRow.Cells("Id").Value

        ' Dim Preco As Decimal
        'Decimal.TryParse(txtPreco.Text, Preco)
        Dim Preco As Decimal = CDec(txtPreco.Text)

        Dim cmd As New SqlCommand("UPDATE Produtos SET NomeProduto=@n,Preco=@p,Foto=@f WHERE id=" & id, con)

        cmd.Parameters.AddWithValue("@n", txtNome.Text)
        cmd.Parameters.AddWithValue("@p", Preco)
        cmd.Parameters.AddWithValue("@f", ImagemBytes)
        cmd.Parameters.AddWithValue("@id", id)

        con.Open()
        cmd.ExecuteNonQuery()
        MessageBox.Show("Produto actualizado com sucesso")

        con.Close()

        CarregarDados()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CarregarDados()
    End Sub

    Private Sub dgvProdutos_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProdutos.CellClick

        txtNome.Text = dgvProdutos.CurrentRow.Cells("NomeProduto").Value

        Dim Valor As Decimal = dgvProdutos.CurrentRow.Cells("Preco").Value
        dgvProdutos.Columns("Preco").DefaultCellStyle.Format = "C2"
        txtPreco.Text = Valor.ToString("C2", Globalization.CultureInfo.GetCultureInfo("pt-PT"))

        'Mostrar imagem na PictureBox

        ImagemBytes = dgvProdutos.CurrentRow.Cells("Foto").Value
        Dim ms As New MemoryStream(ImagemBytes)
        picFoto.Image = Image.FromStream(ms)

    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Dim id As Integer = dgvProdutos.CurrentRow.Cells("Id").Value

        Dim cmd As New SqlCommand("DELETE FROM Produtos WHERE id=" & id, con)

        con.Open()
        cmd.ExecuteNonQuery()
        MessageBox.Show("Produto eliminado com sucesso")

        con.Close()

        CarregarDados()
    End Sub
End Class
