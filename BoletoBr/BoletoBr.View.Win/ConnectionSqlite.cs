using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BoletoBr.View.Win.Dominio;
using SQLite;

namespace BoletoBr.View.Win
{
    public class ConnectionSqlite
    {
        private SQLiteConnection CriarTabelaCarteiraBoleto()
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory + "BoletoBr.db3";
            var conn = new SQLiteConnection(path);
            conn.CreateTable<CarteiraBoleto>();
            
            return conn;
        }

        public void SalvarCarteiraBoleto(CarteiraBoleto carteiraBoleto)
        {
            try
            {
                var connection = CriarTabelaCarteiraBoleto();
                if (carteiraBoleto.IdCarteiraBoleto == 0)
                    connection.Insert(carteiraBoleto);
                else
                    connection.Update(carteiraBoleto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CarteiraBoleto> GetAll()
        {
            try
            {
                var connection = CriarTabelaCarteiraBoleto();
                return connection.Table<CarteiraBoleto>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluirCarteiraBoleto(CarteiraBoleto carteiraBoleto)
        {
            try
            {
                var connection = CriarTabelaCarteiraBoleto();
                connection.Delete(carteiraBoleto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
