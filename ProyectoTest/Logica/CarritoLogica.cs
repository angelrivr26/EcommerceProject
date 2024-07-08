﻿using ProyectoTest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace ProyectoTest.Logica
{
    public class CarritoLogica
    {
        private static CarritoLogica _instancia = null;

        public CarritoLogica()
        {

        }

        public static CarritoLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CarritoLogica();
                }

                return _instancia;
            }
        }

        public int Registrar(Carrito oCarrito)
        {
            int respuesta = 0;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertarCarrito", oConexion);
                    cmd.Parameters.AddWithValue("IdUsuario", oCarrito.oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("IdProducto", oCarrito.oProducto.IdProducto);
                    cmd.Parameters.AddWithValue("Cantidad", oCarrito.Cantidad);
                    cmd.Parameters.AddWithValue("Adicionales", oCarrito.Adicionales);
                    cmd.Parameters.AddWithValue("PrecioExtra", oCarrito.PrecioExtra);
                    cmd.Parameters.AddWithValue("Observaciones", oCarrito.Observaciones); // Agregar el nuevo campo
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                }
                catch (Exception ex)
                {
                    respuesta = 0;
                }
            }
            return respuesta;
        }


        //public int Registrar(Carrito oCarrito)
        //{
        //    int respuesta = 0;
        //    using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand("sp_InsertarCarrito", oConexion);
        //            cmd.Parameters.AddWithValue("IdUsuario", oCarrito.oUsuario.IdUsuario);
        //            cmd.Parameters.AddWithValue("IdProducto", oCarrito.oProducto.IdProducto);
        //            cmd.Parameters.AddWithValue("Cantidad", oCarrito.Cantidad);
        //            cmd.Parameters.AddWithValue("Adicionales", oCarrito.Adicionales);
        //            cmd.Parameters.AddWithValue("PrecioExtra", oCarrito.PrecioExtra);
        //            cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            oConexion.Open();
        //            cmd.ExecuteNonQuery();
        //            respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);

        //        }
        //        catch (Exception ex)
        //        {
        //            respuesta = 0;
        //        }
        //    }
        //    return respuesta;
        //}


        public int Cantidad(int idusuario)
        {
            int respuesta = 0;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("select count(*) from carrito where idusuario = @idusuario", oConexion);
                    cmd.Parameters.AddWithValue("@idusuario", idusuario);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    respuesta = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                }
                catch (Exception ex)
                {
                    respuesta = 0;
                }
            }
            return respuesta;
        }

        public List<Carrito> Obtener(int _idusuario)
        {
            List<Carrito> lst = new List<Carrito>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ObtenerCarrito", oConexion);
                    cmd.Parameters.AddWithValue("IdUsuario", _idusuario);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lst.Add(new Carrito()
                            {
                                IdCarrito = Convert.ToInt32(dr["IdCarrito"]),
                                oProducto = new Producto()
                                {
                                    IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    oMarca = new Marca() { Descripcion = dr["Descripcion"].ToString() },
                                    Precio = Convert.ToDecimal(dr["Precio"], CultureInfo.InvariantCulture),
                                    RutaImagen = dr["RutaImagen"].ToString()
                                },
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                Adicionales = dr["Adicionales"].ToString(),
                                PrecioExtra = Convert.ToDecimal(dr["PrecioExtra"], CultureInfo.InvariantCulture),
                                Observaciones = dr["Observaciones"].ToString() // Agregar el nuevo campo Observaciones
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lst = new List<Carrito>();
                }
            }
            return lst;
        }



        public bool Eliminar(string IdCarrito, string IdProducto)
        {

            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("delete from carrito where idcarrito = @idcarrito");


                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("@idcarrito", IdCarrito);
                    cmd.Parameters.AddWithValue("@idproducto", IdProducto);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
        //public List<Compra> ObtenerCompra()
        //{
        //    List<Compra> rptDetalleCompra = new List<Compra>();

        //    using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
        //    {
        //        SqlCommand cmd = new SqlCommand("sp_ObtenerCompra", oConexion);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        try
        //        {
        //            oConexion.Open();
        //            using (SqlDataReader dr = cmd.ExecuteReader())
        //            {
        //                while (dr.Read())
        //                {
        //                    Compra compra = new Compra
        //                    {
        //                        IdCompra = Convert.ToInt32(dr["IdCompra"]),
        //                        TotalProducto = Convert.ToInt32(dr["TotalProducto"]),
        //                        Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-PE")),
        //                        Nombre = dr["Nombre"].ToString(),
        //                        Telefono = dr["Telefono"].ToString(),
        //                        Correo = dr["Correo"].ToString(),
        //                        Direccion = dr["Direccion"].ToString(),
        //                        Referencia = dr["Referencia"].ToString(),
        //                        DocumentoFacturacion = dr["DocumentoFacturacion"].ToString(),
        //                        FormaPago = dr["FormaPago"].ToString(),
        //                        FechaTexto = dr["FechaTexto"].ToString(),
        //                        oDetalleCompra = new List<DetalleCompra>()
        //                    };

        //                    string detalleXml = dr["DETALLE_PRODUCTO"].ToString();
        //                    XDocument doc = XDocument.Parse(detalleXml);
        //                    foreach (var producto in doc.Descendants("PRODUCTO"))
        //                    {
        //                        DetalleCompra detalle = new DetalleCompra
        //                        {
        //                            IdDetalleCompra = Convert.ToInt32(producto.Element("IdDetalleCompra").Value),
        //                            IdCompra = Convert.ToInt32(producto.Element("IdCompra").Value),
        //                            IdProducto = Convert.ToInt32(producto.Element("IdProducto").Value),
        //                            oProducto = new Producto
        //                            {
        //                                oMarca = new Marca { Descripcion = producto.Element("Nombre").Value },
        //                                Nombre = producto.Element("Nombre").Value,
        //                                RutaImagen = producto.Element("RutaImagen").Value
        //                            },
        //                            Total = Convert.ToDecimal(producto.Element("Total").Value, new CultureInfo("es-PE")),
        //                            Cantidad = Convert.ToInt32(producto.Element("Cantidad").Value),
        //                            Adicionales = producto.Element("Adicionales").Value
        //                        };
        //                        compra.oDetalleCompra.Add(detalle);
        //                    }

        //                    rptDetalleCompra.Add(compra);
        //                }

        //                dr.Close();
        //            }

        //            return rptDetalleCompra;
        //        }
        //        catch (Exception ex)
        //        {
        //            // Manejo de excepciones aquí si es necesario

        //            return null;
        //        }
        //    }
        //}


        public List<Compra> ObtenerCompra()
        {
            List<Compra> rptDetalleCompra = new List<Compra>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerCompra", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;


                try
                {
                    oConexion.Open();
                    using (XmlReader dr = cmd.ExecuteXmlReader())
                    {
                        while (dr.Read())
                        {
                            XDocument doc = XDocument.Load(dr);
                            if (doc.Element("DATA") != null)
                            {
                                rptDetalleCompra = (from c in doc.Element("DATA").Elements("COMPRA")
                                                    select new Compra()
                                                    {
                                                        Referencia = c.Element("Referencia").Value,
                                                        FormaPago = c.Element("FormaPago").Value,
                                                        Nombre = c.Element("Nombre").Value,
                                                        DocumentoFacturacion = c.Element("DocumentoFacturacion").Value,
                                                        Telefono = c.Element("Telefono").Value,
                                                        Direccion = c.Element("Direccion").Value,
                                                        Correo = c.Element("Correo").Value,
                                                        Total = Convert.ToDecimal(c.Element("Total").Value, new CultureInfo("es-PE")),
                                                        FechaTexto = c.Element("Fecha").Value,
                                                        oDetalleCompra = (from d in c.Element("DETALLE_PRODUCTO").Elements("PRODUCTO")
                                                                          select new DetalleCompra()
                                                                          {

                                                                              oProducto = new Producto()
                                                                              {
                                                                                  oMarca = new Marca() { Descripcion = d.Element("Descripcion").Value },
                                                                                  Nombre = d.Element("Nombre").Value,
                                                                                  RutaImagen = d.Element("RutaImagen").Value
                                                                              },
                                                                              Adicionales = d.Element("Adicionales").Value,
                                                                              Total = Convert.ToDecimal(d.Element("Total").Value, new CultureInfo("es-PE")),
                                                                              Cantidad = Convert.ToInt32(d.Element("Cantidad").Value)
                                                                          }).ToList()
                                                    }).ToList();
                            }
                            else
                            {
                                rptDetalleCompra = new List<Compra>();
                            }
                        }

                        dr.Close();

                    }

                    return rptDetalleCompra;
                }
                catch (Exception ex)
                {
                    rptDetalleCompra = null;
                    return rptDetalleCompra;
                }
            }
        }




    }
}