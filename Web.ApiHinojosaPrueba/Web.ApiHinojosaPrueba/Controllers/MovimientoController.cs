using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ApiHinojosaPrueba.Datos;
using Web.ApiHinojosaPrueba.Modelos;
using Web.ApiHinojosaPrueba.Utilitario;

namespace Web.ApiHinojosaPrueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoController : ControllerBase
    {
        private readonly BaseDatosContext _contexto;

        private static readonly decimal Valor_diario = 1000;
        private static readonly string Debito = "Retiro";

        public MovimientoController(BaseDatosContext contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        public async Task<ActionResult<List<Movimiento>>> Consultar()
        {
            return await _contexto.Movimientos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Resultado> ConsultarId(int id)
        {
            Resultado resulltado = new();
            try
            {
                resulltado.Respuesta = await _contexto.Movimientos.FindAsync(id);
                resulltado.Exito = true;
                if (resulltado.Respuesta == null)
                {
                    resulltado.Mensaje = "Datos no existentes";
                }
            }
            catch (Exception e)
            {
                resulltado.Mensaje = "Error: " + e.StackTrace;
                resulltado.Exito = false;
            }
            return resulltado;
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Modificar(int id, Movimiento movimiento)
        {
            if (id != movimiento.MovIdMovimiento)
            {
                return BadRequest();
            }

            _contexto.Entry(movimiento).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_contexto.Movimientos.Any(e => e.MovIdMovimiento == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<Resultado> Insertar(Movimiento movimiento)
        {
            Resultado resultado = new();
            movimiento.MovFecha = DateTime.Now;
            #region Validacion campos vacios
            if (movimiento.MovMovimientoValor <= 0)
            {
                resultado.Exito = true;
                resultado.Mensaje = "Valor minimo $1.00";
                return resultado;
            }
            #endregion

            Movimiento ultimoMovimiento = await _contexto.Movimientos.Where(x => x.CueNumero == movimiento.CueNumero).OrderByDescending(x => x.MovFecha).FirstOrDefaultAsync();

            if (ultimoMovimiento != null)
            {
                if (string.Equals(movimiento.MovTipo, Debito))
                {
                    resultado = await Validacion(movimiento.CueNumero, movimiento.MovMovimientoValor);
                    if (!resultado.Exito)
                    {
                        return resultado;
                    }
                }

                if (string.Equals(movimiento.MovTipo, Debito) && ultimoMovimiento.MovSaldoActual < Math.Abs(movimiento.MovMovimientoValor))
                {
                    resultado.Exito = true;
                    resultado.Mensaje = "Saldo insuficiente";
                }
                else
                {
                    movimiento.MovSaldoInicial = ultimoMovimiento.MovSaldoActual;
                    if (string.Equals(movimiento.MovTipo, Debito))
                    {
                        movimiento.MovSaldoActual = ultimoMovimiento.MovSaldoActual - movimiento.MovMovimientoValor;
                        movimiento.MovMovimientoValor = Math.Abs(movimiento.MovMovimientoValor) * (-1);
                    }
                    else
                        movimiento.MovSaldoActual = ultimoMovimiento.MovSaldoActual + movimiento.MovMovimientoValor;
                    resultado.Exito = true;
                    _contexto.Movimientos.Add(movimiento);
                    await _contexto.SaveChangesAsync();
                }
            }
            else
            {
                if (string.Equals(movimiento.MovTipo, Debito))
                {
                    movimiento.MovSaldoActual = movimiento.MovMovimientoValor;
                    movimiento.MovMovimientoValor = Math.Abs(movimiento.MovMovimientoValor) * (-1);
                }
                else
                    movimiento.MovSaldoActual = movimiento.MovMovimientoValor;
                resultado.Exito = true;
                _contexto.Movimientos.Add(movimiento);
                await _contexto.SaveChangesAsync();
            }

            resultado.Respuesta = CreatedAtAction("ConsultarId", new { id = movimiento.MovIdMovimiento }, movimiento);
            return resultado;
        }

        [HttpPost("{NumeroCuenta},{Monto}")]
        private async Task<Resultado> Validacion(string NumeroCuenta, decimal Monto)
        {
            Resultado resultado = new();
            DateTime diaActual = DateTime.Now;

            string dia = diaActual.ToString("dd-MM-yyyy");
            DateTime InicioDeDia = DateTime.ParseExact(dia, "dd-MM-yyyy", null);
            DateTime FinalDeDia = DateTime.ParseExact(dia + "23:59:59", "dd-MM-yyyy HH:mm:ss", null);

            decimal valoresRetiro = await _contexto.Movimientos
                .Where(x => x.CueNumero == NumeroCuenta
                && x.MovFecha >= InicioDeDia
                && x.MovFecha <= FinalDeDia
                && x.MovTipo == Debito)
                .SumAsync(a => a.MovMovimientoValor);

            decimal totalsupuesto = Math.Abs(valoresRetiro) + Math.Abs(Monto);

            resultado.Respuesta = Math.Abs(totalsupuesto);
            if (totalsupuesto > Valor_diario || totalsupuesto > Valor_diario)
            {
                resultado.Mensaje = "Su cupo diario fue exedido";
                resultado.Exito = false;
                resultado.Respuesta = Math.Abs(valoresRetiro);
            }
            else
                resultado.Exito = true;
            return resultado;
        }


        [HttpDelete("{id}")]
        public async Task<Resultado> Eliminar(int id)
        {
            Resultado oResultado = new();
            try
            {
                var oMovimiento = await _contexto.Movimientos.FindAsync(id);
                oResultado.Respuesta = _contexto.Movimientos.Remove(oMovimiento);
                await _contexto.SaveChangesAsync();
                oResultado.Exito = true;
            }
            catch (Exception ex)
            {
                oResultado.Mensaje = "Error: " + ex.StackTrace;
            }

            return oResultado;
        }

        [HttpGet("{Identificacion}&{FechaInicio}&{FechaFin}")]
        public async Task<Resultado> Reporte(string Identificacion, string FechaInicio, string FechaFin)
        {
            List<Movimiento> lstMovimiento = new();
            Resultado resultado = new();
            try
            {
                resultado.Respuesta = await _contexto.Movimientos.Select(x => new VmoMovimientoCliente
                {
                    Fecha = x.MovFecha,
                    Nombre = x.MoNumeroCuentaNavigation.CliIdClienteNavigation.CliNombre,
                    NumeroCuenta = x.MoNumeroCuentaNavigation.CueNumero,
                    Tipo = x.MoNumeroCuentaNavigation.CueTipo,
                    SaldoInicial = x.MovSaldoInicial,
                    Estado = x.MoNumeroCuentaNavigation.CliIdClienteNavigation.CliEstado,
                    Movimiento = x.MovMovimientoValor,
                    SaldoDisponible = x.MovSaldoActual,
                    Identificacion = x.MoNumeroCuentaNavigation.CliIdClienteNavigation.CliIdentificacion,

                }).Where(s => s.Identificacion == Identificacion && s.Fecha >= Convert.ToDateTime(FechaInicio) && s.Fecha <= Convert.ToDateTime(FechaFin)).ToListAsync();
                resultado.Exito = true;
            }
            catch (Exception e)
            {
                resultado.Mensaje = "Error: " + e.StackTrace;
            }
            return resultado;
        }
    }
}
